using Medallion.Threading.SqlServer;
using Microsoft.EntityFrameworkCore;
using OPM.SFS.Core.Shared;
using OPM.SFS.Data;
using Sgbj.Cron;
using System;

namespace OPM.SFS.TaskProcessor.Tasks
{
    /// <summary>
    /// Checks for Students who have a pending registration for 14 days. Queues email to remind
    /// students to complete their registration
    /// </summary>
    public class StudentRegistrationReminderTask : BackgroundService
    {
        public readonly ScholarshipForServiceContext _efDB;
        private readonly ILogger _logger;
        private readonly IEmailerService _emailer;
        private readonly ICryptoHelper _crypto;
        private readonly IConfiguration _appSettings;

        public StudentRegistrationReminderTask(ILogger<StudentRegistrationReminderTask> logger, IServiceProvider serviceProvider, 
            IEmailerService emailer, ICryptoHelper crypto, IConfiguration appSettings)
        {            
            _logger = logger;
            _efDB = serviceProvider.CreateScope().ServiceProvider.GetRequiredService<ScholarshipForServiceContext>();
            _emailer = emailer;
            _crypto = crypto;
            _appSettings = appSettings;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {            
            var task = await _efDB.ScheduledTask.Where(m => m.Name == "StudentRegistrationReminderTask").FirstOrDefaultAsync();
            if (!task.IsDisabled)
            {
                using var timer = new CronTimer(task.Schedule, TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time"));
                while (await timer.WaitForNextTickAsync())
                {
                    var @lock = new SqlDistributedLock("StudentRegistrationReminderLock", _efDB.Database.GetConnectionString());
                    await using (await @lock.AcquireAsync())
                    {
                        _logger.LogInformation("Scheduled Task StudentRegistrationReminderTask running");
						await SetTaskStateAsync("RUNNING");
						var GlobalConfigSettings = await _efDB.GlobalConfiguration.ToListAsync();
                        await SendReminderEmailAsync(GlobalConfigSettings);
                        _logger.LogInformation("Scheduled Task StudentRegistrationReminderTask completed");
						await SetTaskStateAsync("COMPLETE");
					}
                }
            }
            
        }

        private async Task<bool> SendReminderEmailAsync(List<GlobalConfiguration> globalConfigSettings)
        {
            var accounts = await GetAccountsForRemindersAsync(globalConfigSettings);
            string baseUrl = _appSettings["General:BaseUrl"];
            foreach (var a in accounts)
            {                
                var RegistrationCode = _crypto.Encrypt(a.StudentUID, globalConfigSettings);
                var institution = await _efDB.Institutions.Where(m => m.InstitutionId == a.InstitutionID)
               .Select(m => m.Name.Trim()).FirstOrDefaultAsync();

                string emailContent = $@"Hello {a.FirstName}, <br/><br/>
                                   This is a reminder to register on the SFS portal and post your resume using the following registration access code: {RegistrationCode} (Note: Do not share this code). Failure to do so may result in your ineligibility for participation in the SFS program.<br/><br/>
								   To be recognized as a SFS scholar, the following must be completed:<br/><br/>
									• Visit the registration page of the SFS website: {baseUrl}/Student/Registration<br/>
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

        private async Task<List<UnregisteredAccountData>> GetAccountsForRemindersAsync(List<GlobalConfiguration> globalConfigSettings)
        {
            
            var allStatus = await _efDB.ProfileStatus.ToListAsync();
            var unregisteredID = allStatus.Where(m => m.Name == "Not Registered").Select(m => m.ProfileStatusID).FirstOrDefault();
            int reminderDays = Convert.ToInt32(globalConfigSettings.Where(m => m.Key == "UnregisteredReminderDays").Select(m => m.Value).FirstOrDefault());
            var loginDateFilter = DateTime.UtcNow.AddDays(reminderDays * -1);
            DateTime minDate = new DateTime(loginDateFilter.Date.Year, loginDateFilter.Date.Month, loginDateFilter.Date.Day);
            DateTime maxDate = minDate.AddSeconds(86399);
            List<UnregisteredAccountData> usersToEmail = new();
            usersToEmail = await _efDB.Students.Where(m => m.DateAdded >= minDate && m.DateAdded <= maxDate && m.ProfileStatusID == unregisteredID)
                .Select(m => new UnregisteredAccountData()
                {
                    Email = m.Email,
                    FirstName = m.FirstName,
                    StudentUID = m.StudentUID.ToString(),
                    InstitutionID = m.StudentInstitutionFundings.FirstOrDefault().InstitutionId.Value,

                }).ToListAsync();

            return usersToEmail;
        }

		private async Task SetTaskStateAsync(string state)
		{
			var task = await _efDB.ScheduledTask.Where(m => m.Name == "StudentRegistrationReminderTask").FirstOrDefaultAsync();
			task.LastRunDate = DateTime.UtcNow;
			task.State = state;
			await _efDB.SaveChangesAsync();
		}

		public class UnregisteredAccountData
        {
            public string Email { get; set; }
            public string FirstName { get; set; }
            public string StudentUID { get; set; }
            public int InstitutionID { get; set; }
        }
    }
}
