using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OPM.SFS.Web.Infrastructure.Extensions
{
    public static class ClaimsExtension
    {
        public static int GetUserId(this ClaimsPrincipal principal) => principal == null ? default : Convert.ToInt32(principal.FindFirstValue("SFS_UserID"));
        public static string GetLoginGovID(this ClaimsPrincipal principal) => principal == null ? default : principal.FindFirstValue(ClaimTypes.NameIdentifier);
        public static string GetFirstName(this ClaimsPrincipal principal) => principal == null ? default : principal.FindFirstValue(ClaimTypes.Name);
        public static string GetUserRole(this ClaimsPrincipal principal) => principal == null ? default : principal.FindFirstValue(ClaimTypes.Role);

        //public static string GetUserId(this ClaimsPrincipal principal)
        //{
        //    return principal.FindFirstValue(ClaimTypes.NameIdentifier);
        //}

        //public static string GetLoginGovID(this ClaimsPrincipal principal)
        //{
        //    return principal.FindFirstValue(ClaimTypes.NameIdentifier);
        //}

        //public static string GetUserFirstName(this ClaimsPrincipal principal)
        //{
        //    return principal.FindFirstValue(ClaimTypes.Name);
        //}

        //public static string GetUserRole(this ClaimsPrincipal principal)
        //{
        //    return principal.FindFirstValue(ClaimTypes.Role);
        //}
    }
}
