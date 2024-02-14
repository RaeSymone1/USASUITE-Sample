using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OPM.SFS.Data;
using OPM.SFS.Web.Models;
using OPM.SFS.Web.Shared;
using OPM.SFS.Web.SharedCode;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OPM.SFS.Web.Pages.Admin
{
    [Authorize(Roles = "AD")]
    public class InstitutionEditModel : PageModel
    {
        [BindProperty]
        public InstitutionEditVM Data { get; set; }

        [FromQuery(Name = "iid")]
        public int InstitutionID { get; set; } = 0;
        public string IsSuccess { get; set; }

        private readonly IMediator _mediator;

        public InstitutionEditModel(IMediator mediator) => _mediator = mediator;

        public async Task OnGetAsync()
        {
            Data = await _mediator.Send(new Query() { ID = InstitutionID });
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {

                return Page();
            }
           var save = await _mediator.Send(new Command() { Model = Data });
            Data = await _mediator.Send(new Query() { ID = save.InstitutionID });
            if (save.IsSuccess == true)
            {
                Data.ShowSuccessMessage = true;
                Data.SuccessMessage = save.ErrorMessage;
            }
            return Page();
        }

        public class InstitutionEditVMValidator : AbstractValidator<InstitutionEditVM>
        {
            public InstitutionEditVMValidator()
            {
                RuleFor(x => x.Institution).NotEmpty().Matches("^[^><]+$");
                RuleFor(x => x.Type).NotNull().NotEqual(0);
                RuleFor(x => x.City).NotEmpty().Matches("^[^><&]+$");
                RuleFor(x => x.State).NotNull().NotEqual(0);
                RuleFor(x => x.PostalCode).NotEmpty().Matches("^[^><&]+$");
                RuleFor(x => x.ProgramPage).Matches("^[^><&]+$");
                RuleFor(x => x.HomePage).Matches("^[^><&]+$");
                RuleFor(x => x.GrantNumber).GreaterThan(0);
                RuleFor(x => x.GrantExpiration).Must(IsValidDate)
                    .WithMessage("Grant Expiration is invalid date format.");
            }

            protected bool IsValidDate(string date)
            {
                if (string.IsNullOrWhiteSpace(date)) return true;
                if (DateTime.TryParse(date, out _)) return true;
                return false;
            }

        }

        public class Query : IRequest<InstitutionEditVM>
        {
            public int ID { get; set; }
        }

        public class QueryHandler : IRequestHandler<Query, InstitutionEditVM>
        {
            private readonly ScholarshipForServiceContext _db;
            private readonly ICacheHelper _cache;

            public QueryHandler(ScholarshipForServiceContext db, ICacheHelper cache)
            {
                _db = db;
                _cache = cache;
            }

            public async Task<InstitutionEditVM> Handle(Query request, CancellationToken cancellationToken)
            {
                InstitutionEditVM model = new();
                var allInstitution = await _cache.GetInstitutionsAsync();
                model.InstitutionID = request.ID;
                model.StateList = new SelectList(await _cache.GetStatesAsync(), nameof(State.StateId), nameof(State.Name));
                model.TypeList = new SelectList(await _cache.GetInsitutionTypeAsync (), nameof(InstitutionType.InstitutionTypeId), nameof(InstitutionType.Name));
                model.ParentInstititionList = new SelectList(await _cache.GetInstitutionsAsync(), nameof(Institution.InstitutionId), nameof(Institution.Name));
                model.AcademicScheduleList = new SelectList(await _cache.GetAcademicSchedulesAsync(), nameof(AcademicSchedule.AcademicScheduleId), nameof(AcademicSchedule.Name));

                if (request.ID > 0)
                {
                    var institutionData = await _db.Institutions.Include(m =>m.InstitutionType).Where(m => m.InstitutionId == request.ID)
                        .AsNoTracking()
                        .FirstOrDefaultAsync();

                    model.Institution = institutionData.Name.Trim();
                    model.ParentInstitution = institutionData.ParentInstitutionID;
                    model.City = institutionData.City;
                    model.State = institutionData.StateId.Value;
                    model.PostalCode = institutionData.PostalCode;
                    model.HomePage = institutionData.HomePage;
                    model.ProgramPage = institutionData.ProgramPage;
                    model.Type = institutionData.InstitutionTypeId.HasValue ? institutionData.InstitutionTypeId.Value : 0;
                    model.AcademicSchedule = institutionData.AcademicScheduleId.HasValue ? institutionData.AcademicScheduleId.Value : 0;
                    model.IsActive = institutionData.IsActive;
                    model.IsAcceptingApplications = institutionData.IsAcceptingApplications;
                    if (institutionData.ParentInstitutionID.HasValue && institutionData.ParentInstitutionID > 0)
                    {
                        var x = allInstitution.Where(m => m.InstitutionId == model.ParentInstitution).FirstOrDefault();
                        model.GrantNumber = x.GrantNumber;
                        model.GrantExpiration = x.GrantExpirationDate.HasValue ? x.GrantExpirationDate.Value.ToShortDateString() : "";
                       
                    }
                    else
                    {
                        model.GrantExpiration = institutionData.GrantExpirationDate.HasValue ? institutionData.GrantExpirationDate.Value.ToShortDateString() : "";
                        model.GrantNumber = institutionData.GrantNumber.HasValue ? institutionData.GrantNumber.Value : null;
                    }

                }
                return model;
            }
        }

        public class Command : IRequest<CommandResult>
        {
            public InstitutionEditVM Model { get; set; }
        }

        public class CommandHandler : IRequestHandler<Command, CommandResult>
        {
            private readonly ScholarshipForServiceContext _db;
            private readonly ICacheHelper _cache;
            private readonly IStudentDashboardService _service;

            public CommandHandler(ScholarshipForServiceContext db, ICacheHelper cache, IStudentDashboardService service)
            {
                _db = db;
                _cache = cache;
                _service = service;
            }

            public async Task<CommandResult> Handle(Command request, CancellationToken cancellationToken)
            {
                               
                if (request.Model.InstitutionID > 0)
                {
                    var data = await _db.Institutions.
                        Where(m => m.InstitutionId == request.Model.InstitutionID)
                        .FirstOrDefaultAsync();
                    data.Name = request.Model.Institution;
                    data.InstitutionTypeId = request.Model.Type;
                    data.ParentInstitutionID = request.Model.ParentInstitution > 0 ? request.Model.ParentInstitution : null;
                    data.City = request.Model.City;
                    data.StateId = request.Model.State;
                    data.PostalCode = request.Model.PostalCode;
                    data.HomePage = request.Model.HomePage;
                    data.ProgramPage = request.Model.ProgramPage;
                    
                   
                    data.IsAcceptingApplications = request.Model.IsAcceptingApplications;
                    data.IsActive = request.Model.IsActive;
                    if (data.ParentInstitutionID.HasValue && data.ParentInstitutionID > 0)
                    {
                        data.GrantNumber = null;
                        data.GrantExpirationDate = null;
                    }
                    else
                    {
                        data.GrantNumber = request.Model.GrantNumber.HasValue ? request.Model.GrantNumber : null;
                        data.GrantExpirationDate = !string.IsNullOrWhiteSpace(request.Model.GrantExpiration) ? Convert.ToDateTime(request.Model.GrantExpiration) : null;
                    }
					
					if (!data.AcademicScheduleId.Equals(request.Model.AcademicSchedule))
					{
						//Any changes to the academic schedule needs to update the corresponding student's total terms and service owed
						var studentsWithTermsOverride = await _db.StudentInstitutionFundings.Where(m => m.InstitutionId == request.Model.InstitutionID)
					        .Where(m => m.TotalAcademicTerms != null).ToListAsync();
                        var academicScheduleList = await _cache.GetAcademicSchedulesAsync();

                        foreach(var student in studentsWithTermsOverride)
                        {
							string academicSchedule = academicScheduleList.Where(m => m.AcademicScheduleId == request.Model.AcademicSchedule).Select(m => m.Name).FirstOrDefault();
							string totalTerms = _service.CalculateTotalTerms(student.EnrolledYear.ToString(), student.EnrolledSession, student.FundingEndYear.ToString(), student.FundingEndSession, academicSchedule);
							var strServiceOwed = _service.GetServiceOwed(academicSchedule, totalTerms, student.EnrolledYear.ToString(), student.EnrolledSession, student.FundingEndYear.ToString(), student.FundingEndSession);
							double servieOwedValue;
							var isDouble = double.TryParse(strServiceOwed, out servieOwedValue);
							student.ServiceOwed = servieOwedValue > 0 ? servieOwedValue : null;
                            student.TotalAcademicTerms = totalTerms;
                        }
					}
					data.AcademicScheduleId = request.Model.AcademicSchedule;
					await _db.SaveChangesAsync();
                    return new CommandResult() { IsSuccess = true, ErrorMessage = "Institution record updated successfully." };
                }
                else
                {
                    Institution newInstitution = new();
                    newInstitution.Name = request.Model.Institution;
                    newInstitution.InstitutionTypeId = request.Model.Type;
                    newInstitution.ParentInstitutionID = request.Model.ParentInstitution > 0 ? request.Model.ParentInstitution : null;
                    newInstitution.City = request.Model.City;
                    newInstitution.StateId = request.Model.State;
                    newInstitution.PostalCode = request.Model.PostalCode;
                    newInstitution.HomePage = request.Model.HomePage;
                    newInstitution.ProgramPage = request.Model.ProgramPage;
                    newInstitution.AcademicScheduleId = request.Model.AcademicSchedule;
                    if (newInstitution.ParentInstitutionID.HasValue && newInstitution.ParentInstitutionID > 0)
                    {
                        newInstitution.GrantNumber = null;
                        newInstitution.GrantExpirationDate = null;
                    }
                    else
                    {
                        newInstitution.GrantNumber = request.Model.GrantNumber.HasValue ? request.Model.GrantNumber : null;
                        newInstitution.GrantExpirationDate = !string.IsNullOrWhiteSpace(request.Model.GrantExpiration) ? Convert.ToDateTime(request.Model.GrantExpiration) : null;
                    }
                        _db.Institutions.Add(newInstitution);
                    await _db.SaveChangesAsync();
					return new CommandResult() { IsSuccess = true, InstitutionID = newInstitution.InstitutionId,  ErrorMessage = "Institution record added successfully." };
                }
                
            }
        }


        public class CommandResult
        {
            public bool IsSuccess { get; set; }
            public string ErrorMessage { get; set; }
            public int InstitutionID { get; set; }
        }
    }
}
