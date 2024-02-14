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

namespace OPM.SFS.Web.Pages.Admin
{
    [Authorize(Roles = "AD")]
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
            private readonly ScholarshipForServiceContext _db;

            public QueryHandler(ScholarshipForServiceContext db) => _db = db;


            public async Task<StudentJobActivityViewModel> Handle(Query request, CancellationToken cancellationToken)
            {
                var data = await _db.Students.Where(m => m.StudentId == request.StudentID)
                   .Include(m => m.StudentJobActivities)
                        .ThenInclude(m => m.JobActivityStatus)
                   .Include(m => m.StudentJobActivities)
                       .ThenInclude(m => m.AgencyType)
                    .Include(m => m.StudentInstitutionFundings).ThenInclude(m => m.Institution)
                    .Include(m => m.StudentInstitutionFundings).ThenInclude(m => m.Degree)
                   .FirstOrDefaultAsync();

                StudentJobActivityViewModel model = new();
                model.StudentID = request.StudentID;
                model.Studentname = $"{data.FirstName} {data.LastName}";
                model.University = data.StudentInstitutionFundings.FirstOrDefault().Institution.Name;
                model.DegreeMajor = data.StudentInstitutionFundings.FirstOrDefault().Degree.Name;
                model.GradDate = data.StudentInstitutionFundings.FirstOrDefault().ExpectedGradDate.HasValue ? $"{data.StudentInstitutionFundings.FirstOrDefault().ExpectedGradDate.Value.Month}/{data.StudentInstitutionFundings.FirstOrDefault().ExpectedGradDate.Value.Year}" : "";
                if (data.StudentJobActivities is not null && data.StudentJobActivities.Count > 0)
                {
                    model.Items = new();
                    foreach (var j in data.StudentJobActivities)
                    {
                        model.Items.Add(new StudentJobActivityViewModel.JobActivity()
                        {
                            JobActivityId = j.StudentJobActivityId,
                            DateApplied = j.DateApplied.HasValue ? j.DateApplied.Value.ToShortDateString() : "",
                            PositionTitle = j.PositionTitle,
                            USAJOBSControlNum = j.UsajobscontrolNumber,
                            AgencyTypeName = j.AgencyType is not null ? j.AgencyType.Name : "",
                            AgencyName = j.Agency,
                            DutyLocation = j.DutyLocation,
                            ContactFullName = $"{j.ContactFirstName} {j.ContactLastName}",
                            ContactEmail = j.ContactEmail,
                            Description = j.Description,
                            StatusOther = j.StatusOther,
                            StatusName = j.JobActivityStatus.Name,
                            LastUpdated = j.LastUpdated.HasValue ? j.LastUpdated.Value.ToShortDateString() : null
                        });
                    }
                }
                return model;
            }
        }
    }
}
