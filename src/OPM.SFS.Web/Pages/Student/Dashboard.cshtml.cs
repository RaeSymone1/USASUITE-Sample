using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OPM.SFS.Data;
using OPM.SFS.Web.SharedCode;

namespace OPM.SFS.Web.Pages.Student
{
    [Authorize(Roles="ST")]
    [IsProfileCompleteFilter()]
    public class DashboardModel : PageModel
    {
        private readonly IMediator _mediator;
       
        public DashboardModel(IMediator mediator) => _mediator = mediator;

        public async Task<IActionResult> OnGetAsync()
        {
            string redirectUser = await _mediator.Send(new Query() { StudentID = Convert.ToInt32(User.FindFirst("SFS_UserID").Value) });
            if(!string.IsNullOrWhiteSpace(redirectUser)) 
                return RedirectToPage(redirectUser, new { i = "true" }); ;
            return Page();
        }
              

        public class Query : IRequest<string>
        {
            public int StudentID { get; set; }
        }

        public class QueryHandler : IRequestHandler<Query, string>
        {
            private readonly ScholarshipForServiceContext _db;

            public QueryHandler(ScholarshipForServiceContext db)
            {
                _db = db;
            }
            public async Task<string> Handle(Query request, CancellationToken cancellationToken)
            {
                var backgroundInfo = await _db.Students.Where(m => m.StudentId == request.StudentID)
                    .Select(m => m.EthnicityId).FirstOrDefaultAsync();
                if (!backgroundInfo.HasValue || backgroundInfo.Value == 0)
                    return "BackgroundInfo"; //Redirect to the Background Info page

                var profileInfo = await _db.Students.Where(m => m.StudentId == request.StudentID)
                   .Select(m => m.PermanentAddressId).FirstOrDefaultAsync();

                if (!profileInfo.HasValue || profileInfo.Value == 0)
                    return "Profile"; //Rediret to the Profile page

                return "";
            }
        }
    }

    
}
