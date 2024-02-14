using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OPM.SFS.Web.SharedCode;

namespace OPM.SFS.Web.Pages.Student
{
    
    public class FAQsModel : PageModel
    {
        public void OnGet()
        {
            //TODO: Model bind FAQs to db table
            //Cache the db call
        }
    }
}
