using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Linq;
using System.Threading.Tasks;

namespace OPM.SFS.Web.Pages
{
    public class TransitionModel : PageModel
    {
        public void OnGet()
        {
        }

        public IActionResult OnGetContinueLoginGov()
        {
            return new ChallengeResult(OpenIdConnectDefaults.AuthenticationScheme, new AuthenticationProperties()
            {
                RedirectUri = "/RouteAccount"
            });
        }
    }
}
