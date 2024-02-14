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
    public class WorkExperienceModel : PageModel
    {
        [FromQuery(Name = "cid")]
        public int WorkExperienceId { get; set; } = 0;

        [BindProperty]
        public WorkExperienceViewModel Data { get; set; }

        private readonly IMediator _mediator;

        public WorkExperienceModel(IMediator mediator) => _mediator = mediator;

        public async Task OnGetAsync()
        {
            Data = await _mediator.Send(new Query() { StudentId = User.GetUserId(), WorkExperienceID  = WorkExperienceId });
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {               
                return Page();
            }
            var response = await _mediator.Send(new Command() { Model = Data, StudentId = User.GetUserId() });
            return RedirectToPage("BuilderResume", new { s = "true" });
        }

      

        public class WorkExperienceValidator : AbstractValidator<WorkExperienceViewModel>
        {
            public WorkExperienceValidator()
            {
                RuleFor(x => x.Employer).NotEmpty().Matches("^[^><&]+$");
                RuleFor(x => x.Address1).NotEmpty().Matches("^[^><&]+$");
                RuleFor(x => x.City).NotEmpty().Matches("^[^><&]+$");
                RuleFor(x => x.State).NotEmpty();
                RuleFor(x => x.ZipCode).NotEmpty().Matches("^[^><&]+$");
                RuleFor(x => x.Start).NotEmpty().Matches("^[^><&]+$").WithName("From");
                RuleFor(x => x.End).NotEmpty().Matches("^[^><&]+$").WithName("To");
                RuleFor(x => x.Title).NotEmpty().Matches("^[^><&]+$");
                RuleFor(x => x.HoursPerWeek).NotEmpty().Matches("^[^><&]+$");
                RuleFor(x => x.Duties).NotEmpty().Matches("^[^><&]+$");
                RuleFor(x => x.Address2).Matches("^[^><&]+$");
                RuleFor(x => x.Address3).Matches("^[^><&]+$");
                RuleFor(x => x.Country).Matches("^[^><&]+$");
                RuleFor(x => x.Series).Matches("^[^><&]+$");
                RuleFor(x => x.Payplan).Matches("^[^><&]+$");
                RuleFor(x => x.Grade).Matches("^[^><&]+$");
                RuleFor(x => x.Salary).Matches("^[^><&]+$");
                RuleFor(x => x.Supervisor).Matches("^[^><&]+$");
                RuleFor(x => x.SupervisorPhone).Matches("^[^><&]+$");
                RuleFor(x => x.SupervisorPhoneExt).Matches("^[^><&]+$");
             
            }
        }

        public class Query : IRequest<WorkExperienceViewModel>
        {
            public int StudentId { get; set; }
            public int WorkExperienceID { get; set; }
        }

        public class QueryHandler : IRequestHandler<Query, WorkExperienceViewModel>
        {
            private readonly ScholarshipForServiceContext _db;
            private readonly ICacheHelper _cache;
            public QueryHandler(ScholarshipForServiceContext db, ICacheHelper cache)
            {
                _db = db;
                _cache = cache;

            }

            public async Task<WorkExperienceViewModel> Handle(Query request, CancellationToken cancellationToken)
            {
                var model = new WorkExperienceViewModel();
                model.StateList = new SelectList(await _cache.GetStatesAsync(), nameof(State.StateId), nameof(State.Name));
                model.WorkExperienceID = request.WorkExperienceID;

                if (request.WorkExperienceID > 0)
                {
                   
                    var data = await _db.WorkExperiences
                   .AsNoTracking()
                   .Include(m => m.StudentBuilderResume)
                   .Include(m => m.Address)
                   .Where(m => m.WorkExperienceId == request.WorkExperienceID && m.StudentBuilderResume.StudentId == request.StudentId).FirstOrDefaultAsync();                    
                    if (data is not null)
                    {
                        model.Employer = data.Employer;
                        model.Address1 = data.Address.LineOne;
                        model.Address2 = data.Address.LineTwo;
                        model.Address3 = data.Address.LineThree;
                        model.City = data.Address.City;
                        model.State = data.Address.StateId;
                        model.ZipCode = data.Address.PostalCode;
                        model.Start = data.Start;
                        model.End = data.End;
                        model.Title = data.Title;
                        model.Series = data.Series;
                        model.Payplan = data.PayPlan;
                        model.Grade = data.Grade;
                        model.Salary = data.Salary;
                        model.HoursPerWeek = data.HoursPerWeek;
                        model.Supervisor = data.SupervisorName;
                        model.SupervisorPhone = data.SupervisorPhone;
                        model.SupervisorPhoneExt = data.SupervisorPhoneExt;
                        model.Duties = data.Duties;
                    }
                }
                return model;
            }
        }

        public class Command : IRequest<bool>
        {           
            public int StudentId { get; set; }
            public WorkExperienceViewModel  Model { get; set; }
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
                if (request.Model.WorkExperienceID > 0)
                {
                    var data = await _db.WorkExperiences
                       .Include(m => m.StudentBuilderResume)
                       .Include(m => m.Address)
                       .Where(m => m.WorkExperienceId == request.Model.WorkExperienceID && m.StudentBuilderResume.StudentId == request.StudentId).FirstOrDefaultAsync();

                    data.Employer = request.Model.Employer;
                    data.Address.LineOne = request.Model.Address1;
                    data.Address.LineTwo = request.Model.Address2;
                    data.Address.LineThree = request.Model.Address3;
                    data.Address.City = request.Model.City;
                    data.Address.StateId = request.Model.State;
                    data.Address.PostalCode = request.Model.ZipCode;
                    data.Address.Country = request.Model.Country;
                    data.Start = request.Model.Start;
                    data.End = request.Model.End;
                    data.Title = request.Model.Title;
                    data.Series = request.Model.Series;
                    data.PayPlan = request.Model.Payplan;
                    data.Grade = request.Model.Grade;
                    data.Salary = request.Model.Salary;
                    data.HoursPerWeek = request.Model.HoursPerWeek;
                    data.SupervisorName = request.Model.Supervisor;
                    data.SupervisorPhone = request.Model.SupervisorPhone;
                    data.SupervisorPhoneExt = request.Model.SupervisorPhoneExt;
                    data.Duties = request.Model.Duties;
                    data.StudentBuilderResume.LastModified = DateTime.UtcNow;
                    await _db.SaveChangesAsync();
                }
                else
                {
                    var resume = await _db.StudentBuilderResumes
                        .Where(m => m.StudentId == request.StudentId)
                        .Include(m => m.WorkExperiences)
                        .FirstOrDefaultAsync();
                    if (resume is not null)
                    {
                        var newWorkExperience = new Data.WorkExperience();
                        newWorkExperience.Employer = request.Model.Employer;
                        newWorkExperience.Address = new();
                        newWorkExperience.Address.LineOne = request.Model.Address1;
                        newWorkExperience.Address.LineTwo = request.Model.Address2;
                        newWorkExperience.Address.LineThree = request.Model.Address3;
                        newWorkExperience.Address.City = request.Model.City;
                        newWorkExperience.Address.StateId = request.Model.State;
                        newWorkExperience.Address.PostalCode = request.Model.ZipCode;
                        newWorkExperience.Address.Country = request.Model.Country;
                        newWorkExperience.Start = request.Model.Start;
                        newWorkExperience.End = request.Model.End;
                        newWorkExperience.Title = request.Model.Title;
                        newWorkExperience.Series = request.Model.Series;
                        newWorkExperience.PayPlan = request.Model.Payplan;
                        newWorkExperience.Grade = request.Model.Grade;
                        newWorkExperience.Salary = request.Model.Salary;
                        newWorkExperience.HoursPerWeek = request.Model.HoursPerWeek;
                        newWorkExperience.SupervisorName = request.Model.Supervisor;
                        newWorkExperience.SupervisorPhone = request.Model.SupervisorPhone;
                        newWorkExperience.SupervisorPhoneExt = request.Model.SupervisorPhoneExt;
                        newWorkExperience.Duties = request.Model.Duties;
                        resume.WorkExperiences.Add(newWorkExperience);
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
                            WorkExperiences = new List<OPM.SFS.Data.WorkExperience>()
                        };
                       
                        var newWorkExperience = new Data.WorkExperience();
                        newWorkExperience.Employer = request.Model.Employer;
                        newWorkExperience.Address = new();
                        newWorkExperience.Address.LineOne = request.Model.Address1;
                        newWorkExperience.Address.LineTwo = request.Model.Address2;
                        newWorkExperience.Address.LineThree = request.Model.Address3;
                        newWorkExperience.Address.City = request.Model.City;
                        newWorkExperience.Address.StateId = request.Model.State;
                        newWorkExperience.Address.PostalCode = request.Model.ZipCode;
                        newWorkExperience.Address.Country = request.Model.Country;
                        newWorkExperience.Start = request.Model.Start;
                        newWorkExperience.End = request.Model.End;
                        newWorkExperience.Title = request.Model.Title;
                        newWorkExperience.Series = request.Model.Series;
                        newWorkExperience.PayPlan = request.Model.Payplan;
                        newWorkExperience.Grade = request.Model.Grade;
                        newWorkExperience.Salary = request.Model.Salary;
                        newWorkExperience.HoursPerWeek = request.Model.HoursPerWeek;
                        newWorkExperience.SupervisorName = request.Model.Supervisor;
                        newWorkExperience.SupervisorPhone = request.Model.SupervisorPhone;
                        newWorkExperience.SupervisorPhoneExt = request.Model.SupervisorPhoneExt;
                        newWorkExperience.Duties = request.Model.Duties;
                        newBuilder.WorkExperiences.Add(newWorkExperience);

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
