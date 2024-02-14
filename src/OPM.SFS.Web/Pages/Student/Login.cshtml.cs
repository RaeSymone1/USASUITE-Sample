using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using System;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using OPM.SFS.Web.Handlers;

namespace OPM.SFS.Web.Pages.Student
{
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public class LoginModel : PageModel
    {

        [FromQuery(Name = "ra")]
        public string EncryptedStudentID { get; set; }

        [FromQuery(Name = "ia")]
        public string IsAccountInactive { get; set; } 
        
        [BindProperty]
        public LoginViewModel Data { get; set; }

        private readonly IMediator _mediator;        

        public LoginModel(IMediator mediator)
        {         
          _mediator = mediator;
        }

        public void OnGet()
        {
            if (!string.IsNullOrWhiteSpace(EncryptedStudentID))
            {
                string uri = $"?handler=SendReactiveEmail&ra={Uri.EscapeDataString(EncryptedStudentID)}";
                Data = new LoginViewModel() { IsAccountInactive = Convert.ToBoolean(IsAccountInactive), EncryptedStudentID = EncryptedStudentID, ReactivateUrl = uri };
            }
            else 
                Data = new LoginViewModel() { IsAccountInactive = Convert.ToBoolean(IsAccountInactive), EncryptedStudentID = EncryptedStudentID, ReactivateUrl = "" };
        }
    
        public async Task<ActionResult> OnGetSendReactiveEmailAsync(string ra)
        {
            var result = await _mediator.Send(new ReactivateAccountRequest() { EncryptedID = ra, AccountType = "ST" });
            Data = new LoginViewModel() {  ShowSuccessEmail = true };
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

    public class LoginViewModel
    {
        public bool IsAccountInactive { get; set; }
        public string EncryptedStudentID { get; set; }
        public bool ShowSuccessEmail { get; set; }
        public string ReactivateUrl { get; set; }
    }
}
