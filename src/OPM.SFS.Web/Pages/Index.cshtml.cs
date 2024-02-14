using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPM.SFS.Web.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            _logger.LogInformation("Index page loaded");

        }

        public ActionResult OnGetExtendSession()
        {
            return Content("Session extended.");
        }

        public async Task<ActionResult> OnGetSignOutAsync()
        {
            await HttpContext.SignOutAsync();
            return Content("OK");
        }
        
    }
}
