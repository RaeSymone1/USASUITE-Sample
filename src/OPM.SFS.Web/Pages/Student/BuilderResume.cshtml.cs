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
using Microsoft.EntityFrameworkCore;
using OPM.SFS.Data;
using OPM.SFS.Web.Infrastructure.Extensions;
using OPM.SFS.Web.Models;
using OPM.SFS.Web.SharedCode;

namespace OPM.SFS.Web.Pages.Student
{
    [Authorize(Roles = "ST")]
    [IsProfileCompleteFilter()]
    public class BuilderResumeModel : PageModel
    {
        [BindProperty]
        public BuilderResumeViewModel Data { get; set; }

        [FromQuery(Name = "s")]
        public string IsSuccess { get; set; }

        private readonly IMediator _mediator;

        public BuilderResumeModel(IMediator mediator) => _mediator = mediator;

        public async Task OnGetAsync()
        {
            Data = await _mediator.Send(new Query() { Id = User.GetUserId() });
            if (!string.IsNullOrWhiteSpace(IsSuccess) && IsSuccess == "true") Data.ShowSuccessMessage = true;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();
            var response = await _mediator.Send(new Command() { Model = Data, Id = User.GetUserId() });
            return RedirectToPage("ResumeList");
        }

        public async Task<IActionResult> OnPostDeleteWorkExpAsync(int wid)
        {
            _ = await _mediator.Send(new CommandDeleteWorkExperience() { WorkExperienceId = wid, StudentId = User.GetUserId() });
            return RedirectToPage("BuilderResume", new { s = "true" });
        }

        public async Task<IActionResult> OnPostDeleteEducationAsync(int eid)
        {
            _ = await _mediator.Send(new CommandDeleteEducation() { EducationId = eid, StudentId = User.GetUserId() });
            return RedirectToPage("BuilderResume", new { s = "true" });
        }
        

        public class BuilderResumeValidator : AbstractValidator<BuilderResumeViewModel>
        {
            public BuilderResumeValidator()
            {                
                RuleFor(x => x.Objective).Matches("^[^><&]+$").MaximumLength(5000);
                RuleFor(x => x.Coursework).Matches("^[^><&]+$").MaximumLength(5000);
                RuleFor(x => x.OtherQuals).Matches("^[^><&]+$").MaximumLength(5000);
                RuleFor(x => x.JobRelatedSkils).Matches("^[^><&]+$").MaximumLength(5000);
                RuleFor(x => x.Certs).Matches("^[^><&]+$").MaximumLength(5000);
                RuleFor(x => x.Awards).Matches("^[^><&]+$").MaximumLength(5000);
                RuleFor(x => x.Supplemental).Matches("^[^><&]+$").MaximumLength(5000);
            }
        }

        public class Query : IRequest<BuilderResumeViewModel>
        {
            public int Id { get; set; }
        }

        public class QueryHandler : IRequestHandler<Query, BuilderResumeViewModel>
        {
            private readonly ScholarshipForServiceContext _db;
            public QueryHandler(ScholarshipForServiceContext db) => _db = db;
          
            public async Task<BuilderResumeViewModel> Handle(Query request, CancellationToken cancellationToken)
            {

                var data = await _db.Students.Where(m => m.StudentId == request.Id)
                    .Include(m => m.StudentBuilderResumes).ThenInclude(m => m.WorkExperiences)
                    .Include(m => m.StudentBuilderResumes).ThenInclude(m => m.Educations)
                    .Include(m => m.PermanentAddress)
                        .ThenInclude(s => s.State)
                    .Include(m => m.CurrentAddress)
                        .ThenInclude(s => s.State)
                    .AsNoTracking()
                    .Select(stud => new
                    {
                        Fullname = $"{stud.FirstName}, {stud.LastName}",
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
                        WorkExperience = stud.StudentBuilderResumes.FirstOrDefault().WorkExperiences,
                        Education = stud.StudentBuilderResumes.FirstOrDefault().Educations
                    })
                    .FirstOrDefaultAsync();
                  
                var model = new BuilderResumeViewModel();
                model.FullName = data.Fullname;
                model.FullAddress = data.FullAddress;
                model.Phone = data.Phone;
                model.OtherPhone = data.OtherPhone;
                model.Fax = data.Fax;
                model.Email = data.Email;
                model.AltEmail = data.AltEmail;
                model.Objective = data.Objective;
                model.Coursework = data.Coursework;
                model.JobRelatedSkils = data.JobRelatedSkills;
                model.Certs = data.Certs;
                model.Honors = data.Honors;
                model.Supplemental = data.Supplemental;

                if(data.WorkExperience is not null)
                {
                    model.WorkExperienceList = new();
                    foreach (var w in data.WorkExperience)
                    {
                        model.WorkExperienceList.Add(new Models.WorkExperience()
                        {
                            Employer = w.Employer,
                            Title = w.Title,
                            WorkExperienceId = w.WorkExperienceId,
                            EmploymentDate = $"{w.Start} to {w.End}"
                        });
                    }
                }

                if(data.Education is not null)
                {
                    model.EducationList = new();
                    foreach (var w in data.Education)
                    {
                        model.EducationList.Add(new Models.Education()
                        {
                            School = w.SchoolName,
                            Degree = w.Degree,
                            Major = w.Major,
                            EducationId = w.EducationId
                        });
                    }

                }
                return model;
            }
        }

