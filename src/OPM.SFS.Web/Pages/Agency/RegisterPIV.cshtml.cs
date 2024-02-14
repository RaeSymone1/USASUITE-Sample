using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
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
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OPM.SFS.Web.Pages.Agency
{
    public class RegisterPIVModel : PageModel
    {
        [BindProperty]
        public RegisterPivViewModel Data { get; set; }

        [FromQuery(Name = "c")]
        public string StageID { get; set; }

        [FromQuery(Name = "i")]
        public int AgencyUserID { get; set; }

        private readonly IMediator _mediator;

        public RegisterPIVModel(IMediator mediator) => _mediator = mediator;

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();
            var registerRequest = await _mediator.Send(new LoginCommand() { Model = Data, ID = StageID });
            Data.Message = "Email has been sent successfully. Please check your email to confirm your Smartcard registration.";
            return Page();
        }       

        public class RegisterPivViewModelValidator : AbstractValidator<RegisterPivViewModel>
        {
            public RegisterPivViewModelValidator()
            {

                RuleFor(request => request.Email).NotEmpty().EmailAddress();
                RuleFor(request => request.Email).Matches("^[^><&]+$");
            }
        }

        public class LoginCommand : IRequest<RegisterPIVResult>
        {
            public string ID { get; set; }
            public RegisterPivViewModel Model { get; set; }
            public int AgencyUserID { get; set; }

        }

        public class LoginCommandHandler : IRequestHandler<LoginCommand, RegisterPIVResult>
        {
            private readonly ScholarshipForServiceContext _db;
            private readonly IHttpContextAccessor _httpContextAccessor;
            private readonly ICryptoHelper _crypto;
            private readonly IEmailerService _emailer;
            private readonly IConfiguration _appSettings;
            public readonly IAuditEventLogHelper _auditLogger;
            private readonly IUtilitiesService _utilities;

            public LoginCommandHandler(ScholarshipForServiceContext db, IHttpContextAccessor httpContextAccessor, ICryptoHelper crypto,
                IEmailerService emailer, IConfiguration appSettings, IAuditEventLogHelper audit, IUtilitiesService utilities)
            {
                _db = db;
                _httpContextAccessor = httpContextAccessor;
                _crypto = crypto;
                _emailer = emailer;
                _appSettings = appSettings;
                _auditLogger = audit;
                _utilities = utilities;
            }

            public async Task<RegisterPIVResult> Handle(LoginCommand request, CancellationToken cancellationToken)
            {
                
                string baseUrl = _appSettings["General:BaseUrl"];

                //Confirm that email is valid SFS account
                var agencyUser = await _db.AgencyUsers.Where(m => m.Email == request.Model.Email)
                    .Include(m => m.ProfileStatus)
                    .FirstOrDefaultAsync();

                if (agencyUser != null)
                {
                    var linkExpireDate = DateTime.UtcNow.AddMinutes(55);
                    string linkExpireDateDisplayEST = _utilities.ConvertUtcToEastern(linkExpireDate).ToString("MM/dd/yyyy h:mm tt");
					var secureLink = $"{baseUrl}/Agency/RegisterPivConfirm?c={request.ID}&i={agencyUser.AgencyUserId}";
                    string emailContent = $@"Hello {agencyUser.Firstname}, <br /><br /> 
                                        Please click on the following link to confirm and proceed with registering your Smartcard with your SFS account.
                                        <br/> {secureLink} <br/>
                                        This link will expire on {linkExpireDateDisplayEST} <br />
                                        If you did not request this action or need additional assistance, please send an email to sfs@opm.gov. <br /><br />
                                        Sincerely, <br /><br />
                                        The SFS Help Desk";

                    await _emailer.SendEmailDefaultTemplateAsync(agencyUser.Email, "SFS Confirm Smartcard Registration", emailContent);
                    await _auditLogger.LogAuditEvent($"Smartcard: User {agencyUser.AgencyID} Role AO generated a smartcard registration email");
                }
                
              
                return new RegisterPIVResult() { IsSuccess = true };

            }
        }

    }
}
