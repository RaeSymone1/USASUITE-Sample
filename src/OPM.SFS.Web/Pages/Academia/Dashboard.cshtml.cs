using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OPM.SFS.Web.Pages.Academia
{
    [Authorize(Roles = "PI")]
    public class DashboardModel : PageModel
    {
        public void OnGet()
        {

        }
    }
}
