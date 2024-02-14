using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace OPM.SFS.Web.Pages
{
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    [IgnoreAntiforgeryToken]
    public class ErrorModel : PageModel
    {
        public string RequestId { get; set; }
        public string ErrorStatusCode { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

        private readonly ILogger<ErrorModel> _logger;

        public ErrorModel(ILogger<ErrorModel> logger)
        {
            _logger = logger;
        }

        public void OnGet(string code)
        {            
            var exceptionThrown =
            HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            if (exceptionThrown != null)
            {
                _logger.LogError(exceptionThrown.Error, exceptionThrown.Error.Message);
            }
            if(code == "404")
            {
                var requestURL = HttpContext.Features.Get<IHttpRequestFeature>();
                _logger.LogWarning($"404: Page not found for {requestURL.RawTarget}");
            }

        }
    }
}
