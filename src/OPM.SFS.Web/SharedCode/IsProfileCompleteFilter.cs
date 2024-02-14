using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OPM.SFS.Data;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OPM.SFS.Web.SharedCode
{

    public class IsProfileCompleteFilter : ResultFilterAttribute
    {

        public override async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {

            //need to use Service Locator pattern here instead of default DI 
            var validator = context.HttpContext.RequestServices.GetService<IStudentProfileValidator>();
            ClaimsIdentity claimsIdentity = context.HttpContext.User.Identity as ClaimsIdentity;

            int id = int.Parse(claimsIdentity.FindFirst("SFS_UserID").Value);
            bool isBackgroundComplete = validator.IsBackgroundComplete(id);

            bool isProfileComplete = validator.IsProfileComplete(id);
            if (!isProfileComplete)
            {
                context.Result = new RedirectResult("/Student/Profile?i=true");
                await next.Invoke();
            }

            if (!isBackgroundComplete)
            {
                context.Result = new RedirectResult("/Student/BackgroundInfo?i=true");
                await next.Invoke();
            }

            await next.Invoke();
        }
    }
}
