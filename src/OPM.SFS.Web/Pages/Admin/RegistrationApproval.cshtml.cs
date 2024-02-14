using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OPM.SFS.Core.Shared;
using OPM.SFS.Data;
using OPM.SFS.Web.Models;
using OPM.SFS.Web.Shared;
using OPM.SFS.Web.SharedCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static OPM.SFS.Web.Models.AdminStudentSearchViewModel;

namespace OPM.SFS.Web.Pages.Admin
{
    [Authorize(Roles = "AD")]
    public class RegistrationApprovalModel : PageModel
    {
        [BindProperty]
        public AdminRegisterApprovalViewModel Data { get; set; }

        [FromQuery(Name = "t")]
        public string ProfileType { get; set; }

        private readonly IMediator _mediator;

        public RegistrationApprovalModel(IMediator mediator) => _mediator = mediator;
		public async Task OnGetAsync()
		{
			Data = await _mediator.Send(new Query() { Type = ProfileType} );
		}
		public async Task<IActionResult> OnGetApproveAccount(int id, string type)
		{
            _ = await _mediator.Send(new Command() { ID = id, Type = type, Status = "approve" });
            return RedirectToPage("/Admin/RegistrationApproval", new { t = type });
        }
        public async Task<IActionResult> OnGetRejectAccount(int id, string type)
        {
            _ = await _mediator.Send(new Command() { ID = id, Type = type, Status = "reject" });
            return RedirectToPage("/Admin/RegistrationApproval", new { t = type });
        }


        public class Query : IRequest<AdminRegisterApprovalViewModel>
        {
            public string Type { get; set; }
        }

        public class QueryHandler : IRequestHandler<Query, AdminRegisterApprovalViewModel>
        {
            private readonly ScholarshipForServiceContext _efDB;
            private readonly ICacheHelper _cache;
            private readonly IFeatureManager _feature;

            public QueryHandler(ScholarshipForServiceContext efDB, ICacheHelper cache, IFeatureManager feature)
            {
                _efDB = efDB;
                _cache = cache;
                _feature = feature;
            }

            public async Task<AdminRegisterApprovalViewModel> Handle(Query request, CancellationToken cancellationToken)
            {
                AdminRegisterApprovalViewModel model = new();
                model.Accounts = new();

                var allStatus = await _cache.GetProfileStatusAsync();
                var allAgencies = await _cache.GetAgenciesAsync();
                var pendingID = allStatus.Where(m => m.Name == "Pending").Select(m => m.ProfileStatusID).FirstOrDefault();
                model.IsEnabledOnSite = await _feature.IsEnabledSiteWideAsync("UnregisteredStudentFlow");
				if (request.Type == "students")
                {

                    var pendingStudents = await _efDB.Students
                        .Include(m => m.StudentInstitutionFundings)
                        .Where(m => m.ProfileStatusID == pendingID)                       
                        .Select(m => new
                        {
                            ID = m.StudentId,
                            UID = m.StudentUID,
                            Firstname = m.FirstName,
                            Lastname = m.LastName,
                            Email = m.Email,
                            Institution = m.StudentInstitutionFundings.Select(m => m.Institution.Name).FirstOrDefault(),
                        })
                        .ToListAsync();
                    model.Type = "students";
                    foreach (var student in pendingStudents)
                    {
                        model.Accounts.Add(new AdminRegisterApprovalViewModel.UserItem()
                        {
                            ID = student.ID,
                            UID = student.UID,
                            LastName = student.Lastname,
                            FirstName = student.Firstname,
                            Email = student.Email,
                            Instituion = student.Institution
                        });
                    }
                    return model;
                }

                if(request.Type == "agencyusers")
                {
                    var pendingAOs = await _efDB.AgencyUsers
                        .Where(m => m.ProfileStatusID == pendingID)
                        .Include(m => m.Agency)
                        .Select(m => new
                        {
                            ID = m.AgencyUserId,
                            Firstname = m.Firstname,
                            Lastname = m.Lastname,
                            Email = m.Email,
                            Phone = m.Address.PhoneNumber,
                            Agency = m.Agency.Name,
                            ParentAgency = m.Agency.ParentAgencyId
                        })
                        .ToListAsync();
                    model.Type = "agencyusers";
                    foreach (var ao in pendingAOs)
                    {
                        AdminRegisterApprovalViewModel.UserItem item = new();
                        item.ID = ao.ID;
                        item.LastName = ao.Lastname;
                        item.FirstName = ao.Firstname;
                        item.Email = ao.Email;
                        item.Telephone = ao.Phone;
                        if (ao.ParentAgency.HasValue)
                        {
                            item.Agency = allAgencies.Where(m => m.AgencyId == ao.ParentAgency).Select(m => m.Name).FirstOrDefault();
                            item.SubAgency = ao.Agency;
                        }
                        else
                            item.Agency = ao.Agency;
                        model.Accounts.Add(item);
                    }
                }

                if(request.Type == "academiausers")
                {
                    var pendingPIs = await _efDB.AcademiaUsers
                        .Where(m => m.ProfileStatusID == pendingID)
                        .Select(m => new
                        {
                            ID = m.AcademiaUserId,
                            Lastname = m.Lastname,
                            Firstname = m.Firstname,
                            Email = m.Email,
                            Institution = m.Institution.Name
                        })
                        .ToListAsync();
                    model.Type = "academiausers";
                    foreach (var pi in pendingPIs)
                    {
                        model.Accounts.Add(new AdminRegisterApprovalViewModel.UserItem()
                        {
                            ID = pi.ID,
                            LastName = pi.Lastname,
                            FirstName = pi.Firstname,
                            Email = pi.Email,
                            Instituion = pi.Institution                          
                        });
                    }
                    return model;
                }


                return model;
            }
        }

