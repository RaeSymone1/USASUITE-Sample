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
    public class AgencyEditModel : PageModel
    {
        [BindProperty]
        public AdminAgencyEditViewModel Data { get; set; }
        [FromQuery(Name = "aid")]
        public int AgencyID { get; set; } = 0;

        private readonly IMediator _mediator;

        public AgencyEditModel(IMediator mediator) => _mediator = mediator;

        public async Task OnGetAsync()
        {
            Data = await _mediator.Send(new Query() { AgencyID  = AgencyID });
        }

        public async Task<IActionResult> OnPostSaveAsync()
        {
            if (!ModelState.IsValid)
            {
                //await _mediator.Send(new QuerySelectList() { Model = Data });
                return Page();
            }
            var save = await _mediator.Send(new CommandSave() { Model = Data });
            return Redirect($"/Admin/AgencyList");
        }

        public async Task<IActionResult> OnPostDeleteAsync()
        {           
            var delete = await _mediator.Send(new CommandDelete() { Model = Data });
            return Redirect($"/Admin/AgencyList");
        }


        public class AdminAgencyEditViewModelValidator : AbstractValidator<AdminAgencyEditViewModel>
        {
            public AdminAgencyEditViewModelValidator()
            {
                RuleFor(x => x.AgencyType).NotNull().NotEqual(0);
                RuleFor(x => x.CommitmentApprovalWorkflow).NotNull().NotEqual(0);
                RuleFor(x => x.AgencyName).NotEmpty().Matches("^[^><&]+$").WithName("Agency");
                RuleFor(x => x.City).NotEmpty().Matches("^[^><&]+$").WithMessage("City is required.");
                RuleFor(x => x.County).Matches("^[^><&]+$");
                RuleFor(x => x.Country).Matches("^[^><&]+$");
            }
          
        }

        public class Query : IRequest<AdminAgencyEditViewModel>
        {
            public int AgencyID { get; set; }
        }

        public class QueryHandler : IRequestHandler<Query, AdminAgencyEditViewModel>
        {
            private readonly ScholarshipForServiceContext _db;
            private readonly ICacheHelper _cache;

            public QueryHandler(ScholarshipForServiceContext db, ICacheHelper cache )
            {
                _db = db;
                _cache = cache;
            }
            public async Task<AdminAgencyEditViewModel> Handle(Query request, CancellationToken cancellationToken)
            {
                var allAgencies = await _cache.GetAgenciesAsync();
                var allAgencyTypes = await _cache.GetAgencyTypesAsync();
                var fedExecType = allAgencyTypes.Where(m => m.Code == "FederalExec").FirstOrDefault().AgencyTypeId;
                AdminAgencyEditViewModel model = new();
                model.StateList = new SelectList(await _cache.GetStatesAsync(), nameof(State.StateId), nameof(State.Name));
                model.AgencyTypeList = new SelectList(allAgencyTypes, nameof(AgencyType.AgencyTypeId), nameof(AgencyType.Name));                
                var parentAgencies = allAgencies.Where(m => m.AgencyTypeId == fedExecType);
                model.ParentAgencyList = new SelectList(parentAgencies, nameof(OPM.SFS.Data.Agency.AgencyId), nameof(OPM.SFS.Data.Agency.Name));
                model.ApprovalProcessList = new SelectList(await _cache.GetCommitmentApprovalWorkflowsAsync(), nameof(OPM.SFS.Data.CommitmentApprovalWorkflow.CommitmentApprovalWorkflowId), nameof(OPM.SFS.Data.CommitmentApprovalWorkflow.Code));
                
                if (request.AgencyID > 0)
                {
                    var agencyData = await _db.Agencies
                        .Where(m => m.AgencyId == request.AgencyID)
                        .Select(m => new
                        {
                            AgencyTypeID = m.AgencyTypeId,
                            AgencyID = m.AgencyId,
                            Name = m.Name,
                            ParentAgencyID = m.ParentAgencyId,
                            AddressCity = m.Address != null ? m.Address.City : "",
                            AddressState = m.Address != null ? m.Address.StateId : 0,
                            AddressCountry = m.Address != null ? m.Address.Country : "",
                            CommitWorkFlow = m.CommitmentApprovalWorkflowId
                        }).FirstOrDefaultAsync();                       

                    model.AgencyType = agencyData.AgencyTypeID.Value;
                    model.AgencyID = request.AgencyID;
                    model.AgencyName = agencyData.Name;
                    model.CommitmentApprovalWorkflow = agencyData.CommitWorkFlow.HasValue ? agencyData.CommitWorkFlow.Value : 0;
                    if (agencyData.ParentAgencyID.HasValue && agencyData.ParentAgencyID.Value > 0)
                    {
                        model.ParentAgency = agencyData.ParentAgencyID.Value;
                    }
                   
                    model.City = agencyData.AddressCity;
                    model.State = agencyData.AddressState;                      
                    model.Country = agencyData.AddressCountry;
                    
                    if(agencyData.CommitWorkFlow.HasValue && agencyData.CommitWorkFlow > 0)
                    {
                        model.CommitmentApprovalWorkflow = agencyData.CommitWorkFlow.Value;
                    }

                }

                return model;
            }
        }

        public class CommandSave : IRequest<bool>
        {
            public AdminAgencyEditViewModel Model { get; set; }
        }

        public class CommandSaveHandler : IRequestHandler<CommandSave, bool>
        {
            private readonly ScholarshipForServiceContext _db;
            private readonly ICacheHelper _cache;

            public CommandSaveHandler(ScholarshipForServiceContext db, ICacheHelper cache)
            {
                _db = db;
                _cache = cache;
            }

            public async Task<bool> Handle(CommandSave request, CancellationToken cancellationToken)
            {
                if (request.Model.AgencyID == 0)
                {
                    OPM.SFS.Data.Agency addAgency = new();
                    addAgency.Name = request.Model.AgencyName;
                    addAgency.ParentAgencyId = request.Model.ParentAgency;
                    addAgency.AgencyTypeId = request.Model.AgencyType;
                    addAgency.CommitmentApprovalWorkflowId = request.Model.CommitmentApprovalWorkflow;
                    addAgency.IsDisabled = false;
                    if (!string.IsNullOrWhiteSpace(request.Model.City) || !string.IsNullOrWhiteSpace(request.Model.Country)
                            || request.Model.State > 0)
                    {
                        addAgency.Address = new();
                        addAgency.Address.LineOne = "";
                        if (!string.IsNullOrWhiteSpace(request.Model.City))
                            addAgency.Address.City = request.Model.City;
                        if(!string.IsNullOrWhiteSpace(request.Model.Country))
                            addAgency.Address.Country = request.Model.Country;
                        if (request.Model.State > 0)
                            addAgency.Address.StateId = request.Model.State;
                    }
                    _db.Agencies.Add(addAgency);
                }
                else
                {
                    var agencyData = await _db.Agencies
                        .Include(m => m.Address).ThenInclude(m => m.State)
                        .FirstOrDefaultAsync(m => m.AgencyId == request.Model.AgencyID);

                    agencyData.Name = request.Model.AgencyName;
                    agencyData.AgencyTypeId = request.Model.AgencyType;
                    agencyData.ParentAgencyId = request.Model.ParentAgency;
                    agencyData.LastModified = DateTime.UtcNow;
                    agencyData.CommitmentApprovalWorkflowId = request.Model.CommitmentApprovalWorkflow;
                    if (agencyData.Address is not null)
                    {
                        agencyData.Address.LineOne = "";
                        agencyData.Address.City = request.Model.City;
                        agencyData.Address.Country = request.Model.Country;
                        agencyData.Address.StateId = request.Model.State;
                    }
                    else
                    {
                        if (!string.IsNullOrWhiteSpace(request.Model.City) || !string.IsNullOrWhiteSpace(request.Model.Country)
                            || request.Model.State > 0)
                        {
                            agencyData.Address = new Address()
                            {
                                LineOne = "",
                                City = request.Model.City,
                                Country = request.Model.Country,
                                StateId = request.Model.State
                            };
                        }
                    }
                }
                
                await _db.SaveChangesAsync();
                return true;
            }
        }

        public class CommandDelete : IRequest<bool>
        {
            public AdminAgencyEditViewModel Model { get; set; }
        }

        public class CommandDeleteHandler : IRequestHandler<CommandDelete, bool>
        {
            private readonly ScholarshipForServiceContext _db;
            private readonly ICacheHelper _cache;


            public CommandDeleteHandler(ScholarshipForServiceContext db, ICacheHelper cache)
            {
                _db = db;
                _cache = cache;
            }

            public async Task<bool> Handle(CommandDelete request, CancellationToken cancellationToken)
            {
               if(request.Model.AgencyID > 0 && request.Model.ParentAgency == 0)
                {
                    //TODO: Prevent parent agencies from being deleted until confirm with PMO
                    var data = await _db.Agencies.FirstOrDefaultAsync(m => m.AgencyId == request.Model.AgencyID);
                    data.IsDisabled = true;
                    await _db.SaveChangesAsync();
                    return true;
                }
                return false;
            }
        }
    }
}