        public class Command : IRequest<bool>
        {
            public int Id { get; set; }
            public BuilderResumeViewModel Model { get; set; }
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
                var resumeToUpdate = await _db.StudentBuilderResumes.Where(m => m.StudentId == request.Id).Include( m => m.StudentDocument).Include(m => m.Student).FirstOrDefaultAsync();
                //var resumeToUpdate = await _db.StudentDocuments.Where(m => m.StudentDocumentId == request.Id).Include(m => m.StudentBuilderResumes).Include(m => m.Student).FirstOrDefaultAsync();
                if (resumeToUpdate is not null)
                {
                    resumeToUpdate.Objective = request.Model.Objective;
                    resumeToUpdate.OtherQualification = request.Model.Coursework;
                    resumeToUpdate.JobRelatedSkill = request.Model.JobRelatedSkils;
                    resumeToUpdate.Certificate = request.Model.Certs;
                    resumeToUpdate.HonorsAwards = request.Model.Honors;
                    resumeToUpdate.Supplemental = request.Model.Supplemental;
                    resumeToUpdate.LastModified = DateTime.UtcNow;
                }
                else
                {
                    var docTypes = await _cache.GetDocumentTypeAsync();
                    var resumeTypeID = docTypes.Where(m => m.Name == "Resume").Select(m => m.DocumentTypeId).FirstOrDefault();
                    StudentBuilderResume newBuilder = new StudentBuilderResume
                    {
                        StudentId = request.Id,
                        Objective = request.Model.Objective,
                        OtherQualification = request.Model.Coursework,
                        JobRelatedSkill = request.Model.JobRelatedSkils,
                        Certificate = request.Model.Certs,
                        HonorsAwards = request.Model.Honors,
                        Supplemental = request.Model.Supplemental,
                        DateInserted = DateTime.UtcNow
                    };
                    List<StudentBuilderResume> resumeList = new();
                    resumeList.Add(newBuilder);

                    StudentDocument newResumeDocument = new StudentDocument
                    {
                        StudentId = request.Id,
                        FileName = "Builder Resume",
                        FilePath = "",
                        DateCreated = DateTime.UtcNow,
                        DocumentTypeId = resumeTypeID,
                        StudentBuilderResumes = resumeList
                    };

                    _db.StudentDocuments.Add(newResumeDocument);
                    //resumeToUpdate.Student.StudentDocuments.Add(newResumeDocument);
                   
                    
                }
                await _db.SaveChangesAsync();
                return true;
             
            }
        }

        public class CommandDeleteWorkExperience : IRequest<bool>
        {
            public int WorkExperienceId { get; set; }
            public int StudentId { get; set; }
        }

        public class CommandDeleteWorkExperienceHandler : IRequestHandler<CommandDeleteWorkExperience, bool>
        {
            private readonly ScholarshipForServiceContext _db;
            public CommandDeleteWorkExperienceHandler(ScholarshipForServiceContext db) => _db = db;

            public async Task<bool> Handle(CommandDeleteWorkExperience request, CancellationToken cancellationToken)
            {
                var workExperience = await _db.WorkExperiences
                    .Include(m => m.StudentBuilderResume)
                    .Where(m => m.StudentBuilderResume.StudentId == request.StudentId && request.WorkExperienceId == request.WorkExperienceId)
                    .FirstOrDefaultAsync();

                if (workExperience is not null)
                {
                    _db.WorkExperiences.Remove(workExperience);
                    await _db.SaveChangesAsync();
                }
                return true;
            }
        }

        public class CommandDeleteEducation : IRequest<bool>
        {
            public int EducationId { get; set; }
            public int StudentId { get; set; }
        }

        public class CommandDeleteEducationHandler : IRequestHandler<CommandDeleteEducation, bool>
        {
            private readonly ScholarshipForServiceContext _db;
            public CommandDeleteEducationHandler(ScholarshipForServiceContext db) => _db = db;

            public async Task<bool> Handle(CommandDeleteEducation request, CancellationToken cancellationToken)
            {
                var education = await _db.Educations 
                   .Include(m => m.StudentBuilderResume)
                   .Where(m => m.StudentBuilderResume.StudentId == request.StudentId && request.EducationId == request.EducationId)
                   .FirstOrDefaultAsync();

                if (education is not null)
                {
                    _db.Educations.Remove(education);
                    await _db.SaveChangesAsync();
                }
                return true;
            }
        }

    }

  
}