        public class Command : IRequest<bool>
        {
            public int ID { get; set; }
            public string Type { get; set; }
            public string Status { get; set; }
            //public AdminRegisterApprovalViewModel Model { get; set; }
        }

        public class CommandHandler : IRequestHandler<Command, bool>
        {

            private readonly ScholarshipForServiceContext _efDB;
            private readonly ICacheHelper _cache;
            private readonly IEmailerService _emailer;
            private readonly ICryptoHelper _crypto;
            private readonly IFeatureManager _featureManager;
            private readonly IConfiguration _config;

            public CommandHandler(ScholarshipForServiceContext efDB, ICacheHelper cache, IEmailerService emailer, ICryptoHelper crypto, IFeatureManager featureManager, IConfiguration configuration)
            {
                _efDB = efDB;
                _cache = cache;
                _emailer = emailer;
                _crypto = crypto;
                _featureManager = featureManager;
                _config = configuration;
            }

            public async Task<bool> Handle(Command request, CancellationToken cancellationToken)
            {
                var allStatus = await _cache.GetProfileStatusAsync();
                var active = allStatus.Where(m => m.Name == "Active").Select(m => m.ProfileStatusID).FirstOrDefault();
                var disabled = allStatus.Where(m => m.Name == "Disabled").Select(m => m.ProfileStatusID).FirstOrDefault();
                var inactive = allStatus.Where(m => m.Name == "Inactive").Select(m => m.ProfileStatusID).FirstOrDefault();
				if (request.Type == "students")
                {
                    var studentRecord = await _efDB.Students.Where(m => m.StudentId == request.ID)
                        .Include(m => m.StudentAccount)
                        .Include(m => m.StudentInstitutionFundings)
                        .FirstOrDefaultAsync();
                    if (request.Status == "approve")
                    {
                        //generate password and add to studentaccount
                        var passwordExpirationDate = DateTime.Now.AddHours(24);
                        var tempPassword = Guid.NewGuid().ToString().Replace("-", "");
						var PIContacts = await _efDB.AcademiaUsers
                            .Where(m => m.InstitutionID == studentRecord.StudentInstitutionFundings.FirstOrDefault().InstitutionId)
                            .Where(m => m.ProfileStatusID == active || m.ProfileStatusID == inactive)
                            .ToListAsync();
                            
						studentRecord.ProfileStatusID = active;
                        studentRecord.RegistrationApprovalDate = DateTime.Now;
                        if(studentRecord.StudentAccount != null)
                        {
                            studentRecord.StudentAccount.Password = _crypto.ComputeSha512Hash(tempPassword);
                            studentRecord.StudentAccount.PasswordCrypto = "SHA512";
                            studentRecord.StudentAccount.ForcePasswordReset = true;
                            studentRecord.StudentAccount.PasswordExpiration = passwordExpirationDate;
                        }
                        else
                        {
                            studentRecord.StudentAccount = new StudentAccount()
                            {
                                Password = _crypto.ComputeSha512Hash(tempPassword),
                                PasswordCrypto = "SHA512",
                                ForcePasswordReset = true,
                                PasswordExpiration = passwordExpirationDate

                            };
                        }
                      
                        await _efDB.SaveChangesAsync();
                        await SendApprovedEmailAsync(studentRecord.Email, tempPassword, studentRecord.FirstName , studentRecord.StudentAccount.PasswordExpiration);
						DateTime DueDate = (DateTime.Now).AddDays(10);
                        if (PIContacts != null)
                        {
                            foreach (var PIContact in PIContacts)
                            {
                                string emailContent = $@"Hello, {PIContact.Firstname} {PIContact.Lastname} <br/><br/>
                                    Student, {studentRecord.FirstName} {studentRecord.LastName}, is now registered with the SFS program. <br/><br/>
                                    Please follow up with the student to ensure they create or upload a resume and make it available to hiring managers by {DueDate}. 
                                    You may also track the status of this action by logging into the PI portal and navigating to the students profile/document repository.<br/><br/>
                                    The students email address on record is: {studentRecord.Email} <br/><br/>
                                    Please contact the SFS program office at sfs@opm.gov if you have any questions.<br/><br/>";
                                await _emailer.SendEmailDefaultTemplateAsync(PIContact.Email, "A student has registered", emailContent);
                            }
                        }
					}
                    else
                    {
                        string email = studentRecord.Email;
                        string name = studentRecord.FirstName + " " + studentRecord.LastName;
                        _efDB.Students.Remove(studentRecord);
                        await _efDB.SaveChangesAsync();
                        await SendRejectEmailAsync(email, name, request.Type);
                    }                 
                    return true;
                }

                if (request.Type == "agencyusers")
                {
                    var aoRecord = await _efDB.AgencyUsers.Where(m => m.AgencyUserId == request.ID).FirstOrDefaultAsync();
                    if (request.Status == "approve")
                    {

                        aoRecord.ProfileStatusID = active;
                        await _efDB.SaveChangesAsync();
                    }
                    else
                    {
                        _efDB.AgencyUsers.Remove(aoRecord);
                        await _efDB.SaveChangesAsync();
                        string name = aoRecord.Firstname + " " + aoRecord.Lastname;
                        await SendRejectEmailAsync(aoRecord.Email, name, request.Type);
                    }
                    return true;
                }

                if (request.Type == "academiausers")
                {
                    var piRecord = await _efDB.AcademiaUsers.Where(m => m.AcademiaUserId == request.ID).FirstOrDefaultAsync();
                    if (request.Status == "approve")
                    {

                        piRecord.ProfileStatusID = active;
                        await _efDB.SaveChangesAsync();                   
                    }
                    else
                    {
                        _efDB.AcademiaUsers.Remove(piRecord);
                        await _efDB.SaveChangesAsync();
                        string name = piRecord.Firstname + " " + piRecord.Lastname;
                        await SendRejectEmailAsync(piRecord.Email, name, request.Type);
                    }
                    return true;
                }              

                return false;
            }

