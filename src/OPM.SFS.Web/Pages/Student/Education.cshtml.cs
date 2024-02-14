using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OPM.SFS.Data;
using OPM.SFS.Web.Infrastructure.Extensions;
using OPM.SFS.Web.Models;
using OPM.SFS.Web.SharedCode;

namespace OPM.SFS.Web.Pages.Student
{
    [Authorize(Roles = "ST")]
    [IsProfileCompleteFilter()]
    public class ResumeEducationModel : PageModel
    {
        [BindProperty]
        public EducationViewModel Data { get; set; }

        private readonly IMediator _mediator;

        [FromQuery(Name = "cid")]
        public int EducationId { get; set; } = 0;

        public ResumeEducationModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task OnGetAsync()
        {
            Data = await _mediator.Send(new Query { EducationId = EducationId, StudentId = User.GetUserId() });
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            var response = await _mediator.Send(new Command() { Model = Data, StudentId = User.GetUserId() });
            return RedirectToPage("BuilderResume", new { s = "true"} );
        }


        public class EducationValidator : AbstractValidator<EducationViewModel>
        {
            public EducationValidator()
            {
                RuleFor(x => x.SchoolName).NotEmpty().Matches("^[^><&]+$");
                RuleFor(x => x.SchoolType).NotEmpty();
                RuleFor(x => x.City).NotEmpty().Matches("^[^><&]+$");
                RuleFor(x => x.Degree).NotEmpty();
                RuleFor(x => x.Year).NotEmpty().Matches("^[^><&]+$");
                RuleFor(x => x.Credits).NotEmpty().Matches("^[^><&]+$");
                RuleFor(x => x.CreditType).NotEmpty().Matches("^[^><&]+$");
                RuleFor(x => x.Zipcode).Matches("^[^><&]+$");
                RuleFor(x => x.Country).Matches("^[^><&]+$");
                RuleFor(x => x.DegreeText).Matches("^[^><&]+$");
                RuleFor(x => x.TotalPoints).Matches("^[^><&]+$");
                RuleFor(x => x.CreditTypeOther).Matches("^[^><&]+$");
                RuleFor(x => x.Major).Matches("^[^><&]+$");
                RuleFor(x => x.Minor).Matches("^[^><&]+$");
                RuleFor(x => x.Honors).Matches("^[^><&]+$");

            }
        }

        public class Query : IRequest<EducationViewModel>
        {
            public int StudentId { get; set; }
            public int EducationId { get; set; }
        }

        
        public class QueryHandler : IRequestHandler<Query, EducationViewModel>
        {
            private readonly ScholarshipForServiceContext _db;
            private readonly ICacheHelper _cache;

            public QueryHandler(ScholarshipForServiceContext db, ICacheHelper cache) { 
                
                _db = db;
                _cache = cache;
            
            }

            public async Task<EducationViewModel> Handle(Query request, CancellationToken cancellationToken)
            {
                var model = new EducationViewModel();
                model.StateList = new SelectList(await _cache.GetStatesAsync(), nameof(State.StateId), nameof(State.Name));
                model.SchoolTypeList = new SelectList(await _cache.GetSchoolTypesAsync(), nameof(SchoolType.SchoolTypeID), nameof(SchoolType.SchoolTypeName));
                model.DegreeList = new SelectList(await _cache.GetDegreesAsync(), nameof(Degree.DegreeId), nameof(Degree.Name));
                model.EducationId = request.EducationId;
                if(request.EducationId > 0)
                {
                    var data = await _db.Educations
                   .AsNoTracking()
                   .Include(m => m.StudentBuilderResume)
                   .Where(m => m.EducationId == request.EducationId && m.StudentBuilderResume.StudentId == request.StudentId).FirstOrDefaultAsync();

                    if(data is not null)
                    {
                        model.SchoolName = data.SchoolName;
                        model.SchoolType = data.SchoolTypeID.HasValue ? data.SchoolTypeID.Value : 0;
                        model.City = data.CityName;
                        model.State = data.StateId.HasValue ? data.StateId.Value : 0;
                        model.Zipcode = data.PostalCode;
                        model.Country = data.Country;
                        model.Degree = data.Degree;
                        model.DegreeText = data.DegreeOther;
                        model.Year = data.CompletionYear.HasValue ? data.CompletionYear.Value.ToString() : "";
                        model.GPA = data.Gpa;
                        model.TotalPoints = data.Gpamax;
                        model.Credits = data.TotalCredits;
                        model.CreditType = data.CreditType;
                        model.CreditTypeOther = data.CreditTypeOther;
                        model.Major = data.Major;
                        model.Minor = data.Minor;
                        model.Honors = data.Honors;
                    }
                }
                return model;
            }
        }

        public class Command : IRequest<bool>
        {
            public int StudentId { get; set; }
            public EducationViewModel Model { get; set; }
        }

        public class CommandHandler : IRequestHandler<Command, bool>
        {
            private readonly ScholarshipForServiceContext _db;
            private readonly ICacheHelper _cache;

            public CommandHandler(ScholarshipForServiceContext db, ICacheHelper cache)
            {
                _db = db;
                _cache = cache;
            }

