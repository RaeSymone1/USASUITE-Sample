using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using OPM.SFS.Data;
using OPM.SFS.Web.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OPM.SFS.Web.SharedCode
{
    public class LoginGovLog { }
    public class LoginGovAuthentication
    {
        public void Configure(OpenIdConnectOptions options, IConfiguration configuration)
        {

            options.UsePkce = true;
            options.ResponseType = OpenIdConnectResponseType.Code;
            options.Authority = configuration["LoginGov:Authority"];
            options.ClientId = configuration["LoginGov:ClientId"];
            options.MetadataAddress = configuration["LoginGov:MetaData"];
            options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            options.SignedOutRedirectUri = "/Index";
            options.SkipUnrecognizedRequests = true;
            options.SignedOutCallbackPath = "/signout-callback-oidc";
            options.RemoteSignOutPath = "/signout-oidc";
            options.SaveTokens = true;
            options.Scope.Clear();
            options.Scope.Add("openid");
            options.Scope.Add("email");
            options.Scope.Add("x509");
            options.Scope.Add("all_emails");
            options.Events = new OpenIdConnectEvents()
            {
                OnRedirectToIdentityProvider = m =>
                {
                    m.ProtocolMessage.AcrValues = "http://idmanagement.gov/ns/assurance/loa/1";
                    return Task.CompletedTask;
                },
                OnRedirectToIdentityProviderForSignOut = m =>
                {
                    m.ProtocolMessage.ClientId = configuration["LoginGov:ClientId"];
                    m.ProtocolMessage.IdTokenHint = null;
                    return Task.CompletedTask;
                },
                OnTokenValidated = ctx =>
                {

                    var identity = (ClaimsIdentity)ctx.Principal.Identity;
                    identity.AddClaim(new Claim("auth_time", DateTime.UtcNow.ToString()));
                    return Task.CompletedTask;

                },
                OnAccessDenied = m =>
                {
                    //add logging to log denied status
                    var _auditLogger = m.HttpContext.RequestServices.GetRequiredService<IAuditEventLogHelper>();
                    _ = _auditLogger.LogAuditEvent("Login.gov: Access denied").Result;
                    return Task.CompletedTask;
                },
                OnAuthenticationFailed = m =>
                {
                    //add logging to log auth failures
                    var _auditLogger = m.HttpContext.RequestServices.GetRequiredService<IAuditEventLogHelper>();
                    _ = _auditLogger.LogAuditEvent("Login.gov: Authentication failed").Result;
                    return Task.CompletedTask;
                }
            };
        }
    }
}
