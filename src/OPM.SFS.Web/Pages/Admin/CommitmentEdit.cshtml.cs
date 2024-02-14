using AutoMapper;
using AutoMapper.QueryableExtensions;
using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OPM.SFS.Core.DTO;
using OPM.SFS.Core.Shared;
using OPM.SFS.Data;
using OPM.SFS.Web.Infrastructure.Extensions;
using OPM.SFS.Web.Mappings;
using OPM.SFS.Web.Models;
using OPM.SFS.Web.Shared;
using OPM.SFS.Web.SharedCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace OPM.SFS.Web.Pages.Admin
{
    [Authorize(Roles = "AD")]
    public class CommitmentEditModel : PageModel
    {

        [BindProperty]
        public CommitmentModelViewModel Data { get; set; }

        [BindProperty(SupportsGet = true)]
        public int AgencyTypeID { get; set; }
        [BindProperty(SupportsGet = true)]
        public int ParentAgencyID { get; set; }
        [FromQuery(Name = "cid")]
        public int CommitId { get; set; } = 0;

        [FromQuery(Name = "sid")]
        public int StudentID { get; set; } = 0;

        private readonly IMediator _mediator;

        public CommitmentEditModel(IMediator mediator) => _mediator = mediator;


        public async Task OnGetAsync()
        {
            Data = await _mediator.Send(new Query() { StudId = StudentID, CommitId = CommitId });
        }

        public JsonResult OnGetAgenciesByType()
        {
            var data = _mediator.Send(new JsonQueryAgencyType() { Id = AgencyTypeID }).Result;
            return new JsonResult(data);
        }

        public JsonResult OnGetSubAgencies()
        {
            var data = _mediator.Send(new JsonQuerySubAgencies() { Id = ParentAgencyID }).Result;
            return new JsonResult(data);
        }

        public JsonResult OnGetCommitmentTypes(string flow)
        {
            var data = _mediator.Send(new JsonQueryCommitmentType() { ApprovalFlow = flow }).Result;
            return new JsonResult(data);
        }

        public async Task<IActionResult> OnPostApproveAsync()
        {
            if (!ModelState.IsValid)
            {
                await _mediator.Send(new QuerySelectList() { Model = Data });
                return Page();
            }

            var validator = new CommitmentEditValidator();
            var results = validator.Validate(Data);
            if (!results.IsValid)
            {
                results.AddToModelState(ModelState, null);
                await _mediator.Send(new QuerySelectList() { Model = Data });
                return Page();
            }

            var submit = await _mediator.Send(new Command() { Model = Data, Id = StudentID, Action = CommitmentProcessConst.Approved, AdminID = User.GetUserId() });
            return Redirect($"/Admin/CommitmentList");
        }

        public async Task<IActionResult> OnPostRejectAsync()
        {
            if (!ModelState.IsValid)
            {
                await _mediator.Send(new QuerySelectList() { Model = Data });
                return Page();
            }
            var commitmentID = await _mediator.Send(new Command() { Model = Data, Id = StudentID, Action = CommitmentProcessConst.Rejected, AdminID = User.GetUserId() });
            return Redirect($"/Admin/CommitmentReject?cid={Data.CommitmentId}");
        }

        public async Task<IActionResult> OnPostDeleteAsync()
        {
            if (!ModelState.IsValid)
            {
                await _mediator.Send(new QuerySelectList() { Model = Data });
                return Page();
            }
            var commitmentID = await _mediator.Send(new Command() { Model = Data, Id = StudentID, Action = "Delete", AdminID = User.GetUserId() });
            return Redirect($"/Admin/CommitmentList");
        }

        public async Task<IActionResult> OnPostOverrideAsync()
        {
            if (!ModelState.IsValid)
            {
                await _mediator.Send(new QuerySelectList() { Model = Data });
                return Page();
            }

            var validator = new CommitmentEditValidator();
            var results = validator.Validate(Data);
            if (!results.IsValid)
            {
                results.AddToModelState(ModelState, null);
                await _mediator.Send(new QuerySelectList() { Model = Data });
                return Page();
            }

            var result = await _mediator.Send(new CommandOverride() { Model = Data, Id = StudentID, AdminID = User.GetUserId() });
            if (result.IsSuccess)
                return Redirect($"/Admin/CommitmentList");
            await _mediator.Send(new QuerySelectList() { Model = Data });
            ModelState.AddModelError("", result.Message);
            return Page();


        }


        public class CommitmentEditValidator : AbstractValidator<CommitmentModelViewModel>
        {
            public CommitmentEditValidator()
            {
                RuleFor(x => x.AgencyType).NotEmpty();
                RuleFor(x => x.Agency).NotEmpty();
                RuleFor(x => x.JobTitle).NotEmpty();
                RuleFor(x => x.JobTitle).Matches("^[^><&]+$");
                RuleFor(x => x.CommitmentType).GreaterThan(0);


                //Approved workflow validations - 
                When(x => x.AgencyApprovalWorkflow == "PREAPPR", () =>
                {
                    RuleFor(x => x.Justification).NotEmpty();
                    RuleFor(x => x.Justification).Matches("^[^><&]+$");
                });

                //Approved workflow validations - 
                When(x => x.AgencyApprovalWorkflow == "APPR", () =>
                {
                    RuleFor(x => x.SalaryMin).NotNull().GreaterThan(0);
                    RuleFor(x => x.SalaryMax).NotNull().GreaterThan(0);
                    RuleFor(x => x.PayPlan).NotEmpty();
                    RuleFor(x => x.Series).NotEmpty();
                    RuleFor(x => x.Grade).NotEmpty();
                    RuleFor(x => x.State).NotNull().GreaterThan(0);
                    RuleFor(x => x.PostalCode).NotEmpty();
                    RuleFor(x => x.PostalCode).Matches("^[^><&]+$");
                    RuleFor(x => x.SupervisorFirstName).NotEmpty();
                    RuleFor(x => x.SupervisorFirstName).Matches("^[^><&]+$");
                    RuleFor(x => x.SupervisorLastName).NotEmpty();
                    RuleFor(x => x.SupervisorLastName).Matches("^[^><&]+$");
                    RuleFor(x => x.SupervisorEmailAddress).NotEmpty();
                    RuleFor(x => x.SupervisorEmailAddress).Matches("^[^><&]+$");
                    RuleFor(x => x.SupervisorPhoneAreaCode).NotEmpty();
                    RuleFor(x => x.SupervisorPhoneAreaCode).Matches("^[^><&]+$");
                    RuleFor(x => x.SupervisorPhonePrefix).NotEmpty();
                    RuleFor(x => x.SupervisorPhonePrefix).Matches("^[^><&]+$");
                    RuleFor(x => x.SupervisorPhoneSuffix).NotEmpty();
                    RuleFor(x => x.SupervisorPhoneSuffix).Matches("^[^><&]+$");
                    RuleFor(x => x.SupervisorPhoneExtension).Matches("^[^><&]+$");
                    RuleFor(x => x.MentorFirstName).Matches("^[^><&]+$");
                    RuleFor(x => x.MentorLastName).Matches("^[^><&]+$");
                    RuleFor(x => x.MentorEmailAddress).Matches("^[^><&]+$");
                    RuleFor(x => x.MentorPhoneAreaCode).Matches("^[^><&]+$");
                    RuleFor(x => x.MentorPhonePrefix).Matches("^[^><&]+$");
                    RuleFor(x => x.MentorPhoneSuffix).Matches("^[^><&]+$");
                    RuleFor(x => x.JobSearchType).NotNull().GreaterThan(0);
                    RuleFor(x => x.StartDateMonth).NotNull();
                    RuleFor(x => x.StartDateMonth).Matches("^[^><&]+$");
                    RuleFor(x => x.StartDateDay).NotNull();
                    RuleFor(x => x.StartDateDay).Matches("^[^><&]+$");
                    RuleFor(x => x.StartDateYear).NotNull();
                    RuleFor(x => x.StartDateYear).Matches("^[^><&]+$");

                    When(x => x.IsInternational, () =>
                    {
                        RuleFor(x => x.Country).NotNull();
                        RuleFor(x => x.Country).Matches("^[^><&]+$");
                    });
                });

            }
        }

        public class Query : IRequest<CommitmentModelViewModel>
        {
            public int StudId { get; set; }
            public int CommitId { get; set; }

        }

        public class QueryHandler : IRequestHandler<Query, CommitmentModelViewModel>
        {
            private readonly ScholarshipForServiceContext _efDB;
            private readonly ICacheHelper _cache;
            private readonly ICryptoHelper _crypto;
            private readonly ICommitmentProcessService _commitHelper;
            private readonly ICommitmentMappingHelper _mapper;



            public QueryHandler(ScholarshipForServiceContext efDB, ICacheHelper cache, ICryptoHelper crypto, ICommitmentProcessService commitHelper, ICommitmentMappingHelper mapper)
            {
                _efDB = efDB;
                _cache = cache;
                _crypto = crypto;
                _commitHelper = commitHelper;
                _mapper = mapper;
            }
            public async Task<CommitmentModelViewModel> Handle(Query request, CancellationToken cancellationToken)
            {


                var GlobalConfigSettings = await _cache.GetGlobalConfigurationsAsync();
                if (request.CommitId > 0)
                {
                    var commitmentTypes = await _cache.GetCommitmentTypeAsync();
                    //Get data from the database and populate the vm
                    var commitmentData = await _efDB.StudentCommitments.Where(m => m.StudentId == request.StudId && m.StudentCommitmentId == request.CommitId)
                        .ProjectTo<StudentCommitmentDTO>(_mapper.GetConfigurationForDTO()).FirstOrDefaultAsync();
                    var vm = await _mapper.PopulateViewModelAsync(commitmentData);
                    var ssn = _crypto.Decrypt(commitmentData.Ssn, GlobalConfigSettings);
                    vm.FormattedSSN = $"xxx-xx-{ssn.Substring(ssn.Length - 4)}";
                    vm.CommitmentTypeList = new SelectList(commitmentTypes, nameof(CommitmentType.CommitmentTypeId), nameof(CommitmentType.Name));
					return vm;
                }

                else
                {
                    CommitmentModelViewModel model = new CommitmentModelViewModel();
                    model.AgencyTypeList = new SelectList(await _cache.GetAgencyTypesAsync(), nameof(AgencyType.AgencyTypeId), nameof(AgencyType.Name));
                    model.PayRateList = new SelectList(await _cache.GetPayRateListAsync(), nameof(SalaryType.SalaryTypeId), nameof(SalaryType.Name));
                    model.JobSearchTypeList = new SelectList(await _cache.GetJobSearchTypeAsync(), nameof(JobSearchType.JobSearchTypeID), nameof(JobSearchType.Name));
                    model.StateList = new SelectList(await _cache.GetStatesAsync(), nameof(State.StateId), nameof(State.Name));
                    model.CommitmentTypeList = new SelectList(await _cache.GetCommitmentTypeAsync(), nameof(CommitmentType.CommitmentTypeId), nameof(CommitmentType.Name));
                    var studentData = await _efDB.Students.Where(m => m.StudentId == request.StudId)
                        .AsNoTracking()
                        .Select(s => new
                        {
                            Firstname = s.FirstName,
                            Lastname = s.LastName,
                            SSN = s.Ssn
                        }).FirstOrDefaultAsync();


                    model.FirstName = studentData.Firstname;
                    model.LastName = studentData.Lastname;
                    var ssn = _crypto.Decrypt(studentData.SSN, GlobalConfigSettings);
                    model.FormattedSSN = $"xxx-xx-{ssn.Substring(ssn.Length - 4)}";
                    return model;
                }


            }
        }

        public class JsonQueryAgencyType : IRequest<IEnumerable<CommitmentModelViewModel.AgencyListVM>>
        {
            public int Id { get; set; }
        }

        public class JsonQueryAgencyTypeHandler : IRequestHandler<JsonQueryAgencyType, IEnumerable<CommitmentModelViewModel.AgencyListVM>>
        {
            //Get Agency Types
            private readonly ScholarshipForServiceContext _efDB;
            private readonly ICacheHelper _cache;
            public JsonQueryAgencyTypeHandler(ScholarshipForServiceContext efDB, ICacheHelper cache)
            {
                _efDB = efDB;
                _cache = cache;
            }
            public async Task<IEnumerable<CommitmentModelViewModel.AgencyListVM>> Handle(JsonQueryAgencyType request, CancellationToken cancellationToken)
            {
                List<CommitmentModelViewModel.AgencyListVM> data = new();
                var agencies = await _cache.GetAgenciesAsync();
                var agencyFiltered = agencies.Where(m => m.AgencyTypeId == request.Id)
                                             .Select(m => new { m.AgencyId, m.Name, Workflow = m.Workflow })
                                             .OrderBy(m => m.Name)
                                             .ToList();

                foreach (var agency in agencyFiltered)
                {
                    data.Add(new CommitmentModelViewModel.AgencyListVM()
                    {
                        Id = agency.AgencyId,
                        Name = agency.Name,
                        ApprFlow = agency.Workflow
                    });
                }
                return data;
            }
        }

        public class JsonQuerySubAgencies : IRequest<IEnumerable<CommitmentModelViewModel.AgencyListVM>>
        {
            public int Id { get; set; }
        }

        public class JsonQuerySubAgencyHandler : IRequestHandler<JsonQuerySubAgencies, IEnumerable<CommitmentModelViewModel.AgencyListVM>>
        {
            //Get Sub-Agencies
            private readonly ScholarshipForServiceContext _efDB;
            private readonly ICacheHelper _cache;
            public JsonQuerySubAgencyHandler(ScholarshipForServiceContext efDB, ICacheHelper cache)
            {
                _efDB = efDB;
                _cache = cache;
            }

            public async Task<IEnumerable<CommitmentModelViewModel.AgencyListVM>> Handle(JsonQuerySubAgencies request, CancellationToken cancellationToken)
            {
                List<CommitmentModelViewModel.AgencyListVM> data = new();
                var agencies = await _cache.GetAgenciesAsync();
                var agenciesFiltered = agencies.Where(m => m.ParentAgencyId == request.Id)
                                             .Select(m => new { m.AgencyId, m.Name, Workflow = m.Workflow })
                                             .OrderBy(m => m.Name)
                                             .ToList();

                foreach (var agency in agenciesFiltered)
                {
                    data.Add(new CommitmentModelViewModel.AgencyListVM()
                    {
                        Id = agency.AgencyId,
                        Name = agency.Name,
                        ApprFlow = agency.Workflow
                    });
                }
                return data;
            }
        }

        public class JsonQueryCommitmentType : IRequest<IEnumerable<CommitmentModelViewModel.CommitmentTypeVM>>
        {
            public string ApprovalFlow { get; set; }
        }

        public class JsonQueryCommitmentTypeHandler : IRequestHandler<JsonQueryCommitmentType, IEnumerable<CommitmentModelViewModel.CommitmentTypeVM>>
        {
            private readonly ICacheHelper _cache;
            public JsonQueryCommitmentTypeHandler(ICacheHelper cache)
            {
                _cache = cache;
            }
            public async Task<IEnumerable<CommitmentModelViewModel.CommitmentTypeVM>> Handle(JsonQueryCommitmentType request, CancellationToken cancellationToken)
            {
                var commitmentTypes = await _cache.GetCommitmentTypeAsync();
                List<CommitmentModelViewModel.CommitmentTypeVM> data = new();
                foreach (var type in commitmentTypes)
                {
                    data.Add(new CommitmentModelViewModel.CommitmentTypeVM()
                    {
                        Id = type.CommitmentTypeId,
                        Name = type.Name
                    });
                }
                return data;
            }
        }

        public class Command : IRequest<bool>
        {
            public int Id { get; set; }
            public string Action { get; set; }
            public CommitmentModelViewModel Model { get; set; }
            public int AdminID { get; set; }
        }

        public class CommandHandler : IRequestHandler<Command, bool>
        {
            private readonly ScholarshipForServiceContext _efDB;
            private readonly ICommitmentNotificationService _emailer;
            public readonly ICommitmentProcessService _commitHelper;
            private readonly ICacheHelper _cache;
            private readonly IStudentDashboardService _service;

            public CommandHandler(ScholarshipForServiceContext efDB, ICommitmentNotificationService emailer, ICommitmentProcessService commitHelper, ICacheHelper cache, IStudentDashboardService service)
            {
                _efDB = efDB;
                _emailer = emailer;
                _commitHelper = commitHelper;
                _cache = cache;
                _service = service;
            }

            public async Task<bool> Handle(Command request, CancellationToken cancellationToken)
            {

                var theCommitment = await _efDB.StudentCommitments
                    .Where(m => m.StudentCommitmentId == request.Model.CommitmentId).Include(m => m.SupervisorContact)
                    .Include(m => m.Student).ThenInclude(m => m.StudentInstitutionFundings).ThenInclude(m => m.Institution)
                    .Include(m => m.Agency).ThenInclude(m => m.AgencyType)
					.Include(m => m.Agency).ThenInclude(m => m.CommitmentApprovalWorkflow)
					.Include(m => m.CommitmentType)
                    .Include(m => m.CommitmentStatus)
                    .FirstOrDefaultAsync();
                if (request.Action is not "Delete")
                {
                    theCommitment.LastUpdatedByAdminID = request.AdminID;
                    theCommitment.LastModified = DateTime.UtcNow;

                    CommitmentProcessService.NextStatusData nextStatusInfo;
                    if (request.Action == CommitmentProcessConst.Rejected)
                    {
                        nextStatusInfo = await _commitHelper.GetNextStatusForRejectAsync();
                    }
                    else
                    {
                        nextStatusInfo = await _commitHelper.GetNextStatusForApprovalAsync(theCommitment.CommitmentStatus.Code, theCommitment.Agency.CommitmentApprovalWorkflow.Code, isAdminApproval: true);
                    }

                    theCommitment.CommitmentStatusId = nextStatusInfo.StatusID;
                    if (request.Action == CommitmentProcessConst.Approved)
                    {
                        theCommitment.DateApproved = DateTime.UtcNow;
                    }

                    CommitmentNotificationRequest emailData = new()
                    {
                        Commitment = theCommitment,
                        InstitutionID = theCommitment.Student.StudentInstitutionFundings.FirstOrDefault().InstitutionId.Value
                    };

                    if (nextStatusInfo.StatusCode == CommitmentProcessConst.Approved)
                    {
                        await _emailer.SendEmailWhenPOApprovesAsync(emailData);

                    }
                    else if (nextStatusInfo.StatusCode == CommitmentProcessConst.RequestFinalDocs)
                        await _emailer.SendEmailWhenPORequestFinalDocsAsync(emailData);
                    else if (nextStatusInfo.StatusCode == CommitmentProcessConst.Rejected)
                        await _emailer.SendEmailWhenPORejectsAsync(emailData);
                    await _efDB.SaveChangesAsync();
					await _service.UpdateCommitmentReportedForDashboard(request.Model.StudentID);
					return true;
                }

                theCommitment.IsDeleted = true;
                theCommitment.LastModified = DateTime.UtcNow;
                theCommitment.LastUpdatedByAdminID = request.AdminID;
                await _efDB.SaveChangesAsync();
                await _service.UpdateCommitmentReportedForDashboard(request.Model.StudentID);
                return true;


            }
        }

        public class CommandOverride : IRequest<CommandResult>
        {
            public int Id { get; set; }
            public int AdminID { get; set; }
            public CommitmentModelViewModel Model { get; set; }

        }

        public class CommandResult
        {
            public bool IsSuccess { get; set; }
            public string Message { get; set; }
            public int ID { get; set; }
        }

        public class CommandOverrideHandler : IRequestHandler<CommandOverride, CommandResult>
        {
            private readonly ScholarshipForServiceContext _efDB;
            private readonly ICacheHelper _cache;
            private readonly ICommitmentProcessService _commitUpdate;
            private readonly IStudentDashboardService _service;
            public CommandOverrideHandler(ScholarshipForServiceContext efDB, ICacheHelper cache, ICommitmentProcessService commitUpdate, IStudentDashboardService service)
            {
                _efDB = efDB;
                _cache = cache;
                _commitUpdate = commitUpdate;
                _service = service;
            }

            public async Task<CommandResult> Handle(CommandOverride request, CancellationToken cancellationToken)
            {

                var result = await _commitUpdate.StudentUpdateCommitmentAsync(request.Model);
                var commitmentType = await _service.GetCommimentType(request.Model.CommitmentType);
                await _service.UpdateCommitmentReportedForDashboard(request.Model.StudentID);
                return new CommandResult() { IsSuccess = result.IsSuccess, Message = result.Message, ID = result.ID };
            }

        }


        public class QuerySelectList : IRequest<CommitmentModelViewModel>
        {
            public CommitmentModelViewModel Model { get; set; }
        }

        public class QuerySelectListHandler : IRequestHandler<QuerySelectList, CommitmentModelViewModel>
        {
            private readonly ScholarshipForServiceContext _efDB;
            public QuerySelectListHandler(ScholarshipForServiceContext efDB)
            {
                _efDB = efDB;
            }
            public async Task<CommitmentModelViewModel> Handle(QuerySelectList request, CancellationToken cancellationToken)
            {
                request.Model.AgencyTypeList = new SelectList(await _efDB.AgencyTypes.OrderBy(m => m.Name).AsNoTracking().ToListAsync(), nameof(AgencyType.AgencyTypeId), nameof(AgencyType.Name));
                request.Model.CommitmentTypeList = new SelectList(await _efDB.CommitmentTypes.OrderBy(m => m.Name).AsNoTracking().ToListAsync(), nameof(CommitmentType.CommitmentTypeId), nameof(CommitmentType.Name));
                request.Model.PayRateList = new SelectList(await _efDB.SalaryTypes.OrderBy(m => m.Name).AsNoTracking().ToListAsync(), nameof(SalaryType.SalaryTypeId), nameof(SalaryType.Name));
                request.Model.JobSearchTypeList = new SelectList(await _efDB.JobSearchTypes.OrderBy(m => m.Name).AsNoTracking().ToListAsync(), nameof(JobSearchType.JobSearchTypeID), nameof(JobSearchType.Name));
                request.Model.StateList = new SelectList(await _efDB.States.OrderBy(m => m.Name).AsNoTracking().ToListAsync(), nameof(State.StateId), nameof(State.Name));
                if (request.Model.Agency > 0)
                {
                    request.Model.AgencyList = new SelectList(await _efDB.Agencies.OrderBy(m => m.Name).AsNoTracking().Where(m => m.AgencyTypeId == request.Model.AgencyType).ToListAsync(), nameof(OPM.SFS.Data.Agency.AgencyId), nameof(OPM.SFS.Data.Agency.Name));
                }
                if (request.Model.SubAgency > 0)
                {
                    request.Model.SubAgencyList = new SelectList(await _efDB.Agencies.OrderBy(m => m.Name).AsNoTracking().Where(m => m.ParentAgencyId == request.Model.Agency).ToListAsync(), nameof(OPM.SFS.Data.Agency.AgencyId), nameof(OPM.SFS.Data.Agency.Name));

                }
                return request.Model;
            }
        }

    }
}
