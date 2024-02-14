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
using OPM.SFS.Core.Shared;
using OPM.SFS.Data;
using OPM.SFS.Web.Infrastructure.Extensions;
using OPM.SFS.Web.Models;
using OPM.SFS.Web.Shared;
using OPM.SFS.Web.SharedCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace OPM.SFS.Web.Pages.Admin
{
    [Authorize(Roles = "AD")]
    public class CommitmentAddModel : PageModel
    {
        [FromQuery(Name = "sid")]
        public int StudentID { get; set; } = 0;

        [BindProperty]
        public CommitmentModelViewModel Data { get; set; }

        private readonly IMediator _mediator;

        public CommitmentAddModel(IMediator mediator) => _mediator = mediator;


        public async Task OnGetAsync()
        {
            Data = await _mediator.Send(new Query() { StudId = StudentID });
        }

        public JsonResult OnGetAgenciesByType(int AgencyTypeID)
        {
            var data = _mediator.Send(new JsonQueryAgencyType() { Id = AgencyTypeID }).Result;
            return new JsonResult(data);
        }

        public JsonResult OnGetAgenciesByState(int stateID, int typeID)
        {
            var data = _mediator.Send(new JsonQueryAgencyByState() { StateID = stateID, TypeID = typeID }).Result;
            return new JsonResult(data);
        }

        public JsonResult OnGetSubAgencies(int ParentAgencyID)
        {
            var data = _mediator.Send(new JsonQuerySubAgencies() { Id = ParentAgencyID }).Result;
            return new JsonResult(data);
        }

        public async Task<IActionResult> OnPostSubmitAsync()
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
            var result = await _mediator.Send(new Command() { Model = Data, Id = User.GetUserId() });
            if (result.IsSuccess)
                return Redirect($"/Admin/CommitmentDocuments?sid={StudentID}&cid={result.CommitmentID}");
            await _mediator.Send(new QuerySelectList() { Model = Data });
            ModelState.AddModelError("", result.ErrorMessage);
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
                When(x => x.ShowForm == CommitmentProcessConst.CommitmentApprovalTentative, () =>
                {
                    RuleFor(x => x.Justification).NotEmpty();
                    RuleFor(x => x.Justification).Matches("^[^><&]+$");
                    RuleFor(x => x.Justification).MaximumLength(1000);
                });

                //Approved workflow validations - 
                When(x => x.ShowForm == CommitmentProcessConst.CommitmentApprovalFinal, () =>
                {
                    RuleFor(x => x.SalaryMin).NotNull().GreaterThan(0);
                    RuleFor(x => x.SalaryMax).NotNull().GreaterThan(0);
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

        }
        public class QueryHandler : IRequestHandler<Query, CommitmentModelViewModel>
        {
            private readonly ScholarshipForServiceContext _efDB;
            private readonly ICacheHelper _cache;
            private readonly ICryptoHelper _crypto;

            public QueryHandler(ScholarshipForServiceContext efDB, ICacheHelper cache, ICryptoHelper crypto, ICommitmentProcessService commitHelper)
            {
                _efDB = efDB;
                _cache = cache;
                _crypto = crypto;
            }
            public async Task<CommitmentModelViewModel> Handle(Query request, CancellationToken cancellationToken)
            {
                CommitmentModelViewModel model = new CommitmentModelViewModel();
                var GlobalConfigSettings = await _cache.GetGlobalConfigurationsAsync();
                //populate the select lists
                model.AgencyTypeList = new SelectList(await _cache.GetAgencyTypesAsync(), nameof(AgencyType.AgencyTypeId), nameof(AgencyType.Name));
                model.PayRateList = new SelectList(await _cache.GetPayRateListAsync(), nameof(SalaryType.SalaryTypeId), nameof(SalaryType.Name));
                model.JobSearchTypeList = new SelectList(await _cache.GetJobSearchTypeAsync(), nameof(JobSearchType.JobSearchTypeID), nameof(JobSearchType.Name));
                model.StateList = new SelectList(await _cache.GetStatesAsync(), nameof(State.StateId), nameof(State.Name));
                model.ShowForm = CommitmentProcessConst.CommitmentApprovalTentative;
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
                if (!string.IsNullOrWhiteSpace(studentData.SSN))
                {
					var ssn = _crypto.Decrypt(studentData.SSN, GlobalConfigSettings);
					model.FormattedSSN = $"xxx-xx-{ssn.Substring(ssn.Length - 4)}";
				}
                else
                {
                    model.FormattedSSN = "xxx-xx-0000";
                }
               
                model.StudentID = request.StudId;
                return model;
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

        public class JsonQueryAgencyByState : IRequest<IEnumerable<CommitmentModelViewModel.AgencyListVM>>
        {
            public int StateID { get; set; }
            public int TypeID { get; set; }
        }

        public class JsonQueryAgencyByStateHandler : IRequestHandler<JsonQueryAgencyByState, IEnumerable<CommitmentModelViewModel.AgencyListVM>>
        {
            //Get Agencies by State
            private readonly ScholarshipForServiceContext _efDB;
            private readonly ICacheHelper _cache;
            public JsonQueryAgencyByStateHandler(ScholarshipForServiceContext efDB, ICacheHelper cache)
            {
                _efDB = efDB;
                _cache = cache;
            }
            public async Task<IEnumerable<CommitmentModelViewModel.AgencyListVM>> Handle(JsonQueryAgencyByState request, CancellationToken cancellationToken)
            {
                List<CommitmentModelViewModel.AgencyListVM> data = new();
                var agencies = await _cache.GetAgenciesAsync();
                var agencyFiltered = agencies.Where(m => m.StateID == request.StateID && m.AgencyTypeId == request.TypeID)
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

        public JsonResult OnGetCommitmentTypes(string flow)
        {
            var data = _mediator.Send(new JsonQueryCommitmentType() { ApprovalFlow = flow }).Result;
            return new JsonResult(data);
        }



        public class QuerySelectList : IRequest<CommitmentModelViewModel>
        {
            public CommitmentModelViewModel Model { get; set; }
        }

        public class QuerySelectListHandler : IRequestHandler<QuerySelectList, CommitmentModelViewModel>
        {
            private readonly ScholarshipForServiceContext _efDB;
            private readonly ICacheHelper _cache;
            private readonly ICryptoHelper _crypto;
            public QuerySelectListHandler(ScholarshipForServiceContext efDB, ICacheHelper cache, ICryptoHelper crypto)
            {
                _efDB = efDB;
                _cache = cache;
                _crypto = crypto;
            }
            public async Task<CommitmentModelViewModel> Handle(QuerySelectList request, CancellationToken cancellationToken)
            {
                var GlobalConfigSettings = await _cache.GetGlobalConfigurationsAsync();
                request.Model.AgencyTypeList = new SelectList(await _cache.GetAgencyTypesAsync(), nameof(AgencyType.AgencyTypeId), nameof(AgencyType.Name));
                request.Model.CommitmentTypeList = new SelectList(await _cache.GetCommitmentTypeAsync(), nameof(CommitmentType.CommitmentTypeId), nameof(CommitmentType.Name));
                request.Model.PayRateList = new SelectList(await _cache.GetPayRateListAsync(), nameof(SalaryType.SalaryTypeId), nameof(SalaryType.Name));
                request.Model.JobSearchTypeList = new SelectList(await _cache.GetJobSearchTypeAsync(), nameof(JobSearchType.JobSearchTypeID), nameof(JobSearchType.Name));
                request.Model.StateList = new SelectList(await _cache.GetStatesAsync(), nameof(State.StateId), nameof(State.Name));
                if (request.Model.Agency > 0)
                {
                    request.Model.AgencyList = new SelectList(await _efDB.Agencies.OrderBy(m => m.Name).AsNoTracking().Where(m => m.AgencyTypeId == request.Model.AgencyType).ToListAsync(), nameof(OPM.SFS.Data.Agency.AgencyId), nameof(OPM.SFS.Data.Agency.Name));
                }
                if (request.Model.SubAgency > 0)
                {
                    request.Model.SubAgencyList = new SelectList(await _efDB.Agencies.OrderBy(m => m.Name).AsNoTracking().Where(m => m.ParentAgencyId == request.Model.Agency).ToListAsync(), nameof(OPM.SFS.Data.Agency.AgencyId), nameof(OPM.SFS.Data.Agency.Name));

                }
                if (request.Model.StudentID > 0)
                {
                    var studentData = await _efDB.Students.Where(m => m.StudentId == request.Model.StudentID)
                        .Select(m => new
                        {
                            SSN = m.Ssn,
                            LastName = m.LastName,
                            FirstName = m.FirstName
                        }).FirstOrDefaultAsync();
                    request.Model.FirstName = studentData.FirstName;
                    request.Model.LastName = studentData.LastName;

                    var ssn = _crypto.Decrypt(studentData.SSN, GlobalConfigSettings);
                    request.Model.FormattedSSN = $"xxx-xx-{ssn.Substring(ssn.Length - 4)}";
                }
                return request.Model;
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
        public class CommandResult
        {
            public bool IsSuccess { get; set; }
            public string ErrorMessage { get; set; }
            public int CommitmentID { get; set; }
        }
        public class Command : IRequest<CommandResult>
        {
            public int Id { get; set; }
            public CommitmentModelViewModel Model { get; set; }
            public int AdminID { get; set; }
        }
        public class CommandHandler : IRequestHandler<Command, CommandResult>
        {
            private readonly ScholarshipForServiceContext _efDB;
            private readonly ICommitmentProcessService _commitUpdate;
            private readonly ICacheHelper _cache;

            public CommandHandler(ScholarshipForServiceContext efDB, ICommitmentProcessService commitUpdate, ICacheHelper cache)
            {
                _efDB = efDB;
                _commitUpdate = commitUpdate;
                _cache = cache;
            }

            public async Task<CommandResult> Handle(Command request, CancellationToken cancellationToken)
            {
                request.Model.ShowForm = request.Model.AgencyApprovalWorkflow;
                var AgencyTypes = await _cache.GetAgencyTypesAsync();
                var agencyTypename = AgencyTypes.Where(m => m.AgencyTypeId == request.Model.AgencyType).FirstOrDefault().Name;
                if (agencyTypename == "Federal Executive")
                {
                    if (request.Model.PayPlan == null || request.Model.Series == null || request.Model.Grade == null)
                    {
                        return new CommandResult() { IsSuccess = false, CommitmentID = request.Model.CommitmentId, ErrorMessage = "Pay Plan, Series and Grade required!" };
                    }
                }
                var result = await _commitUpdate.StudentAddNewCommitmentAsync(request.Model);
                return new CommandResult() { IsSuccess = result.IsSuccess, CommitmentID = result.ID, ErrorMessage = result.Message };
            }
        }
    }
}
