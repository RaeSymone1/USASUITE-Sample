using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OPM.SFS.Core.Shared;
using OPM.SFS.Data;
using OPM.SFS.Web.Shared;

namespace OPM.SFS.Web.SharedCode
{
    public class SignInHandler : CookieAuthenticationEvents
    {
        private readonly ScholarshipForServiceContext _efDB;
        public readonly IAuditEventLogHelper _auditLogger;
        private readonly IEmailerService _emailer;
        public readonly IFeatureManager _featureManager;

        public SignInHandler(ScholarshipForServiceContext db, IAuditEventLogHelper audit, IEmailerService emailer, IFeatureManager featureManager)
        {
            _efDB = db;
            _auditLogger = audit;
            _emailer = emailer;
            _featureManager = featureManager;
        }

        public override Task SigningIn(CookieSigningInContext ctx)
        {
            var redirect = OnSignin(
                ctx.Principal,
                new UserAgentInfo(ctx.Request.Headers["User-Agent"], ctx.Request.HttpContext.Connection.RemoteIpAddress.ToString()),
                out var principal
            );
            ctx.Principal = principal;


            return Task.CompletedTask;
        }

        public string OnSignin(ClaimsPrincipal incomingPrincipal, UserAgentInfo info, out ClaimsPrincipal outPrincipal)
        {

            outPrincipal = incomingPrincipal;
            var incomingIdentity = (ClaimsIdentity)incomingPrincipal?.Identity;
            if (incomingIdentity != null && incomingIdentity.IsAuthenticated)
            {

                //Check if user is already authenticated via the one-time linking link
                var sfsID = incomingPrincipal.Claims.Where(m => m.Type == "SFS_UserID").FirstOrDefault();
                if (sfsID is not null && !string.IsNullOrWhiteSpace(sfsID.Value))
                    return "";


                //get the UID from the token                
                string loginGovLinkID = incomingPrincipal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
                var allEmails = incomingPrincipal.Claims.Where(c => c.Type == "all_emails").ToList();
                string emailList = "";
                if (allEmails != null && allEmails.Count > 0)
                {
                    emailList = string.Join(",", allEmails.Select(m => m.Value));
                }
                var emailUsedForLogin = incomingPrincipal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email).Value;

                //1st look up the user by UID to check if Login.gov is already linked. Add the roles based the user type
                var linkedAccount = GetAccountsByLoginGovIDAsync(loginGovLinkID);

                if (linkedAccount != null && linkedAccount.Count == 1)
                {
                    SyncEmailFromLoginGov(loginGovLinkID, linkedAccount.First(), emailUsedForLogin);
                    var identity = AddClaims(incomingIdentity, info, linkedAccount);
                    outPrincipal = new ClaimsPrincipal(identity);

                }
                else if (!string.IsNullOrWhiteSpace(emailList))
                {

                    var identity = AddClaimsLinkingRequired(incomingIdentity, info);
                    outPrincipal = new ClaimsPrincipal(identity);
                }
            }

            return "";
        }

        private List<Account> GetAccountsByLoginGovIDAsync(string id)
        {
            var IsEnabledOnSite =  _featureManager.IsEnabledSiteWideAsync("EmploymentVerfication");
            List<Account> accounts = new();
            var studentLookup = _efDB.Students.Where(m => m.LoginGovLinkID == id).Select(m => new Account
            {
                AccountID = m.StudentId,
                FirstName = m.FirstName,
                Role = "ST",
                AdminRole = "",
                IsInactive = m.ProfileStatus.Name == "Inactive",
                EVFToggle = IsEnabledOnSite.Result.ToString()
            }).FirstOrDefault();
            var adminLookup = _efDB.AdminUsers.Where(m => m.LoginGovLinkID == id).Select(m => new Account
            {
                AccountID = m.AdminUserId,
                FirstName = m.FirstName,
                Role = "AD",
				AdminRole = m.AdminUserRole.Role,
				IsInactive = m.IsDisabled,
                EVFToggle = IsEnabledOnSite.Result.ToString()
            }).FirstOrDefault();
            var agencyLookup = _efDB.AgencyUsers.Where(m => m.LoginGovLinkID == id).Select(m => new Account
            {
                AccountID = m.AgencyUserId,
                FirstName = m.Firstname,
                Role = "AO",
				AdminRole = "",
				IsInactive = m.ProfileStatus.Name == "Inactive",
                EVFToggle = IsEnabledOnSite.Result.ToString()
            }).FirstOrDefault();
            var academiaUser = _efDB.AcademiaUsers.Where(m => m.LoginGovLinkID == id).Select(m => new Account
            {
                AccountID = m.AcademiaUserId,
                FirstName = m.Firstname + ' ' + m.Lastname ,
                Role = "PI",
				AdminRole = "",
				IsInactive = m.ProfileStatus.Name == "Inactive",
                EVFToggle = IsEnabledOnSite.Result.ToString()
            }).FirstOrDefault();

            if (studentLookup != null) accounts.Add(studentLookup);
            if (adminLookup != null) accounts.Add(adminLookup);
            if (agencyLookup != null) accounts.Add(agencyLookup);
            if (academiaUser != null) accounts.Add(academiaUser);
            return accounts;
        }