            private async Task SendApprovedEmailAsync(string email, string password, string name,DateTime? passwordExpiration)
            {
                string baseUrl = _config["General:BaseUrl"];
                if (await _featureManager.IsEnabledSiteWideAsync("UnregisteredStudentFlow"))
                {                  
                    var secureLink = $"{baseUrl}/Student/Login";
                    string ApprovalEmailContent = $@"Hello {name}, <br /><br/>
                                         Thank you for registering with the Scholarship for Service (SFS) Program. Your registration has been approved. Further ACTION IS REQUIRED for your registration to be completed.<br/><br/>
                                         Please log in to the SFS portal ({secureLink}) and select the <b>Documents/Resumes</b> tab to upload or build a resume. You must add your resume within 10 days after receiving this email.<br/><br/>
                                         Please contact the SFS program office at sfs@opm.gov if you have any questions.<br/><br/>";

                    await _emailer.SendEmailDefaultTemplateAsync(email, "SFS Registration ACTION REQUIRED", ApprovalEmailContent);

                }
                else
                {
                    string helpLink = $"{baseUrl}/Help/Account";
                    string ApprovalEmailContent = $@"Congratulations {name}! <br/><br/> Your online registration to access the Scholarship For Service (SFS)
                                                Program website has been approved. You will now have access to the login options. Login access to the SFS site is through login.gov. 
                                                All new users must first setup their login.gov account. 
                                                See the SFS <a href='{helpLink}'>help center</a> for further guidance. <br/><br/>                                              
                                                You have taken the first step in joining the Federal Government's elite cadre of Information
                                                Assurance professionals. You must now enter the SFS website to complete an online resume as
                                                soon as possible, but no later than five business days from the date of this notice. 
                                                This is an automated email that is generated when the SFS Program Office approves your
                                                registration. <br/><br/> If you have any problems or need assistance, send an email to the SFS
                                                Program Office at sfs@opm.gov or simply reply to this email confirmation describing the
                                                problems you have encountered.";


                    await _emailer.SendEmailDefaultTemplateAsync(email, "SFS Registration Approved", ApprovalEmailContent);
                    EncryptionKeys _keys = new EncryptionKeys();
                   
                }
            }

