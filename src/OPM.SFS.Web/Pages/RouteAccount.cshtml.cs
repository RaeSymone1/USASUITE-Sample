using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OPM.SFS.Data;
using OPM.SFS.Core.Shared;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Collections.Generic;
using OPM.SFS.Web.SharedCode;

namespace OPM.SFS.Web.Pages
{
    [Authorize(Roles = "ST,PI,AO,AD,PD")]
    public class RouteAccount : PageModel
    {
        private readonly ILoginGovLinkingHelper _helper;
        public readonly IAuditEventLogHelper _auditLogger;
        private readonly ScholarshipForServiceContext _db;
        private readonly ICryptoHelper _crypto;
        private readonly ICacheHelper _cache;

        public RouteAccount(ILoginGovLinkingHelper helper, IAuditEventLogHelper audit, ScholarshipForServiceContext db, ICryptoHelper crypto, ICacheHelper cache)
        {
            _helper = helper;
            _auditLogger = audit;
            _db = db;
            _cache = cache;
            _crypto = crypto;
        }
        public async Task<IActionResult> OnGetAsync()
        {
            var role = User.FindFirst(ClaimTypes.Role).Value;
            var id = User.FindFirst("SFS_UserID").Value;
            var isInactive = Convert.ToBoolean(User.FindFirst("IsInactive").Value);
            var loginGovID = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var allEmails = User.Claims.Where(c => c.Type == "all_emails").ToList();
            var GlobalConfigSettings = await _cache.GetGlobalConfigurationsAsync();

            if (id == "0")
            {
                await _helper.StartLinkingProcessAsync(allEmails, loginGovID);
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme);
                return Redirect("ActionRequired");
            }

            if (isInactive)
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme);
                string toEncrypt = id.Trim();
                var encryptedID = EncryptString(id, GlobalConfigSettings);
                if(role == "ST") return Redirect($"/Student/Login?ia=true&ra={Uri.EscapeDataString(encryptedID)}");
                if (role == "AO") return Redirect($"/Agency/Login?ia=true&ra={Uri.EscapeDataString(encryptedID)}");
                if (role == "PI") return Redirect($"/Academia/Login?ia=true&ra={Uri.EscapeDataString(encryptedID)}");
                if (role == "AD") return Redirect($"/Admin/Login?ia=true&ra={Uri.EscapeDataString(encryptedID)}");
            }


            if (role == "ST" && id != "0")
            {                
                var student = await _db.Students.Where(m => m.StudentId == Convert.ToInt32(id)).FirstOrDefaultAsync();
                student.LastLoginDate = DateTime.UtcNow;
                await _db.SaveChangesAsync();
                await _auditLogger.LogAuditEvent($"Login.gov: User {id} Role ST logged in successfully.");
                return Redirect("/Student/Dashboard");
            }
                
            if (role == "AO" && id != "0")
            {
                var aoUser = await _db.AgencyUsers.Where(m => m.AgencyUserId == Convert.ToInt32(id)).FirstOrDefaultAsync();
                aoUser.LastLoginDate = DateTime.UtcNow;
                await _db.SaveChangesAsync();
                await _auditLogger.LogAuditEvent($"Login.gov: User {id} Role AO logged in successfully.");
                return Redirect("/Agency/Dashboard");
            }
              
            if (role == "PI" && id != "0")
            {
                
                var aoUser = await _db.AcademiaUsers.Where(m => m.AcademiaUserId == Convert.ToInt32(id)).FirstOrDefaultAsync();
                aoUser.LastLoginDate = DateTime.UtcNow;
                await _db.SaveChangesAsync();
                await _auditLogger.LogAuditEvent($"Login.gov: User {id} Role PI logged in successfully.");
                return Redirect("/Academia/Dashboard");
            }
                
            if (role == "AD" && id != "0")
            {
                
                var admin = await _db.AdminUsers.Where(m => m.AdminUserId == Convert.ToInt32(id)).FirstOrDefaultAsync();
                if(admin.EnforcePIV)
                {
					await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
					await HttpContext.SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme);
					return RedirectToPage("/Admin/Login", new { ep = "true" });
				}
                admin.LastLoginDate = DateTime.UtcNow;
                await _db.SaveChangesAsync();
                await _auditLogger.LogAuditEvent($"Login.gov: User {id} Role AD logged in successfully.");
                return Redirect("/Admin/Home");
            }                    

            return Redirect("AccessDenied");
        }

        private string EncryptString(string input, List<GlobalConfiguration> GlobalConfigSettings)
        {
            
            EncryptionKeys _keys = new EncryptionKeys();
            _keys.Salt = GlobalConfigSettings.Where(m => m.Key == "Salt" && m.Type == "Encryption").Select(m => m.Value).FirstOrDefault();
            _keys.PassPhrase = GlobalConfigSettings.Where(m => m.Key == "Passphrase" && m.Type == "Encryption").Select(m => m.Value).FirstOrDefault();
            _keys.KeySize = Convert.ToInt32(GlobalConfigSettings.Where(m => m.Key == "Keysize" && m.Type == "Encryption").Select(m => m.Value).FirstOrDefault());
            _keys.InitVector = GlobalConfigSettings.Where(m => m.Key == "InitVect" && m.Type == "Encryption").Select(m => m.Value).FirstOrDefault();
            _keys.PasswordIterations = Convert.ToInt32(GlobalConfigSettings.Where(m => m.Key == "PasswordIterations" && m.Type == "Encryption").Select(m => m.Value).FirstOrDefault());
            return _crypto.Encrypt(input, _keys);
        }
    }
}
