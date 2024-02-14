using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OPM.SFS.Data;
using OPM.SFS.Web.Models;
using OPM.SFS.Web.SharedCode;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OPM.SFS.Web.Pages.Admin
{
    [Authorize(Roles = "AD")]
    public class StudentJobActivityEditModel : PageModel
    {
        [BindProperty]
        public AdminJobActivityEditViewModel Data { get; set; }

        [FromQuery(Name = "jid")]
        public int JobActivityID { get; set; } 

        [FromQuery(Name = "sid")]
        public int StudentID { get; set; } 

        private readonly IMediator _mediator;

        public StudentJobActivityEditModel(IMediator mediator) => _mediator = mediator;


        public async Task OnGetAsync()
        {
            Data = await _mediator.Send(new Query() { JobActivityId = JobActivityID, StudentID = StudentID });
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            var response = await _mediator.Send(new Command() { Model = Data });
            return RedirectToPage("StudentJobActivity", new { sid = Data.StudentID });
        }

        public class AdminJobActivityEditValidator : AbstractValidator<AdminJobActivityEditViewModel>
        {
            public AdminJobActivityEditValidator()
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

        public class Query : IRequest<AdminJobActivityEditViewModel>
        {
            public int StudentID { get; set; }
            public int JobActivityId { get; set; }
        }

        public class QueryHandler : IRequestHandler<Query, AdminJobActivityEditViewModel>
        {
            private readonly ScholarshipForServiceContext _db;
            private readonly ICacheHelper _cache;

            public QueryHandler(ScholarshipForServiceContext db, ICacheHelper cache)
            {
                _db = db;
                _cache = cache;
            }

            public async Task<AdminJobActivityEditViewModel> Handle(Query request, CancellationToken cancellationToken)
            {
                var model = new AdminJobActivityEditViewModel();
                model.AgencyTypeList = new SelectList(await _cache.GetAgencyTypesAsync(), nameof(AgencyType.AgencyTypeId), nameof(AgencyType.Name));
                model.StudentID = request.StudentID;
                model.JobActivityId = request.JobActivityId;
                model.StatusList = new SelectList(await _cache.GetJobActivityStatusAsync(), nameof(JobActivityStatus.JobActivityStatusID), nameof(JobActivityStatus.Name));

                var studentData = await _db.Students.Where(m => m.StudentId == request.StudentID)
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
                    var jobActivityData = await _db.StudentJobActivities.Where(m => m.StudentId == request.StudentID && m.StudentJobActivityId == request.JobActivityId)
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

        public class Command : IRequest<bool>
        {           
            public AdminJobActivityEditViewModel Model { get; set; }
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
               
                    var editJobActivity = await _db.StudentJobActivities
                        .Where(m => m.StudentId == request.Model.StudentID && m.StudentJobActivityId == request.Model.JobActivityId)
                        .FirstOrDefaultAsync();

                    editJobActivity.DateApplied = !string.IsNullOrWhiteSpace(request.Model.DateApplied) ? Convert.ToDateTime(request.Model.DateApplied) : null;
                    editJobActivity.PositionTitle = request.Model.PositionTitle;
                    editJobActivity.UsajobscontrolNumber = request.Model.USAJControlNunber;
                    editJobActivity.DutyLocation = request.Model.DutyLocation;
                    editJobActivity.Description = request.Model.Description;
                    editJobActivity.StatusOther = request.Model.StatusOther;
                    editJobActivity.ContactEmail = request.Model.ContactEmailaddress;
                    editJobActivity.ContactFirstName = request.Model.ContactFirstname;
                    editJobActivity.ContactLastName = request.Model.ContactLastname;
                    editJobActivity.Agency = request.Model.AgencyName;
                    editJobActivity.AgencyTypeId = request.Model.AgencyType;
                    editJobActivity.ContactEmail = request.Model.ContactEmailaddress;
                    editJobActivity.LastUpdated = DateTime.UtcNow;
                    editJobActivity.JobActivityStatusID = request.Model.Status;
                await _auditLogger.LogAuditEvent("Admin Update Job Activity");
                await _db.SaveChangesAsync();
                
               

                return true;
            }
        }


      
    }
}