        private void SyncEmailFromLoginGov(string loginGovLinkedID, Account linkedAccount, string mainEmailFromLoginGov)
        {
            if (linkedAccount.Role == "ST")
            {
                var studentToUpdate = _efDB.Students.Where(m => m.StudentId == linkedAccount.AccountID).FirstOrDefault();
                studentToUpdate.Email = mainEmailFromLoginGov;
                _efDB.SaveChanges();
            }
            if (linkedAccount.Role == "AO")
            {
                var aoToUpdate = _efDB.AgencyUsers.Where(m => m.AgencyUserId == linkedAccount.AccountID).FirstOrDefault();
                aoToUpdate.Email = mainEmailFromLoginGov;
                aoToUpdate.LastLoginDate = DateTime.UtcNow;
                _efDB.SaveChanges();
            }
            if (linkedAccount.Role == "PI")
            {
                var piToUpdate = _efDB.AcademiaUsers.Where(m => m.AcademiaUserId == linkedAccount.AccountID).FirstOrDefault();
                piToUpdate.Email = mainEmailFromLoginGov;
                piToUpdate.LastLoginDate = DateTime.UtcNow;
                _efDB.SaveChanges();
            }
            if (linkedAccount.Role == "AD")
            {
                var adminToUpdate = _efDB.AdminUsers.Where(m => m.AdminUserId == linkedAccount.AccountID).FirstOrDefault();
                adminToUpdate.Email = mainEmailFromLoginGov;
                adminToUpdate.LastLoginDate = DateTime.UtcNow;
                _efDB.SaveChanges();
            }
        }

        private ClaimsIdentity AddClaims(ClaimsIdentity incomingIdentity, UserAgentInfo info, List<Account> linkedAccount)
        {
            var identity = incomingIdentity.Clone();
            identity.AddClaim(new Claim("SFS_UserID", linkedAccount.FirstOrDefault().AccountID.ToString()));
            identity.AddClaim(new Claim(ClaimTypes.Role, linkedAccount.FirstOrDefault().Role));
			identity.AddClaim(new Claim("AdminRole", linkedAccount.FirstOrDefault().AdminRole));
			identity.AddClaim(new Claim(ClaimTypes.Name, linkedAccount.FirstOrDefault().FirstName));
            identity.AddClaim(new Claim(ClaimTypes.AuthenticationMethod, "logingov"));
            identity.AddClaim(new Claim("EVFToggle", linkedAccount.FirstOrDefault().EVFToggle.ToString()));
            if (linkedAccount.FirstOrDefault().IsInactive)
                identity.AddClaim(new Claim("IsInactive", "true"));
            else
                identity.AddClaim(new Claim("IsInactive", "false"));
            return identity;
        }

        private ClaimsIdentity AddClaimsLinkingRequired(ClaimsIdentity incomingIdentity, UserAgentInfo info)
        {
            var identity = incomingIdentity.Clone();
            identity.AddClaim(new Claim("SFS_UserID", "0")); //UserID = 0 need to iniate the linking process
            identity.AddClaim(new Claim(ClaimTypes.Role, "PD")); //Role = PD for pending login.gov linking
            identity.AddClaim(new Claim("IsInactive", "false"));
            return identity;
        }

    }

    public class UserAgentInfo
    {
        public UserAgentInfo(string userAgent = "", string userIP = "")
        {
            UserAgent = userAgent;
            UserIP = userIP;
        }
        public string UserAgent { get; private set; }
        public string UserIP { get; private set; }
    }

    public class Account
    {
        public int? AccountID { get; set; }
        public string FirstName { get; set; }
        public string AdminRole { get; set; }
        public string Role { get; set; }
        public bool IsInactive { get; set; }
        public string EVFToggle { get; internal set; }
    }
}
