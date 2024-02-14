using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OPM.SFS.Core.Shared;
using OPM.SFS.Data;
using OPM.SFS.Web.Models.Agency;
using OPM.SFS.Web.SharedCode;

namespace OPM.SFS.Web.Pages.Agency
{
    [Authorize(Roles = "AO")]
    public class StudentSearchModel : PageModel
    {
        [BindProperty]
        public AgencyOfficialStudentSearchViewModel Data { get; set; }

        private readonly IMediator _mediator;

        public StudentSearchModel(IMediator mediator) => _mediator = mediator;

        public async Task OnGetAsync()
        {

            Data = await _mediator.Send(new Query());
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                Data = await _mediator.Send(new Query());
                return Page();
            }
            Data = await _mediator.Send(new Query() {  Model = Data });
            return Page();
        }

        public class AgencyStudentSearchValidator : AbstractValidator<AgencyOfficialStudentSearchViewModel>
        {
            public AgencyStudentSearchValidator()
            {               
                //RuleFor(x => x.InternshipStartDate).Matches("^[^><&]+$").WithName("Start Date");
                //RuleFor(x => x.InternshipStartDate).Must(IsValidDate).WithMessage("Start Date is invalid date format");
                //RuleFor(x => x.InternshipEndDate).Matches("^[^><&]+$").WithName("End Date");
                //RuleFor(x => x.InternshipEndDate).Must(IsValidDate).WithMessage("End Date is invalid date format");
                //RuleFor(x => x.PostGradStartDate).Matches("^[^><&]+$").WithName("Start Date");
                //RuleFor(x => x.PostGradStartDate).Must(IsValidDate).WithMessage("Start Date is invalid date format");
                //RuleFor(x => x.PostGradEndDate).Matches("^[^><&]+$").WithName("End Date");
                //RuleFor(x => x.PostGradEndDate).Must(IsValidDate).WithMessage("End Date is invalid date format");
                //RuleFor(x => x.GraduationYear).Matches("^[^><&]+$").WithName("Year");
                //RuleFor(x => x.GraduationYear).Must(IsValidYear).WithMessage("Year is invalid format");
            }

            protected bool IsValidYear(string year)
            {
                if (string.IsNullOrWhiteSpace(year)) return true;
                if (int.TryParse(year, out _)) return true;
                return false;
            }

            protected bool IsValidDate(string date)
            {
                if (string.IsNullOrWhiteSpace(date)) return true;
                if (DateTime.TryParse(date, out _)) return true;
                return false;
            }
        }


        public class Query : IRequest<AgencyOfficialStudentSearchViewModel>
        {
            public AgencyOfficialStudentSearchViewModel Model { get; set; }
        }

        public class QueryHandler : IRequestHandler<Query, AgencyOfficialStudentSearchViewModel>
        {
            private readonly ICacheHelper _cache;
            private readonly ScholarshipForServiceContext _efDB;
            private readonly IUtilitiesService _utilities;

            public QueryHandler(ICacheHelper cache, ScholarshipForServiceContext db, IUtilitiesService utilities)
            {
                _cache = cache;
                _efDB = db;
                _utilities= utilities;
            }

