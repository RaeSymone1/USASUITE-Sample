using OPM.SFS.Data;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using OPM.SFS.Web.Shared;
using Microsoft.Extensions.Configuration;
using OPM.SFS.Core.Shared;

namespace OPM.SFS.Web.SharedCode
{

		public interface IUnregisteredReminderEmailService
		{
			Task<bool> SendReminderEmailAsync();
		}

		public class UnregisteredReminderEmailService : IUnregisteredReminderEmailService
		{
			private readonly ScholarshipForServiceContext _db;
			private readonly ICacheHelper _cache;
			public readonly IAuditEventLogHelper _auditLogger;
			private readonly IEmailerService _emailer;
			private readonly IConfiguration _appSettings;
			private readonly IStudentDashboardService _dashboardService;

			public UnregisteredReminderEmailService(ScholarshipForServiceContext db, ICacheHelper cache, IAuditEventLogHelper audit, IEmailerService emailer, IConfiguration appSettings, IStudentDashboardService dashboardService)
			{
				_db = db;
				_cache = cache;
				_auditLogger = audit;
				_emailer = emailer;
				_appSettings = appSettings;
				_dashboardService = dashboardService;
			}

			public async Task<bool> SendReminderEmailAsync()
			{
				var accounts = await GetAccountsForRemindersAsync();
				foreach (var a in accounts)
				{
					var RegistrationCode = await _dashboardService.GenerateRegistrationCode(a.StudentUID.ToString());
					var institution = await _db.Institutions.Where(m => m.InstitutionId == a.InstitutionID)
				   .Select(m => m.Name.Trim()).FirstOrDefaultAsync();

					string emailContent = $@"Hello {a.FirstName}, <br/><br/>
                                   This is a reminder to register on the SFS portal and post your resume using the following registration access code: {RegistrationCode} (Note: Do not share this code). Failure to do so may result in your ineligibility for participation in the SFS program.<br/><br/>
								   To be recognized as a SFS scholar, the following must be completed:<br/><br/>
									• Visit the registration page of the SFS website: https://sfs.opm.gov/Student/Registration<br/>
								    • Enter the registration access code provided above<br/>
									• Complete and submit the registration form<br/>
									• After registration is approved, you will receive an email providing you first-time login instructions.<br/><br/>
									• NOTE: You will need to create a login.gov account to sign into SFS and access your profile information. For assistance, visit:  https://sfs.opm.gov/Help/Account <br/><br/>
									Within the SFS portal you will find valuable information and resources regarding the program, including student guidance materials which provide information on each phase of the SFS Program.<br/><br/>
									<i><b>Consequences of failure to provide information: </b></i><br/>
								    Furnishing the data requested is voluntary, but failure to do so will delay or make it impossible for us to process your registration. Registering on the SFS system and completing an online resume is required for program participation.<br/><br/>
									If you have any questions, contact your institution’s Scholarship for Service Program Principal Investigator (PI) or the SFS Program Office.<br/><br/>";
					await _emailer.SendEmailDefaultTemplateAsync(a.Email, $@"Welcome to the SFS Program - {institution}", emailContent);
				}

				return true;
			}

			public async Task<List<UnregisteredAccountData>> GetAccountsForRemindersAsync()
			{
				var GlobalConfigSettings = await _cache.GetGlobalConfigurationsAsync();
				var allStatus = await _cache.GetProfileStatusAsync();
				var unregisteredID = allStatus.Where(m => m.Name == "Not Registered").Select(m => m.ProfileStatusID).FirstOrDefault();
				int reminderDays = Convert.ToInt32(GlobalConfigSettings.Where(m => m.Key == "UnregisteredReminderDays").Select(m => m.Value).FirstOrDefault());
				var loginDateFilter = DateTime.UtcNow.AddDays(reminderDays * -1);
				DateTime minDate = new DateTime(loginDateFilter.Date.Year, loginDateFilter.Date.Month, loginDateFilter.Date.Day);
				DateTime maxDate = minDate.AddSeconds(86399);
				List<UnregisteredAccountData> usersToEmail = new();
				usersToEmail = await _db.Students.Where(m => m.DateAdded >= minDate && m.DateAdded <= maxDate && m.ProfileStatusID == unregisteredID)
					.Select(m => new UnregisteredAccountData()
					{
						Email = m.Email,
						FirstName = m.FirstName,
						StudentUID =  m.StudentUID.ToString(),
						InstitutionID = m.StudentInstitutionFundings.FirstOrDefault().InstitutionId.Value,

					}).ToListAsync();
							
				return usersToEmail;
			}


		}
		public class UnregisteredAccountData
		{
			public string Email { get; set; }
			public string FirstName { get; set; }
			public string StudentUID { get; set; }
			public int InstitutionID { get; set; }
		}
	
}
