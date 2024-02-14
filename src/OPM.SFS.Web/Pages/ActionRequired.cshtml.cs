using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OPM.SFS.Web.Pages
{
    public class ActionRequiredModel : PageModel
    {
        private readonly IMediator _mediator;
        public ActionRequiredModel(IMediator mediator) => _mediator = mediator;

        public void OnGet()
        {

        }
    }
}