            public async Task<AgencyOfficialStudentSearchViewModel> Handle(Query request, CancellationToken cancellationToken)
            {
                if(request.Model is null)
                {
                    AgencyOfficialStudentSearchViewModel model = new();
                    model.InstitutionList = new SelectList(await _cache.GetInstitutionsAsync(), nameof(Institution.InstitutionId), nameof(Institution.Name));
                    model.DegreeList = new SelectList(await _cache.GetDegreesAsync(), nameof(Degree.DegreeId), nameof(Degree.Name));
                    model.DisciplineList = new SelectList(await _cache.GetDisciplineAsync(), nameof(Discipline.DisciplineId), nameof(Discipline.Name));
                    model.YearList = new SelectList(GetYearList(), nameof(AgencyOfficialStudentSearchViewModel.SelectListOption.ID), nameof(AgencyOfficialStudentSearchViewModel.SelectListOption.Value));
                    model.QuarterList =  new SelectList(GetQuarterList(), nameof(AgencyOfficialStudentSearchViewModel.SelectListOption.ID), nameof(AgencyOfficialStudentSearchViewModel.SelectListOption.Value));
                    model.SearchResults = new();
                    return model;
                }


                request.Model.SearchResults = new();
                var searchResults = await _efDB.Students.Include(m => m.StudentCommitments)
                    .Where(m => m.StudentCommitments.Any() == false)
                    .WhereIf(request.Model.Degree > 0, m => m.StudentInstitutionFundings.FirstOrDefault().Degree.DegreeId == request.Model.Degree)
                    .WhereIf(request.Model.Institition > 0, m => m.StudentInstitutionFundings.FirstOrDefault().Institution.InstitutionId == request.Model.Institition)
                    .WhereIf(request.Model.Discipline > 0, m => m.StudentInstitutionFundings.FirstOrDefault().Major.DisciplineId == request.Model.Discipline)
                    .WhereIf(!string.IsNullOrWhiteSpace(request.Model.Firstname), m => m.FirstName == request.Model.Firstname)                   
                    .WhereIf(!string.IsNullOrWhiteSpace(request.Model.Lastname), m => m.LastName == request.Model.Lastname)
                    .WhereIf(request.Model.InternYear.HasValue && request.Model.InternYear > 0 
                            && request.Model.InternQuarter.HasValue 
                            && request.Model.InternQuarter.Value >= 0, 
                            m => m.StudentInstitutionFundings.FirstOrDefault().InternshipAvailDate > QuarterToDateRange(request.Model.InternQuarter.Value, request.Model.InternYear.Value).Item1)
                    .WhereIf(request.Model.InternYear.HasValue && request.Model.InternYear > 0
                            && request.Model.InternQuarter.HasValue
                            && request.Model.InternQuarter.Value >= 0,
                            m => m.StudentInstitutionFundings.FirstOrDefault().InternshipAvailDate <= QuarterToDateRange(request.Model.InternQuarter.Value, request.Model.InternYear.Value).Item2)
                    .WhereIf(request.Model.PostGradYear.HasValue && request.Model.PostGradYear > 0
                            && request.Model.PostGradQuarter.HasValue
                            && request.Model.PostGradQuarter.Value >= 0,
                            m => m.StudentInstitutionFundings.FirstOrDefault().PostGradAvailDate > QuarterToDateRange(request.Model.PostGradQuarter.Value, request.Model.PostGradYear.Value).Item1)
                    .WhereIf(request.Model.PostGradYear.HasValue && request.Model.PostGradYear > 0
                            && request.Model.PostGradQuarter.HasValue
                            && request.Model.PostGradQuarter.Value >= 0,
                            m => m.StudentInstitutionFundings.FirstOrDefault().PostGradAvailDate <= QuarterToDateRange(request.Model.PostGradQuarter.Value, request.Model.PostGradYear.Value).Item2)
                    .Select(m => new AgencyOfficialStudentSearchViewModel.StudentResult()
                    {
                        StudentID = m.StudentId,
                        StudentName = $"{m.LastName}, {m.FirstName}",
                        Degree = m.StudentInstitutionFundings.FirstOrDefault().Degree.Name,
                        Institution = m.StudentInstitutionFundings.FirstOrDefault().Institution.Name,
                        Discipline = m.StudentInstitutionFundings.FirstOrDefault().Major.Name,
                        InternshipDate = m.StudentInstitutionFundings.FirstOrDefault().InternshipAvailDate.HasValue ? $"{m.StudentInstitutionFundings.FirstOrDefault().InternshipAvailDate.Value.Month}/{m.StudentInstitutionFundings.FirstOrDefault().InternshipAvailDate.Value.Year}" : "",
                        PostGradDate = m.StudentInstitutionFundings.FirstOrDefault().PostGradAvailDate.HasValue ? $"{m.StudentInstitutionFundings.FirstOrDefault().PostGradAvailDate.Value.Month}/{m.StudentInstitutionFundings.FirstOrDefault().PostGradAvailDate.Value.Year}" : "",

                    }).ToListAsync();
                request.Model.SearchResults = searchResults;
                request.Model.ShowResults = true;
                return request.Model;
       

            }

            public List<AgencyOfficialStudentSearchViewModel.SelectListOption> GetYearList()
            {
                List<AgencyOfficialStudentSearchViewModel.SelectListOption> Years = new();
                for(int i = 2000; i < 2025; i++)
                {
                    Years.Add(new AgencyOfficialStudentSearchViewModel.SelectListOption() { ID = i, Value = i.ToString()});
                }
                return Years;
            }

            public List<AgencyOfficialStudentSearchViewModel.SelectListOption> GetQuarterList()
            {
                List<AgencyOfficialStudentSearchViewModel.SelectListOption> QuarterList = new();
                QuarterList.Add(new AgencyOfficialStudentSearchViewModel.SelectListOption() { ID = 0, Value = "All" });
                QuarterList.Add(new AgencyOfficialStudentSearchViewModel.SelectListOption() { ID = 1, Value = "Jan/Feb/Mar" });
                QuarterList.Add(new AgencyOfficialStudentSearchViewModel.SelectListOption() { ID = 2, Value = "Apr/May/Jun" });
                QuarterList.Add(new AgencyOfficialStudentSearchViewModel.SelectListOption() { ID = 3, Value = "Jul/Aug/Sep" });
                QuarterList.Add(new AgencyOfficialStudentSearchViewModel.SelectListOption() { ID = 4, Value = "Oct/Nov/Dec" });
                return QuarterList;

            }

            public (DateTime,DateTime) QuarterToDateRange(int input, int year)
            {
                DateTime start = _utilities.ConvertUtcToEastern(DateTime.UtcNow);
                DateTime end = _utilities.ConvertUtcToEastern(DateTime.UtcNow);
                if(input == 0)
                {
                    start = Convert.ToDateTime($"1/1/{year}");
                    end = Convert.ToDateTime($"12/31/{year}");
                }
                if(input == 1)
                {
                    start = Convert.ToDateTime($"1/1/{year}");
                    end = Convert.ToDateTime($"3/31/{year}");
                }
                if(input == 2)
                {
                    start = Convert.ToDateTime($"4/1/{year}");
                    end = Convert.ToDateTime($"6/30/{year}");
                }
                if (input == 3)
                {
                    start = Convert.ToDateTime($"7/1/{year}");
                    end = Convert.ToDateTime($"9/30/{year}");
                }
                if (input == 4)
                {
                    start = Convert.ToDateTime($"10/1/{year}");
                    end = Convert.ToDateTime($"12/31/{year}");
                }
                return (start, end); 
                    
            }
        }

        
    }
}
