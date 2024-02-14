using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OPM.SFS.Data;
using OPM.SFS.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace OPM.SFS.Web.Pages.Admin
{
    public class StudentBuilderModel : PageModel
    {
        [BindProperty]
        public ViewResumeViewModel Data { get; set; }

        [FromQuery(Name = "sid")]
        public int StudentID { get; set; }

        private readonly IMediator _mediator;

        public StudentBuilderModel(IMediator mediator) => _mediator = mediator;

        public async Task OnGetAsync()
        {
            Data = await _mediator.Send(new Query() { StudentId = StudentID });
        }

        public class Query : IRequest<ViewResumeViewModel>
        {
            public int StudentId { get; set; }
        }

        public class QueryHandler : IRequestHandler<Query, ViewResumeViewModel>
        {
            private readonly ScholarshipForServiceContext _db;
            public QueryHandler(ScholarshipForServiceContext db) => _db = db;

            public async Task<ViewResumeViewModel> Handle(Query request, CancellationToken cancellationToken)
            {
                var data = await _db.Students.Where(m => m.StudentId == request.StudentId)
                   .Select(stud => new
                   {
                       Fullname = $"{stud.FirstName} {stud.LastName}",
                       FullAddress = $"{stud.PermanentAddress.LineOne}, {stud.PermanentAddress.State.Name}, {stud.PermanentAddress.PostalCode}",
                       Phone = stud.PermanentAddress.PhoneNumber,
                       OtherPhone = stud.CurrentAddress.PhoneNumber,
                       Fax = stud.PermanentAddress.Fax,
                       Email = stud.Email,
                       AltEmail = stud.AlternateEmail,
                       Objective = stud.StudentBuilderResumes.FirstOrDefault().Objective,
                       Coursework = stud.StudentBuilderResumes.FirstOrDefault().OtherQualification,
                       JobRelatedSkills = stud.StudentBuilderResumes.FirstOrDefault().JobRelatedSkill,
                       Certs = stud.StudentBuilderResumes.FirstOrDefault().Certificate,
                       Honors = stud.StudentBuilderResumes.FirstOrDefault().HonorsAwards,
                       Supplemental = stud.StudentBuilderResumes.FirstOrDefault().Supplemental,
                       WorkExperience = stud.StudentBuilderResumes.FirstOrDefault().WorkExperiences.Select(m => new
                       {
                           Employer = m.Employer,
                           Title = m.Title,
                           Dates = $"{m.Start} to {m.End}",
                           AddressLine = $"{m.Address.City}, {m.Address.State.Abbreviation}",
                           Hours = m.HoursPerWeek,
                           Salary = m.Salary,
                           Duties = m.Duties
                       }),
                       Education = stud.StudentBuilderResumes.FirstOrDefault().Educations.Select(m => new
                       {
                           School = m.SchoolName,
                           AddressLine = $"{m.CityName}, {m.State.Abbreviation}",
                           Degree = m.Degree,
                           Major = m.Major,
                           Minor = m.Minor,
                           Honors = m.Honors
                       })
                   })
                   .FirstOrDefaultAsync();

                var model = new ViewResumeViewModel();
                model.FullName = data.Fullname;
                model.FullAddress = data.FullAddress;
                model.Email = data.Email;
                model.AltEmail = data.AltEmail;
                model.Phone = data.Phone;
                model.Objective = data.Objective;

                if (data.WorkExperience is not null && data.WorkExperience.Count() > 0)
                {
                    model.WorkExperience = new List<ViewWorkExperience>();
                    foreach (var w in data.WorkExperience)
                    {
                        model.WorkExperience.Add(new ViewWorkExperience()
                        {
                            Employer = w.Employer,
                            Title = w.Title,
                            Dates = w.Dates,
                            AddressLine = w.AddressLine,
                            Hours = w.Hours,
                            Salary = w.Salary,
                            Duties = w.Duties
                        });
                    }
                }

                if (data.Education is not null && data.Education.Count() > 0)
                {
                    model.Education = new List<ViewEduction>();
                    foreach (var e in data.Education)
                    {
                        model.Education.Add(new ViewEduction()
                        {
                            School = e.School,
                            AddressLine = e.AddressLine,
                            Degree = e.Degree,
                            Major = e.Major,
                            Minor = e.Minor,
                            Honors = e.Honors
                        });
                    }
                }
                model.Courses = data.Coursework;
                model.JobRelatedSkills = data.JobRelatedSkills;
                model.Certifications = data.Certs;
                model.Honors = data.Honors;
                model.Supplemental = data.Supplemental;
                return model;
            }
        }

    }
}
