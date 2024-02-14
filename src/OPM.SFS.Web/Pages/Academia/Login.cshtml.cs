using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OPM.SFS.Web.Handlers;
using OPM.SFS.Web.Models;

namespace OPM.SFS.Web.Pages.Academia
{

    [AllowAnonymous]
    public class LoginModel : PageModel
    {


        [FromQuery(Name = "ra")]
        public string EncryptedStudentID { get; set; }

        [FromQuery(Name = "ia")]
        public string IsAccountInactive { get; set; }

        [BindProperty]
        public LoginUserViewModel Data { get; set; }

        private readonly IMediator _mediator;

        public LoginModel(IMediator mediator) => _mediator = mediator;

        public void OnGet()
        {
            if (!string.IsNullOrWhiteSpace(EncryptedStudentID))
            {
                string uri = $"?handler=SendReactiveEmail&ra={Uri.EscapeDataString(EncryptedStudentID)}";
                Data = new LoginUserViewModel() { IsAccountInactive = Convert.ToBoolean(IsAccountInactive), EncryptedStudentID = EncryptedStudentID, ReactivateUrl = uri };
            }
            else
                Data = new LoginUserViewModel() { IsAccountInactive = Convert.ToBoolean(IsAccountInactive), EncryptedStudentID = EncryptedStudentID, ReactivateUrl = "" };
        }

        public async Task<ActionResult> OnGetSendReactiveEmailAsync(string ra)
        {
            var result = await _mediator.Send(new ReactivateAccountRequest() { EncryptedID = ra, AccountType = "PI" });
            Data = new LoginUserViewModel() { ShowSuccessEmail = true };
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
