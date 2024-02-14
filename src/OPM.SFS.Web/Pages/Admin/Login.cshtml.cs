using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OPM.SFS.Core.Shared;
using OPM.SFS.Data;
using OPM.SFS.Web.SharedCode;
using OPM.SFS.Web.Handlers;
using OPM.SFS.Web.Models;

namespace OPM.SFS.Web.Pages.Admin
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {

        [BindProperty]
        public AdminLoginViewModel Data { get; set; }

        [FromQuery(Name = "ra")]
        public string EncryptedStudentID { get; set; }

        [FromQuery(Name = "ia")]
        public string IsAccountInactive { get; set; }

        [FromQuery(Name = "ps")]
        public string ShowPIVRegisterMessage { get; set; }

		[FromQuery(Name = "ep")]
		public string ShowEnforcePIVMessage { get; set; }

		private readonly IMediator _mediator;

        public LoginModel(IMediator mediator) =>  _mediator = mediator;
        

        public void OnGet()
        {           
            if (!string.IsNullOrWhiteSpace(EncryptedStudentID))
            {
                string uri = $"?handler=SendReactiveEmail&ra={Uri.EscapeDataString(EncryptedStudentID)}";
                Data = new AdminLoginViewModel() { IsAccountInactive = Convert.ToBoolean(IsAccountInactive), EncryptedStudentID = EncryptedStudentID, ReactivateUrl = uri };
            }
            else
                Data = new AdminLoginViewModel() { IsAccountInactive = Convert.ToBoolean(IsAccountInactive), EncryptedStudentID = EncryptedStudentID, ReactivateUrl = "", ShowPIVSuccessMessage = Convert.ToBoolean(ShowPIVRegisterMessage), ShowEnforcePIVMessage = Convert.ToBoolean(ShowEnforcePIVMessage) };
        }

        public async Task<ActionResult> OnGetSendReactiveEmailAsync(string ra)
        {
            var result = await _mediator.Send(new ReactivateAccountRequest() { EncryptedID = ra, AccountType = "AD" });
            Data = new AdminLoginViewModel() { ShowSuccessEmail = true };
            return Page();
        }

        public IActionResult OnGetTransition()
        {
            return Redirect("/Transition");
        }

        public IActionResult OnGetLoginGov()
        {
            return new ChallengeResult(OpenIdConnectDefaults.AuthenticationScheme, new AuthenticationProperties()
            {
                RedirectUri = "/RouteAccount"
            });
        }

    }
}
