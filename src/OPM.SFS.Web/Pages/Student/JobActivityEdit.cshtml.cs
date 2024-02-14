using System;
using System.Collections.Generic;
using System.Globalization;
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
    public class JobActivityEditModel : PageModel
    {
        
        [BindProperty]
        public JobActivityEditViewModel Data { get; set; }

        [FromQuery(Name = "cid")]
        public int JobActivityId { get; set; } = 0;

        private readonly IMediator _mediator;

        public JobActivityEditModel(IMediator mediator) => _mediator = mediator;

        public async Task OnGetAsync()
        {
            Data = await _mediator.Send(new Query() { StudentId = User.GetUserId(), JobActivityId  = JobActivityId });
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await _mediator.Send(new QuerySelectList() { Model = Data });
                return Page();
            }
            var response = await _mediator.Send(new Command() { Model = Data, StudentId = User.GetUserId() });
            return RedirectToPage("JobActivity", new { s = "true" });
        }

        public class JobActivityValidator : AbstractValidator<JobActivityEditViewModel>
        {
            public JobActivityValidator()
            {
                RuleFor(x => x.DateApplied).NotEmpty().Matches("^[^><&]+$");
                RuleFor(x => x.PositionTitle).NotEmpty().Matches("^[^><&]+$");
                RuleFor(x => x.USAJControlNunber).Matches("^[^><&]+$");
                RuleFor(x => x.AgencyType).NotEmpty();
                RuleFor(x => x.AgencyName).NotEmpty().Matches("^[^><&]+$");
                RuleFor(x => x.DutyLocation).Matches("^[^><&]+$");
                RuleFor(x => x.ContactFirstname).Matches("^[^><&]+$").WithName("First name");
                RuleFor(x => x.ContactLastname).Matches("^[^><&]+$").WithName("Last name");
                RuleFor(x => x.ContactEmailaddress).Matches("^[^><&]+$").WithName("Email");
                RuleFor(x => x.Description).NotEmpty().Matches("^[^><&]+$");
                RuleFor(x => x.Status).NotEmpty();
            }

        }

        public class Query : IRequest<JobActivityEditViewModel>
        {
            public int StudentId { get; set; }
            public int JobActivityId { get; set; }
        }

        public class QueryHandler : IRequestHandler<Query, JobActivityEditViewModel>
        {
            private readonly ScholarshipForServiceContext _db;
            private readonly ICacheHelper _cache;

            public QueryHandler(ScholarshipForServiceContext db, ICacheHelper cache)
            {
                _db = db;
                _cache = cache;
            }
            
            public async Task<JobActivityEditViewModel> Handle(Query request, CancellationToken cancellationToken)
            {              
                var model = new JobActivityEditViewModel();
                model.AgencyTypeList = new SelectList(await _cache.GetAgencyTypesAsync(), nameof(AgencyType.AgencyTypeId), nameof(AgencyType.Name));
                model.StatusList = new SelectList(await _cache.GetJobActivityStatusAsync(), nameof(JobActivityStatus.JobActivityStatusID), nameof(JobActivityStatus.Name));
                model.JobActivityId = request.JobActivityId;
                var studentData = await _db.Students.Where(m => m.StudentId == request.StudentId)
                    .Include(m => m.StudentInstitutionFundings).ThenInclude(m => m.Institution)
                    .Include(m => m.StudentInstitutionFundings).ThenInclude(m => m.Degree)
                    .Include(m => m.StudentInstitutionFundings).ThenInclude(m => m.Major)
                    .FirstOrDefaultAsync();

                model.Studentname = $"{studentData.FirstName} {studentData.LastName}";
                model.University = studentData.StudentInstitutionFundings.FirstOrDefault().Institution.Name;
                model.Degree = studentData.StudentInstitutionFundings.FirstOrDefault().Degree.Name;
                model.Major = studentData.StudentInstitutionFundings.FirstOrDefault().Major.Name;
                model.GradDate = studentData.StudentInstitutionFundings.FirstOrDefault().ExpectedGradDate.HasValue ? $"{studentData.StudentInstitutionFundings.FirstOrDefault().ExpectedGradDate.Value.Month}/{studentData.StudentInstitutionFundings.FirstOrDefault().ExpectedGradDate.Value.Year}" : "";

                if (request.JobActivityId > 0)
                {
                    var jobActivityData = await _db.StudentJobActivities.Where(m => m.StudentId == request.StudentId && m.StudentJobActivityId == request.JobActivityId)
                        .Include(m => m.JobActivityStatus)
                        .FirstOrDefaultAsync();

                    model.DateApplied = jobActivityData.DateApplied.HasValue ? jobActivityData.DateApplied.Value.ToShortDateString() : "";
                    model.PositionTitle = jobActivityData.PositionTitle;
                    model.USAJControlNunber = jobActivityData.UsajobscontrolNumber;
                    model.AgencyType = jobActivityData.AgencyTypeId.HasValue ? jobActivityData.AgencyTypeId.Value : 0;
                    model.AgencyName = jobActivityData.Agency;
                    model.DutyLocation = jobActivityData.DutyLocation;
                    model.ContactFirstname = jobActivityData.ContactFirstName;
                    model.ContactLastname = jobActivityData.ContactLastName;
                    model.ContactEmailaddress = jobActivityData.ContactEmail;
                    model.Description = jobActivityData.Description;
                    model.Status = jobActivityData.JobActivityStatusID.Value;
                    model.StatusOther = jobActivityData.StatusOther;
                }

                return model;
            }
        }

        public class QuerySelectList : IRequest<JobActivityEditViewModel>
        {
            public JobActivityEditViewModel Model { get; set; }
        }
        public class QuerySelectListHandker : IRequestHandler<QuerySelectList, JobActivityEditViewModel>
        {
            private readonly ICacheHelper _cache;
            public QuerySelectListHandker(ICacheHelper cache)
            {
                _cache = cache;
            }
            public async Task<JobActivityEditViewModel> Handle(QuerySelectList request, CancellationToken cancellationToken)
            {
                request.Model.AgencyTypeList = new SelectList(await _cache.GetAgencyTypesAsync(), nameof(AgencyType.AgencyTypeId), nameof(AgencyType.Name));
                request.Model.StatusList = new SelectList(await _cache.GetJobActivityStatusAsync(), nameof(JobActivityStatus.JobActivityStatusID), nameof(JobActivityStatus.Name));
                return request.Model;
            }
        }

        public class Command : IRequest<bool>
        {
            public int StudentId { get; set; }
            public JobActivityEditViewModel Model { get; set; }
        }

        public class CommandHandler : IRequestHandler<Command, bool>
        {
            private readonly ScholarshipForServiceContext _db;
            private readonly ICacheHelper _cache;
            private readonly IAuditEventLogHelper _auditLogger;

            public CommandHandler(ScholarshipForServiceContext db, ICacheHelper cache, IAuditEventLogHelper auditLogger)
            {
                _db = db;
                _cache = cache;
                _auditLogger = auditLogger;
            }

            public async Task<bool> Handle(Command request, CancellationToken cancellationToken)
            {
                TextInfo textinfo = new CultureInfo("en-US", false).TextInfo;
                if (request.Model.JobActivityId > 0)
                {
                    var editJobActivity = await _db.StudentJobActivities
                        .Where(m => m.StudentId == request.StudentId && m.StudentJobActivityId == request.Model.JobActivityId)
                        .FirstOrDefaultAsync();

                    editJobActivity.DateApplied = !string.IsNullOrWhiteSpace(request.Model.DateApplied) ? Convert.ToDateTime(request.Model.DateApplied) : null;
                    editJobActivity.PositionTitle = request.Model.PositionTitle;
                    editJobActivity.UsajobscontrolNumber = request.Model.USAJControlNunber;
                    editJobActivity.DutyLocation = request.Model.DutyLocation;
                    editJobActivity.Description = request.Model.Description;
                    editJobActivity.StatusOther = request.Model.StatusOther;
                    editJobActivity.JobActivityStatusID = request.Model.Status;
                    editJobActivity.ContactEmail = request.Model.ContactEmailaddress;
                    editJobActivity.ContactFirstName = !string.IsNullOrWhiteSpace(request.Model.ContactFirstname) ? textinfo.ToTitleCase(request.Model.ContactFirstname.ToLower()) : "";
                    editJobActivity.ContactLastName = !string.IsNullOrWhiteSpace(request.Model.ContactLastname) ? textinfo.ToTitleCase(request.Model.ContactLastname.ToLower()) : "";
                    editJobActivity.Agency = request.Model.AgencyName;
                    editJobActivity.AgencyTypeId = request.Model.AgencyType;
                    editJobActivity.ContactEmail = request.Model.ContactEmailaddress;
                    editJobActivity.LastUpdated = DateTime.UtcNow;
                    await _db.SaveChangesAsync();
                }
                else
                {
                    var jobActivityData = await _db.Students
                      .Where(m => m.StudentId == request.StudentId)
                      .Include(m => m.StudentJobActivities)
                      .FirstOrDefaultAsync();

                    var newJobActivity = new Data.StudentJobActivity();
                    newJobActivity.DateApplied = !string.IsNullOrWhiteSpace(request.Model.DateApplied) ? Convert.ToDateTime(request.Model.DateApplied) : null;
                    newJobActivity.PositionTitle = request.Model.PositionTitle;
                    newJobActivity.UsajobscontrolNumber = request.Model.USAJControlNunber;
                    newJobActivity.DutyLocation = request.Model.DutyLocation;
                    newJobActivity.Description = request.Model.Description;
                    newJobActivity.StatusOther = request.Model.StatusOther;
                    newJobActivity.JobActivityStatusID = request.Model.Status;
                    newJobActivity.ContactEmail = request.Model.ContactEmailaddress;
                    newJobActivity.ContactFirstName = !string.IsNullOrWhiteSpace(request.Model.ContactFirstname) ? textinfo.ToTitleCase(request.Model.ContactFirstname.ToLower()) : "";
                    newJobActivity.ContactLastName = !string.IsNullOrWhiteSpace(request.Model.ContactLastname) ? textinfo.ToTitleCase(request.Model.ContactLastname.ToLower()) : "";
                    newJobActivity.Agency = request.Model.AgencyName;
                    newJobActivity.AgencyTypeId = request.Model.AgencyType;
                    newJobActivity.ContactEmail = request.Model.ContactEmailaddress;
                    newJobActivity.LastUpdated = DateTime.UtcNow;
                    jobActivityData.StudentJobActivities.Add(newJobActivity);
                    await _db.SaveChangesAsync();
                }
                await _auditLogger.LogAuditEvent("Student Update Job Activity");
                return true;
            }
        }
    }

  
}
