using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OPM.SFS.Core.Shared;
using OPM.SFS.Data;
using OPM.SFS.Web.Models;
using OPM.SFS.Web.Shared;
using OPM.SFS.Web.SharedCode;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OPM.SFS.Web.Pages.Admin
{
    [Authorize(Roles = "AD")]
    public class PasswordAdministrationModel : PageModel
    {
        [BindProperty]
        public AdminPasswordAdminVW Data { get; set; }

        [FromQuery(Name = "s")]
        public string IsSuccess { get; set; }

        [FromQuery(Name = "u")]
        public string EmailTo { get; set; }

        private readonly IMediator _mediator;
        public PasswordAdministrationModel(IMediator mediator) => _mediator = mediator;


        public async Task OnGetAsync()
        {

            Data = await _mediator.Send(new Query());
            if (!string.IsNullOrWhiteSpace(IsSuccess) && IsSuccess == "true")
            {
                Data.ShowSuccessMessage = true;
                Data.Recipient = EmailTo;
            }

        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            Data = await _mediator.Send(new Query() { SearchFilter = Data.SearchFilter, SearchOption = Data.SearchOption });
            return Page();
        }

        public async Task<IActionResult> OnGetSendEmail(int sid, string type)
        {
            var result = await _mediator.Send(new SendEmail() { AccountID = sid, Type = type });
            return Redirect($"/Admin/PasswordAdministration?s=true&u={result.Receipent}");
            
        }

        public class AdminPasswordAdminVWValidator : AbstractValidator<AdminPasswordAdminVW>
        {
            public AdminPasswordAdminVWValidator()
            {
                RuleFor(x => x.SearchFilter).NotEmpty().Matches("^[^><&]+$");
            }
        }

         

        public class Query : IRequest<AdminPasswordAdminVW>
        {
            public string SearchOption { get; set; }
            public string SearchFilter { get; set; }
            public int FilteredID { get; set; }
        }

        public class QueryHandler : IRequestHandler<Query, AdminPasswordAdminVW>
        {
            private readonly ScholarshipForServiceContext _db;
            private readonly ICryptoHelper _crypto;
            private readonly ICacheHelper _cache;

            public QueryHandler(ScholarshipForServiceContext db, ICryptoHelper crypto, ICacheHelper cache)
            {
                _db = db;
                _crypto = crypto;
                _cache = cache;
            }

            public async Task<AdminPasswordAdminVW> Handle(Query request, CancellationToken cancellationToken)
            {
                var GlobalConfigSettings = await _cache.GetGlobalConfigurationsAsync();
                AdminPasswordAdminVW model = new();
                model.Accounts = new();

                if (string.IsNullOrWhiteSpace(request.SearchOption) && string.IsNullOrWhiteSpace(request.SearchFilter))
                {
                    model.SearchOption = "Student";
                    model.SearchResultsMessage = "";
                    return model;
                }
                if(request.SearchOption == "Student")
                {
                    var result = await _db.Students
                           .Where(m => m.Email == request.SearchFilter)
                            .Select(m => new
                            {
                                ID = m.StudentId,
                                Lastname = m.LastName,
                                Firstname = m.FirstName,
                                Email = m.Email
                            })
                            .FirstOrDefaultAsync();
                    if (result == null) return model;
                    model.Accounts.Add(new AdminPasswordAdminVW.Account()
                    {
                         AccountID = result.ID,
                         LastName = result.Lastname,
                         FirstName = result.Firstname,
                         Email = result.Email, 
                         Type = "ST"
                    });
                   
                    return model;
                }

                if (request.SearchOption == "AO")
                {
                    var result = await _db.AgencyUsers
                           .Where(m => m.Email == request.SearchFilter)
                            .Select(m => new
                            {
                                ID = m.AgencyUserId,
                                Lastname = m.Lastname,
                                Firstname = m.Firstname,
                                Email = m.Email
                            })
                            .FirstOrDefaultAsync();
                    if (result == null) return model;
                    model.Accounts.Add(new AdminPasswordAdminVW.Account()
                    {
                        AccountID = result.ID,
                        LastName = result.Lastname,
                        FirstName = result.Firstname,
                        Email = result.Email,
                        Type = "AO"
                    });
                    return model;
                }

                if(request.SearchOption == "PI")
                {
                    var result = await _db.AcademiaUsers
                           .Where(m => m.Email == request.SearchFilter)
                            .Select(m => new
                            {
                                ID = m.AcademiaUserId,
                                Lastname = m.Lastname,
                                Firstname = m.Firstname,
                                Email = m.Email
                            })
                            .FirstOrDefaultAsync();
                    if (result == null) return model;
                    model.Accounts.Add(new AdminPasswordAdminVW.Account()
                    {
                        AccountID = result.ID,
                        LastName = result.Lastname,
                        FirstName = result.Firstname,
                        Email = result.Email,
                        Type = "PI"
                    });
                    return model;
                }
                return model;
            }
        }

        public class SendEmail : IRequest<SendEmailResult>
        {
            public int AccountID { get; set; }
            public string Type { get; set; }
        }

        public class SendEmailHandler : IRequestHandler<SendEmail, SendEmailResult>
        {
            private readonly ScholarshipForServiceContext _db;
            private readonly ICryptoHelper _crypto;
            private readonly IEmailerService _emailer;
            private readonly ICacheHelper _cache;

            public SendEmailHandler(ScholarshipForServiceContext db, ICryptoHelper crypto, IEmailerService emailer, ICacheHelper cache)
            {
                _db = db;
                _crypto = crypto;
                _emailer = emailer;
                _cache = cache;
            }
            public async Task<SendEmailResult> Handle(SendEmail request, CancellationToken cancellationToken)
            {
                string _receipient = "";
                var profileStatusList = await _cache.GetProfileStatusAsync();

                if (request.Type == "ST")
                {
                    var studentRecord = await _db.Students.Where(m => m.StudentId == request.AccountID)
                        .Include(m => m.StudentAccount)
                        .FirstOrDefaultAsync();
                    var passwordExpirationDate = DateTime.Now.AddHours(24);
                    var tempPassword = Guid.NewGuid().ToString().Replace("-", "").Substring(1, 9);

                    studentRecord.StudentAccount.Password = _crypto.ComputeSha512Hash(tempPassword);
                    studentRecord.StudentAccount.PasswordCrypto = "SHA512";
                    studentRecord.StudentAccount.FailedLoginCount = 0;
                    studentRecord.StudentAccount.FailedLoginDate = null;
                    studentRecord.StudentAccount.ForcePasswordReset = true;
                    studentRecord.StudentAccount.PasswordExpiration = passwordExpirationDate;
                    studentRecord.ProfileStatusID = profileStatusList.Where(m => m.Name == "Temporary").Select(m => m.ProfileStatusID).FirstOrDefault();
                    studentRecord.StudentAccount.IsDisabled = false;
                    _receipient = studentRecord.Email;

                    await _db.SaveChangesAsync();


                    string emailContent = $@"Hello {studentRecord.FirstName}, <br /><br /> You have requested a password reminder from the Scholarship for Service web site; 
                                            therefore, the system has generated a temporary password for you as shown below. Please use this temporary password to gain access 
                                            to SFS, at which time you will be prompted to select a new password. If you do not log into your account by {passwordExpirationDate}, 
                                            you will need to request a new temporary password using the forgot password link on the Student Login page. If you did not request 
                                            this action, please send an email to SFS@opm.gov. <br/><br/>
                                            Note:  SFS passwords are case sensitive. <br/><br/>
                                            Password: {tempPassword}";

                    await _emailer.SendEmailNoTemplateAsync(studentRecord.Email, "Access to SFS", emailContent);
                }
                if (request.Type == "AO")
                {
                    var agencyUserRecord = await _db.AgencyUsers.Where(m => m.AgencyUserId == request.AccountID).FirstOrDefaultAsync();
                    var passwordExpirationDate = DateTime.Now.AddHours(24);
                    var tempPassword = Guid.NewGuid().ToString().Replace("-", "").Substring(1, 9);

                    agencyUserRecord.Password = _crypto.ComputeSha512Hash(tempPassword);
                    agencyUserRecord.PasswordCrypto = "SHA512";
                    agencyUserRecord.ForcePasswordReset = true;
                    agencyUserRecord.ProfileStatusID = profileStatusList.Where(m => m.Name == "Temporary").Select(m => m.ProfileStatusID).FirstOrDefault();
                    agencyUserRecord.PasswordExpirationDate = passwordExpirationDate;
                    _receipient = agencyUserRecord.Email;

                    await _db.SaveChangesAsync();


                    string emailContent = $@"Hello {agencyUserRecord.Firstname}, <br /><br /> You have requested a password reminder from the Scholarship for Service web site; 
                                            therefore, the system has generated a temporary password for you as shown below. Please use this temporary password to gain access 
                                            to SFS, at which time you will be prompted to select a new password. If you do not log into your account by {passwordExpirationDate}, 
                                            you will need to request a new temporary password using the forgot password link on the Student Login page. If you did not request 
                                            this action, please send an email to SFS@opm.gov. <br/><br/>
                                            Note:  SFS passwords are case sensitive. <br/><br/>
                                            Password: {tempPassword}";

                    await _emailer.SendEmailNoTemplateAsync(agencyUserRecord.Email, "Access to SFS", emailContent);
                }

                if (request.Type == "PI")
                {
                    var academiaUserRecord = await _db.AcademiaUsers.Where(m => m.AcademiaUserId == request.AccountID).FirstOrDefaultAsync();
                    var passwordExpirationDate = DateTime.Now.AddHours(24);
                    var tempPassword = Guid.NewGuid().ToString().Replace("-", "").Substring(1, 9);

                    academiaUserRecord.Password = _crypto.ComputeSha512Hash(tempPassword);
                    academiaUserRecord.PasswordCrypto = "SHA512";
                    academiaUserRecord.ForcePasswordReset = true;
                    academiaUserRecord.PasswordExpirationDate = passwordExpirationDate;
                    academiaUserRecord.ProfileStatusID = profileStatusList.Where(m => m.Name == "Temporary").Select(m => m.ProfileStatusID).FirstOrDefault();
                    _receipient = academiaUserRecord.Email;
                    await _db.SaveChangesAsync();


                    string emailContent = $@"Hello {academiaUserRecord.Firstname}, <br /><br /> You have requested a password reminder from the Scholarship for Service web site; 
                                            therefore, the system has generated a temporary password for you as shown below. Please use this temporary password to gain access 
                                            to SFS, at which time you will be prompted to select a new password. If you do not log into your account by {passwordExpirationDate}, 
                                            you will need to request a new temporary password using the forgot password link on the Student Login page. If you did not request 
                                            this action, please send an email to SFS@opm.gov. <br/><br/>
                                            Note:  SFS passwords are case sensitive. <br/><br/>
                                            Password: {tempPassword}";

                    await _emailer.SendEmailNoTemplateAsync(academiaUserRecord.Email, "Access to SFS", emailContent);
                }
                return new SendEmailResult() { IsSuccess = true, Receipent = _receipient};
            }
        }

        public class SendEmailResult
        {
            public string Receipent { get; set; }
            public bool IsSuccess { get; set; }
        }
    }
}
