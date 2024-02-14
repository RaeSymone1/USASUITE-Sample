using Microsoft.AspNetCore.Http;
using System.Linq;

namespace OPM.SFS.Web.SharedCode
{
    public static class HttpRequestExtension
    {
        public static string GetHeader(this HttpRequest request, string key)
        {
            return request.Headers.FirstOrDefault(x => x.Key == key).Value.FirstOrDefault();
        }
    }
}
