using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OPM.SFS.Core.Shared;
using OPM.SFS.Data;
using OPM.SFS.Web.Shared;
using OPM.SFS.Web.SharedCode;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OPM.SFS.Web.Pages.Admin
{
    public class RegisterPivConfirmModel : PageModel
    {
        [FromQuery(Name = "c")]
        public string StageID { get; set; }

        [FromQuery(Name = "i")]
        public int AdminUserID { get; set; }

        [BindProperty]
        public PIVConfirmResult Data { get; set; }

        private readonly IMediator _mediator;

        public RegisterPivConfirmModel(IMediator mediator) => _mediator = mediator;

        public async Task<IActionResult> OnGetAsync()
        {
            Data = await _mediator.Send(new PIVConfirm() { StageID = StageID, AdminUserID = AdminUserID });
            if (Data.IsSuccess) return RedirectToPage("/Admin/Login", new { ps = "true" });
            return Page();
        }

        public class PIVConfirm : IRequest<PIVConfirmResult>
        {
            public string StageID { get; set; }
            public int AdminUserID { get; set; }

        }

        public class PIVConfirmHandler : IRequestHandler<PIVConfirm, PIVConfirmResult>
        {
            private readonly ScholarshipForServiceContext _db;
            private readonly IEmailerService _emailer;
            private readonly IConfiguration _appSettings;
            public readonly IAuditEventLogHelper _auditLogger;

            public PIVConfirmHandler(ScholarshipForServiceContext db, IEmailerService emailer, IConfiguration appSettings, IAuditEventLogHelper audit)
            {
                _db = db;
                _emailer = emailer;
                _appSettings = appSettings;
                _auditLogger = audit;
            }

            public async Task<PIVConfirmResult> Handle(PIVConfirm request, CancellationToken cancellationToken)
            {

                var adminUser = _db.AdminUsers.Where(m => m.AdminUserId == request.AdminUserID)
                    .FirstOrDefault();
                if (adminUser != null)
                {

                    var certInfo = await _db.CertificateStaging.Where(m => m.CertificateStagingID == Guid.Parse(request.StageID) && m.ExpirationDate > DateTime.UtcNow).FirstOrDefaultAsync();
                    if (certInfo == null)
                        return new PIVConfirmResult() { IsSuccess = false, ErrorMessage = "An error has occured with your request. Please try again." };

                    if (certInfo.ExpirationDate < DateTime.UtcNow)
                    {
                        await _auditLogger.LogAuditEvent($"Smartcard: User {request.AdminUserID} Role AO smartcard registration link has expired.");
                        return new PIVConfirmResult() { IsSuccess = false, ErrorMessage = "Link has expired." };
                    }

                    var existingPIVAOs = await _db.AgencyUsers.Where(m => m.Certificate == certInfo.Certificate)
                        .Select(m => m.AgencyUserId)
                       .FirstOrDefaultAsync();

                    var existingPIVAdmin = await _db.AdminUsers.Where(m => m.Certificate == certInfo.Certificate)
                        .Select(m => m.AdminUserId)
                        .FirstOrDefaultAsync();

                    if (existingPIVAOs == 0 && existingPIVAdmin == 0)
                    {
                        //Add the cert info to the AgencyUser table
                        adminUser.Certificate = certInfo.Certificate;
                        adminUser.Issuer = certInfo.Issuer;
                        adminUser.Subject = certInfo.Subject;
                        adminUser.SerialNumber = certInfo.SerialNumber;
                        adminUser.SubjectAlternative = certInfo.SubjectAlternative;
                        adminUser.Thumbprint = certInfo.Thumbprint;
                        adminUser.ValidAfter = certInfo.ValidAfter;
                        adminUser.ValidUntil = certInfo.ValidUntil;
                        await _db.SaveChangesAsync();

                        //remove staging record
                        _db.CertificateStaging.Remove(certInfo);
                        await _db.SaveChangesAsync();
                        return new PIVConfirmResult() { IsSuccess = true };
                    }
                    else
                    {
                        await _auditLogger.LogAuditEvent($"Smartcard: Registration attempt for {request.AdminUserID} Role AO. Smart card registered to another user.", $"AO account: {existingPIVAOs}, AD account: {existingPIVAdmin}");
                    }
                }
                return new PIVConfirmResult() { IsSuccess = false, ErrorMessage = "An error occured while registering the smartcard." };

            }

        }

        public class PIVConfirmResult
        {
            public bool IsSuccess { get; set; }
            public string ErrorMessage { get; set; }
        }
    }
}
