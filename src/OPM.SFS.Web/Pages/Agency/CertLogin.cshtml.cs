using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OPM.SFS.Core.Shared;
using OPM.SFS.Data;
using OPM.SFS.Web.SharedCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace OPM.SFS.Web.Pages.Agency
{
    public class CertLoginModel : PageModel
    {
        private readonly IMediator _mediator;

        public CertLoginModel(IMediator mediator) => _mediator = mediator;


        public async Task<IActionResult> OnGetAsync()
        {
            //string X_ENV_SSL_ClientCertificate = "-----BEGIN CERTIFICATE----- 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 -----END CERTIFICATE-----";
            string X_ENV_SSL_ClientCertificate = Request.GetHeader("X-ENV-SSL_CLIENT_CERTIFICATE");
            var pivAuth = await _mediator.Send(new LoginCommandPIV() { CertificateString = X_ENV_SSL_ClientCertificate });
            if (pivAuth.IsSuccess) return RedirectToPage("/Agency/Dashboard");
            if (pivAuth.RegistrationRequired) return RedirectToPage("/Agency/RegisterPIV", new { c = pivAuth.RegistrationStagingID });
            if (pivAuth.IsInactive) return Redirect($"/Agency/Login?ia=true&ra={Uri.EscapeDataString(pivAuth.EncryptedUserID)}");
            ModelState.AddModelError("UserId", pivAuth.ErrorMessage);
            return RedirectToPage("/Agency/CertificateError");
        }

        public class LoginCommandPIV : IRequest<PIVLoginResult>
        {
            public string CertificateString { get; set; }
        }

        public class LoginPIVHandler : IRequestHandler<LoginCommandPIV, PIVLoginResult>
        {
            private readonly ScholarshipForServiceContext _efDB;
            private readonly ICacheHelper _cache;
            private readonly IHttpContextAccessor _httpContextAccessor;
            private readonly IAuditEventLogHelper _auditLogger;
            private readonly ICryptoHelper _crypto;

            public LoginPIVHandler(ScholarshipForServiceContext efDB, ICacheHelper cache, IHttpContextAccessor httpContextAccessor, IAuditEventLogHelper auditLogger, ICryptoHelper crypto)
            {
                _efDB = efDB;
                _cache = cache;
                _httpContextAccessor = httpContextAccessor;
                _auditLogger = auditLogger;
                _crypto = crypto;
            }

            public async Task<PIVLoginResult> Handle(LoginCommandPIV request, CancellationToken cancellationToken)
            {
                //PIV guidance - https://playbooks.idmanagement.gov/piv/identifiers/
                Regex certRegex = new Regex(@"-+BEGIN CERTIFICATE-+\w?(?<cert>[^-]+)", RegexOptions.IgnoreCase);
                Match myMatch = certRegex.Match(FormatCertificateString(request.CertificateString));
                if (myMatch.Success)
                {
                    X509Certificate2 theCert = new X509Certificate2(Convert.FromBase64String(myMatch.Groups["cert"].Value));
                    var certUser = await _efDB.AgencyUsers.Include(m => m.ProfileStatus).Where(m => m.Thumbprint == theCert.GetCertHash()).FirstOrDefaultAsync();
                    if (certUser != null && certUser.ProfileStatus.Name == "Inactive")
                    {
                        var GlobalConfigSettings = await _cache.GetGlobalConfigurationsAsync();
                        return new PIVLoginResult() { IsInactive = true, IsSuccess = false, ErrorMessage = "", EncryptedUserID = EncryptString(certUser.AgencyUserId.ToString(), GlobalConfigSettings) };
                    }

                    if (certUser != null && certUser.Subject.Equals(theCert.Subject, StringComparison.OrdinalIgnoreCase))
                    {
                       
                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.NameIdentifier, certUser.AgencyUserId.ToString()),
                            new Claim("SFS_UserID", certUser.AgencyUserId.ToString()),
                            new Claim(ClaimTypes.Name, certUser.Firstname),
                            new Claim(ClaimTypes.Role, "AO"),
                            new Claim(ClaimTypes.AuthenticationMethod, "Smartcard")
                        };
                        await _auditLogger.LogAuditEvent($"PIV: User {certUser.AgencyUserId} Role AO logged in successfully.");
                        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        await _httpContextAccessor.HttpContext.SignInAsync(new ClaimsPrincipal(claimsIdentity));
                        return new PIVLoginResult { IsSuccess = true };
                    }
                    else
                    {
                        CertificateStaging stageCert = new CertificateStaging()
                        {
                            Certificate = theCert.GetRawCertData(),
                            Issuer = theCert.Issuer,
                            Subject = theCert.Subject,
                            SerialNumber = theCert.SerialNumber,
                            SubjectAlternative = theCert.SubjectAlternativeName(),
                            Thumbprint = theCert.GetCertHash(),
                            ValidAfter = theCert.NotBefore,
                            ValidUntil = theCert.NotAfter,
                            DateInserted = DateTime.UtcNow,
                            ExpirationDate = DateTime.UtcNow.AddHours(1)
                        };
                        _efDB.CertificateStaging.Add(stageCert);
                        await _efDB.SaveChangesAsync();

                        PIVLoginResult result = new();
                        result.RegistrationRequired = true;
                        result.RegistrationStagingID = stageCert.CertificateStagingID.ToString();
                        return result;

                    }
                }                
                return new PIVLoginResult { IsSuccess = false, ErrorMessage = "Error occured" };
            }

            private string FormatCertificateString(string cert)
            {
                if (cert.Contains("-----BEGIN CERTIFICATE-----") && cert.Contains("-----END CERTIFICATE-----"))
                    return cert;
                return $"-----BEGIN CERTIFICATE----- {cert} -----END CERTIFICATE-----";

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

        public class PIVLoginResult
        {
            public bool IsSuccess { get; set; }
            public string ErrorMessage { get; set; }
            public bool RegistrationRequired { get; set; }
            public string RegistrationStagingID { get; set; }
            public bool IsInactive { get; set; }
            public string EncryptedUserID { get; set; }
        }
    }
}
