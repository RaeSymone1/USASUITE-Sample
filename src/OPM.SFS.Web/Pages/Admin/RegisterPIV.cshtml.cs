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

namespace OPM.SFS.Web.Pages.Admin
{

    public class RegisterPIVModel : PageModel
    {
        [BindProperty]
        public RegisterPivViewModel Data { get; set; }

        [FromQuery(Name = "c")]
        public string StageID { get; set; }

        private readonly IMediator _mediator;

        public RegisterPIVModel(IMediator mediator) => _mediator = mediator;

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();
            var registerRequest = await _mediator.Send(new LoginCommand() { Model = Data, ID = StageID });
            Data.Message = registerRequest.Message;
            Data.ErrorMessage = registerRequest.ErrorMessage;
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

        }

        public class LoginCommandHandler : IRequestHandler<LoginCommand, RegisterPIVResult>
        {
            private readonly ScholarshipForServiceContext _db;
            private readonly IEmailerService _emailer;
            private readonly IConfiguration _appSettings;
            public readonly IAuditEventLogHelper _auditLogger;
            private readonly IUtilitiesService _utilties;

            public LoginCommandHandler(ScholarshipForServiceContext db, IEmailerService emailer, IConfiguration appSettings, IAuditEventLogHelper audit, IUtilitiesService utilities )
            {
                _db = db;
                _emailer = emailer;
                _appSettings = appSettings;
                _auditLogger = audit;
                _utilties = utilities;
            }

            public async Task<RegisterPIVResult> Handle(LoginCommand request, CancellationToken cancellationToken)
            {
                string baseUrl = _appSettings["General:BaseUrl"];

                //Confirm that email is valid SFS account
                var adminUser = await _db.AdminUsers.Where(m => m.Email == request.Model.Email)
                    .FirstOrDefaultAsync();

                if (adminUser != null)
                {
                    var expirationDate = DateTime.UtcNow.AddMinutes(55);
                    string expirationDateDisplayEST = _utilties.ConvertUtcToEastern(expirationDate).ToString("MM/dd/yyyy h:mm tt"); 
                    var secureLink = $"{baseUrl}/Admin/RegisterPivConfirm?c={request.ID}&i={adminUser.AdminUserId}";
                    string emailContent = $@"Hello {adminUser.FirstName}, <br /><br /> 
                                        Please click on the following link to confirm and proceed with registering your Smartcard with your SFS account.
                                        <br/> {secureLink} <br/>
                                        This link will expire on {expirationDateDisplayEST} <br />
                                        If you did not request this action or need additional assistance, please send an email to sfs@opm.gov. <br /><br />
                                        Sincerely, <br /><br />
                                        The SFS Help Desk";

                    await _emailer.SendEmailNoTemplateAsync(adminUser.Email, "SFS Confirm Smartcard Registration", emailContent);
                    await _auditLogger.LogAuditEvent($"Smartcard: User {adminUser.AdminUserId} Role AD generated a smartcard registration email");
                    return new RegisterPIVResult() { IsSuccess = true, Message = "An email will be sent if your account is found. Please check your email to confirm your Smartcard registration." };
                }
                return new RegisterPIVResult() { IsSuccess = false, ErrorMessage = $"Account with email {request.Model.Email} not found." };

            }

            
        }


    }
}
