using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
using Microsoft.Extensions.Configuration;
using OPM.SFS.Core.Shared;
using OPM.SFS.Data;
using OPM.SFS.Web.SharedCode;
using OPM.SFS.Web.Handlers;
using OPM.SFS.Web.Models;
using OPM.SFS.Web.Shared;

namespace OPM.SFS.Web.Pages.Agency
{
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public class LoginModel : PageModel
    {

        [BindProperty]
        public AgencyOfficialLoginViewModel Data { get; set; }

        [FromQuery(Name = "ps")]
        public string ShowPIVRegisterMessage { get; set; }

        [FromQuery(Name = "ra")]
        public string EncryptedStudentID { get; set; }

        [FromQuery(Name = "ia")]
        public string IsAccountInactive { get; set; }


        private readonly IMediator _mediator;

        public LoginModel(IMediator mediator) => _mediator = mediator;

        public void OnGet()
        {
            if (!string.IsNullOrWhiteSpace(EncryptedStudentID))
            {
                string uri = $"?handler=SendReactiveEmail&ra={Uri.EscapeDataString(EncryptedStudentID)}";
                Data = new AgencyOfficialLoginViewModel()
                {
                    ShowPIVSuccessMessage = Convert.ToBoolean(ShowPIVRegisterMessage),
                    IsAccountInactive = Convert.ToBoolean(IsAccountInactive),
                    EncryptedStudentID = EncryptedStudentID,
                    ReactivateUrl = uri
                };
            }
            else
            {
                Data = new AgencyOfficialLoginViewModel()
                {
                    ShowPIVSuccessMessage = Convert.ToBoolean(ShowPIVRegisterMessage),
                    IsAccountInactive = Convert.ToBoolean(IsAccountInactive),
                    EncryptedStudentID = EncryptedStudentID,
                    ReactivateUrl = ""
                };
            }

        }

        public async Task<ActionResult> OnGetSendReactiveEmailAsync(string ra)
        {
            var result = await _mediator.Send(new ReactivateAccountRequest() { EncryptedID = ra, AccountType = "AO" });
            Data = new AgencyOfficialLoginViewModel() { ShowSuccessEmail = true };
            return Page();
        }

        public IActionResult OnGetLoginGov()
        {
            return new ChallengeResult(OpenIdConnectDefaults.AuthenticationScheme, new AuthenticationProperties()
            {
                RedirectUri = "/RouteAccount"
            });
        }

        public IActionResult OnGetTransition()
        {
            return Redirect("/Transition");
        }      

        
    }

 
}
