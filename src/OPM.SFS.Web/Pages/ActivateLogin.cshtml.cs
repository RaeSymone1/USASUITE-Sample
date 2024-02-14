using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OPM.SFS.Data;
using OPM.SFS.Web.SharedCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace OPM.SFS.Web.Pages
{
    public class ActivateLoginModel : PageModel
    {
        [FromQuery(Name = "s")]
        public string StageID { get; set; }


        private readonly IMediator _mediator;
        public ActivateLoginModel(IMediator mediator) => _mediator = mediator;

        public async Task<IActionResult> OnGetAsync()
        {
            var auth = await _mediator.Send(new LoginCommand() { StagedIDPassword = StageID });
            if (auth.IsSuccess) return RedirectToPage(auth.RedirectUrl);
            return RedirectToPage("/AccessDenied");
        }

        public class LoginCommand : IRequest<LoginResult>
        {
            public string StagedIDPassword { get; set; }
        }

        public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResult>
        {
            private readonly ScholarshipForServiceContext _efDB;
            private readonly ICacheHelper _cache;
            private readonly IHttpContextAccessor _httpContextAccessor;
            private readonly IAuditEventLogHelper _auditLogger;
            public readonly IFeatureManager _featureManager;

            public LoginCommandHandler(ScholarshipForServiceContext efDB, ICacheHelper cache, IHttpContextAccessor httpContextAccessor, IAuditEventLogHelper auditLogger, IFeatureManager featureManager)
            {
                _efDB = efDB;
                _cache = cache;
                _httpContextAccessor = httpContextAccessor;
                _auditLogger = auditLogger;
                _featureManager = featureManager;
            }

            public async Task<LoginResult> Handle(LoginCommand request, CancellationToken cancellationToken)
            {
                //identity.AddClaim(new Claim("EVFToggle", linkedAccount.FirstOrDefault().EVFToggle.ToString()));
                var password = Guid.Parse(request.StagedIDPassword);
                var IsEnabledOnSite = await _featureManager.IsEnabledSiteWideAsync("EmploymentVerfication");
                var stagedInfo = await _efDB.LoginGovStaging.Where(m => m.LoginGovStagingID == password).FirstOrDefaultAsync();
                if(stagedInfo != null && stagedInfo.ExpirationDate > DateTime.UtcNow)
                {
                    if(stagedInfo.AccountType == "ST")
                    {
                        var accountInfo = await _efDB.Students.Where(m => m.StudentId == stagedInfo.AccountID).FirstOrDefaultAsync();
                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.NameIdentifier, stagedInfo.LoginGovLinkID),
                            new Claim("SFS_UserID", stagedInfo.AccountID.ToString()),
                            new Claim(ClaimTypes.Name, accountInfo.FirstName),
                            new Claim(ClaimTypes.Role, stagedInfo.AccountType),
                            new Claim(ClaimTypes.AuthenticationMethod, "Cookies"),
                            new Claim("EVFToggle", IsEnabledOnSite.ToString())
                    };

                        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        await _httpContextAccessor.HttpContext.SignInAsync(new ClaimsPrincipal(claimsIdentity));
                        _efDB.LoginGovStaging.Remove(stagedInfo);
                        accountInfo.LoginGovLinkID = stagedInfo.LoginGovLinkID;
                        await _efDB.SaveChangesAsync();
                        await _auditLogger.LogAuditEvent($"Login.gov: User {accountInfo.StudentId} Role ST, successfully linked.");
                        return new LoginResult { IsSuccess = true, ErrorMessage = "", RedirectUrl = "/Student/Dashboard" };
                    }
                    if(stagedInfo.AccountType == "AO")
                    {
                        var accountInfo = await _efDB.AgencyUsers.Where(m => m.AgencyUserId == stagedInfo.AccountID).FirstOrDefaultAsync();
                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.NameIdentifier, stagedInfo.LoginGovLinkID),
                            new Claim("SFS_UserID", stagedInfo.AccountID.ToString()),
                            new Claim(ClaimTypes.Name, accountInfo.Firstname),
                            new Claim(ClaimTypes.Role, stagedInfo.AccountType)
                        };

                        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        await _httpContextAccessor.HttpContext.SignInAsync(new ClaimsPrincipal(claimsIdentity));
                        _efDB.LoginGovStaging.Remove(stagedInfo);
                        accountInfo.LoginGovLinkID = stagedInfo.LoginGovLinkID;
                        await _efDB.SaveChangesAsync();
                        await _auditLogger.LogAuditEvent($"Login.gov: User {accountInfo.AgencyUserId} Role AO, successfully linked.");
                        return new LoginResult { IsSuccess = true, ErrorMessage = "", RedirectUrl = "/Agency/Dashboard" };
                    }
                    if (stagedInfo.AccountType == "AD")
                    {
                        var accountInfo = await _efDB.AdminUsers.Where(m => m.AdminUserId == stagedInfo.AccountID).FirstOrDefaultAsync();
                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.NameIdentifier, stagedInfo.LoginGovLinkID),
                            new Claim("SFS_UserID", stagedInfo.AccountID.ToString()),
                            new Claim(ClaimTypes.Name, accountInfo.FirstName),
                            new Claim(ClaimTypes.Role, stagedInfo.AccountType)
                        };

                        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        await _httpContextAccessor.HttpContext.SignInAsync(new ClaimsPrincipal(claimsIdentity));
                        _efDB.LoginGovStaging.Remove(stagedInfo);
                        accountInfo.LoginGovLinkID = stagedInfo.LoginGovLinkID;
                        await _efDB.SaveChangesAsync();
                        await _auditLogger.LogAuditEvent($"Login.gov: User {accountInfo.AdminUserId} Role AD, successfully linked.");
                        return new LoginResult { IsSuccess = true, ErrorMessage = "", RedirectUrl = "/Admin/Home" };
                    }
                    if (stagedInfo.AccountType == "PI")
                    {
                        var accountInfo = await _efDB.AcademiaUsers.Where(m => m.AcademiaUserId == stagedInfo.AccountID).FirstOrDefaultAsync();
                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.NameIdentifier, stagedInfo.LoginGovLinkID),
                            new Claim("SFS_UserID", stagedInfo.AccountID.ToString()),
                            new Claim(ClaimTypes.Name, accountInfo.Firstname),
                            new Claim(ClaimTypes.Role, stagedInfo.AccountType)
                        };

                        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        await _httpContextAccessor.HttpContext.SignInAsync(new ClaimsPrincipal(claimsIdentity));
                        _efDB.LoginGovStaging.Remove(stagedInfo);
                        accountInfo.LoginGovLinkID = stagedInfo.LoginGovLinkID;
                        await _efDB.SaveChangesAsync();
                        await _auditLogger.LogAuditEvent($"Login.gov: User {accountInfo.AcademiaUserId} Role PI, successfully linked.");
                        return new LoginResult { IsSuccess = true, ErrorMessage = "", RedirectUrl = "/Academia/Dashboard" };
                    }
                    await _auditLogger.LogAuditEvent($"Login.gov: User {stagedInfo.AccountID} Role {stagedInfo.AccountType}, linking failed.");
                    return new LoginResult() { IsSuccess = false, ErrorMessage = "Link has expired or invalid." };
                }
                await _auditLogger.LogAuditEvent($"Login.gov: Invalid linking attepmt. The unique ID is not found.");
                return new LoginResult() { IsSuccess = false, ErrorMessage = "Link has expired or invalid." };
            }
        }

        public class LoginResult
        {
            public bool IsSuccess { get; set; }
            public string ErrorMessage { get; set; }
            public string RedirectUrl { get; set; } ///Agency/Dashboard
        }
    }
}