            public async Task<bool> Handle(Command request, CancellationToken cancellationToken)
            {
                if (request.Model.EducationId > 0)
                {
                    var data = await _db.Educations
                     .Include(m => m.StudentBuilderResume)
                     .Where(m => m.EducationId == request.Model.EducationId && m.StudentBuilderResume.StudentId == request.StudentId).FirstOrDefaultAsync();

                    data.SchoolName = request.Model.SchoolName;
                    data.SchoolTypeID = request.Model.SchoolType;
                    data.CityName = request.Model.City;
                    data.StateId = request.Model.State;
                    data.PostalCode = request.Model.Zipcode;
                    data.Country = request.Model.Country;
                    data.Degree = request.Model.Degree;
                    data.DegreeOther = request.Model.DegreeText;
                    data.CompletionYear = string.IsNullOrWhiteSpace(request.Model.Year) ? null : Convert.ToInt32(request.Model.Year);
                    data.Gpa = request.Model.GPA;
                    data.Gpamax = request.Model.TotalPoints;
                    data.CreditType = request.Model.CreditType;
                    data.CompletionYear = Convert.ToInt32(request.Model.Year);
                    data.TotalCredits = request.Model.Credits;
                    data.CreditTypeOther = request.Model.CreditTypeOther;
                    data.Major = request.Model.Major;
                    data.Minor = request.Model.Minor;
                    data.Honors = request.Model.Honors;
                    await _db.SaveChangesAsync();

                }
                else
                {
                    var resume = await _db.StudentBuilderResumes
                       .Where(m => m.StudentId == request.StudentId)
                       .Include(m => m.Educations)
                       .FirstOrDefaultAsync();

                    if (resume is not null) 
                    { 
                        var newEdu = new Data.Education();
                        newEdu.SchoolName = request.Model.SchoolName;
                        newEdu.SchoolTypeID = request.Model.SchoolType;
                        newEdu.CityName = request.Model.City;
                        newEdu.StateId = request.Model.State;
                        newEdu.PostalCode = request.Model.Zipcode;
                        newEdu.Country = request.Model.Country;
                        newEdu.Degree = request.Model.Degree;
                        newEdu.DegreeOther = request.Model.DegreeText;
                        newEdu.CompletionYear = string.IsNullOrWhiteSpace(request.Model.Year) ? null : Convert.ToInt32(request.Model.Year);
                        newEdu.Gpa = request.Model.GPA;
                        newEdu.Gpamax = request.Model.TotalPoints;
                        newEdu.CreditType = request.Model.CreditType;
                        newEdu.CreditTypeOther = request.Model.CreditTypeOther;
                        newEdu.Major = request.Model.Major;
                        newEdu.Minor = request.Model.Minor;
                        newEdu.Honors = request.Model.Honors;
                        newEdu.TotalCredits = request.Model.Credits;
                        resume.Educations.Add(newEdu);
                    }
                    else
                    {
                        //if there is no resume record then we need to add a record into StudentDocument and StudentBuilderResume
                        var docTypes = await _cache.GetDocumentTypeAsync();
                        var resumeTypeID = docTypes.Where(m => m.Name == "Resume").Select(m => m.DocumentTypeId).FirstOrDefault();
                        List<StudentBuilderResume> resumeList = new();
                        StudentBuilderResume newBuilder = new StudentBuilderResume
                        {
                            StudentId = request.StudentId,
                            Objective = "",
                            OtherQualification = "",
                            JobRelatedSkill = "",
                            Certificate = "",
                            HonorsAwards = "",
                            Supplemental = "",
                            DateInserted = DateTime.UtcNow,
                            Educations = new List<OPM.SFS.Data.Education>()
                        };

                        var newEdu = new Data.Education();
                        newEdu.SchoolName = request.Model.SchoolName;
                        newEdu.SchoolTypeID = request.Model.SchoolType;
                        newEdu.CityName = request.Model.City;
                        newEdu.StateId = request.Model.State;
                        newEdu.PostalCode = request.Model.Zipcode;
                        newEdu.Country = request.Model.Country;
                        newEdu.Degree = request.Model.Degree;
                        newEdu.DegreeOther = request.Model.DegreeText;
                        newEdu.CompletionYear = string.IsNullOrWhiteSpace(request.Model.Year) ? null : Convert.ToInt32(request.Model.Year);
                        newEdu.Gpa = request.Model.GPA;
                        newEdu.Gpamax = request.Model.TotalPoints;
                        newEdu.CreditType = request.Model.CreditType;
                        newEdu.CreditTypeOther = request.Model.CreditTypeOther;
                        newEdu.Major = request.Model.Major;
                        newEdu.Minor = request.Model.Minor;
                        newEdu.Honors = request.Model.Honors;
                        newEdu.TotalCredits = request.Model.Credits;
                        newBuilder.Educations.Add(newEdu);

                        resumeList.Add(newBuilder);
                        StudentDocument newResumeDocument = new StudentDocument
                        {
                            StudentId = request.StudentId,
                            FileName = "Builder Resume",
                            FilePath = "",
                            DateCreated = DateTime.UtcNow,
                            DocumentTypeId = resumeTypeID,
                            StudentBuilderResumes = resumeList
                        };
                        _db.StudentDocuments.Add(newResumeDocument);
                    }
                    await _db.SaveChangesAsync();
                }
                return true;
            }
        }
    }

    
}
