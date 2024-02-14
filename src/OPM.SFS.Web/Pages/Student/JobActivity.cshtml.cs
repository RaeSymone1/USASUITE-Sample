using System;
using System.Collections.Generic;
using System.Globalization;
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
using OPM.SFS.Web.Infrastructure.Extensions;
using OPM.SFS.Web.Models;

namespace OPM.SFS.Web.Pages.Student
{
    [Authorize(Roles = "ST")]
    [IsProfileCompleteFilter()]
    public class JobActivityModel : PageModel
    {
        
        [BindProperty]
        public JobActivityViewModel Data { get; set; }

        [FromQuery(Name = "s")]
        public string IsSuccess { get; set; }

        private readonly IMediator _mediator;

        public JobActivityModel(IMediator mediator) => _mediator = mediator;

        public async Task OnGetAsync()
        {
            Data = await _mediator.Send(new Query { StudentId = User.GetUserId() });
            if (!string.IsNullOrWhiteSpace(IsSuccess) && IsSuccess == "true") Data.ShowSuccessMessage = true;
        }

    
        public class Query : IRequest<JobActivityViewModel>
        {
            public int StudentId { get; set; }
        }

        public class QueryHandler : IRequestHandler<Query, JobActivityViewModel>
        {
            private readonly ScholarshipForServiceContext _db;

            public QueryHandler(ScholarshipForServiceContext db) => _db = db;

            public async Task<JobActivityViewModel> Handle(Query request, CancellationToken cancellationToken)
            {
                var data = await _db.Students.Where(m => m.StudentId == request.StudentId)
                    .Include(m => m.StudentJobActivities)
                    .Include(m => m.StudentJobActivities )
                        .ThenInclude(m => m.AgencyType)
                    .Include(m => m.StudentInstitutionFundings).ThenInclude(m => m.Institution)
                    .Include(m => m.StudentInstitutionFundings).ThenInclude(m => m.Degree)
                    .FirstOrDefaultAsync();
                TextInfo textinfo = new CultureInfo("en-US", false).TextInfo;
                JobActivityViewModel model = new();
                model.Studentname = $"{data.FirstName} {data.LastName}";
                model.University = data.StudentInstitutionFundings.FirstOrDefault().Institution.Name;
                model.DegreeMajor = data.StudentInstitutionFundings.FirstOrDefault().Degree.Name;
                model.GradDate = data.StudentInstitutionFundings.FirstOrDefault().ExpectedGradDate.HasValue ? $"{data.StudentInstitutionFundings.FirstOrDefault().ExpectedGradDate.Value.Month}/{data.StudentInstitutionFundings.FirstOrDefault().ExpectedGradDate.Value.Year}" : "";
                if (data.StudentJobActivities is not null && data.StudentJobActivities.Count > 0)
                {
                    model.Items = new();
                    foreach(var j in data.StudentJobActivities)
                    {
                        model.Items.Add(new JobActivity()
                        {
                            JobActivityId = j.StudentJobActivityId,
                            DateApplied = j.DateApplied.HasValue ? j.DateApplied.Value.ToShortDateString() : "",
                            PositionTitle = j.PositionTitle,
                            USAJOBSControlNum = j.UsajobscontrolNumber,
                            AgencyTypeName = j.AgencyType is not null ? j.AgencyType.Name : "",
                            AgencyName = j.Agency,
                            DutyLocation = j.DutyLocation,
                            ContactFirstName = !string.IsNullOrWhiteSpace(j.ContactFirstName) ? textinfo.ToTitleCase(j.ContactFirstName.ToLower()) : "",
                            ContactLastName = !string.IsNullOrWhiteSpace(j.ContactLastName) ? textinfo.ToTitleCase(j.ContactLastName.ToLower()) : "",
                            ContactEmail = j.ContactEmail,
                            Description = j.Description,
                            CurrentStatus = j.StatusOther,
                            LastUpdated = j.LastUpdated.HasValue ? j.LastUpdated.Value.ToShortDateString() : null
                        });
                    }                  

                }
                return model;
            }
        }

    }

    
}