            private async Task SendRejectEmailAsync(string email, string name, string userType)
            {
                string DisapprovalEmailContent = "";

                if (userType == "students")
                {
                    DisapprovalEmailContent = $@"Hello {name}, <br/><br/>  Thank you for your interest in the CyberCorps®: Scholarship for Service (SFS) program. 
                                        The registration request you submitted has been declined. This registration site is restricted to students who have already 
                                        applied, been selected, and awarded a scholarship under the SFS program. A review of our records shows that you have not been 
                                        awarded a scholarship under the SFS program. If you believe this is incorrect, please contact the SFS program office at sfs@opm.gov. 
                                        <br/><br/>
                                        Resources: <br/><br/>
                                        SFS program information and eligibility requirements can be found at https://www.sfs.opm.gov/Student/Information <br/><br/>
                                        Participating academic institutions can be found at https://www.sfs.opm.gov/Academia/Institutions <br/><br/>
                                        SFS resources can be found at https://www.sfs.opm.gov/Student/Resources
                                        <br/><br/>
                                        Thank you, <br/><br/>
                                        SFS Program Office";
                }
                else if(userType == "agencyusers")
                {
                        DisapprovalEmailContent = $@"Hello {name}, <br/><br/> Thank you for your interest in the CyberCorps®: Scholarship for Service 
                                    (SFS) program. Our records show you recently attempted to register as an Agency Official. We have been unable to 
                                    confirm you are an authorized user. If you believe this is incorrect, please contact the SFS program office at sfs@opm.gov. <br/><br/>
                                    Resources: https://www.sfs.opm.gov/Agency/Resources
                                    <br/><br/>
                                    Thank you, <br/><br/>
                                    SFS Program Office";
                }
                else if(userType == "academiausers")
                {
                    DisapprovalEmailContent = $@"Hello {name}, <br/><br/> Thank you for your interest in the CyberCorps®: Scholarship 
                                    for Service (SFS) program. Our records show you recently attempted to register as a Principal Investigator. 
                                    We have been unable to confirm you are an authorized user. If you believe this is incorrect, please contact 
                                    your academic institution’s SFS program office: https://www.sfs.opm.gov/Academia/Institutions. <br/><br/>
                                    Resources: https://www.sfs.opm.gov/Academia/Resources                                   
                                    <br/><br/>
                                    Thank you, <br/><br/>
                                    SFS Program Office";
                }

                await _emailer.SendEmailNoTemplateAsync(email, "SFS Registration", DisapprovalEmailContent);
            }
        }
    }
}
