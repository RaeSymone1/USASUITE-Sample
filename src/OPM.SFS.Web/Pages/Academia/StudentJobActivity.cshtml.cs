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
using OPM.SFS.Web.Models;

namespace OPM.SFS.Web.Pages.Academia
{
    [Authorize(Roles = "PI")]

    public class StudentJobActivityModel : PageModel
    {
        [BindProperty]
        public StudentJobActivityViewModel Data { get; set; }

        [FromQuery(Name = "sid")]
        public int StudentID { get; set; } = 0;
        private readonly IMediator _mediator;
        public StudentJobActivityModel(IMediator mediator) => _mediator = mediator;
        public async Task OnGetAsync()
        {
            Data = await _mediator.Send(new Query() { StudentID = StudentID });
        }
        public class Query : IRequest<StudentJobActivityViewModel>
        {
            public int StudentID { get; set; }
        }

        public class QueryHandler : IRequestHandler<Query, StudentJobActivityViewModel>
        {
            private readonly IAcademiaRepository _academiaRepository;

            public QueryHandler( IAcademiaRepository academiaRepository)
            { 
                _academiaRepository = academiaRepository;
            }

            public async Task<StudentJobActivityViewModel> Handle(Query request, CancellationToken cancellationToken)
            {
                StudentJobActivityViewModel model = await _academiaRepository.GetStudentJobActivityReport(request.StudentID);
                return model;
            }
        }


    }
}
