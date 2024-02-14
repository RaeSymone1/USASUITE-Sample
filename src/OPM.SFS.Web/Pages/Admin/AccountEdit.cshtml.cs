using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using OPM.SFS.Data;
using OPM.SFS.Web.Models;
using OPM.SFS.Web.SharedCode;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OPM.SFS.Web.Pages.Admin
{
    [Authorize(Roles = "AD")]
    public class AccountAdminModel : PageModel
    {
        [BindProperty]
        public AdminEditUserViewModel Data { get; set; }

        [FromQuery(Name = "aid")]
        public int AccountID { get; set; } = 0;

        [FromQuery(Name = "t")]
        public string Type { get; set; }

        private readonly IMediator _mediator;

        public AccountAdminModel(IMediator mediator) => _mediator = mediator;


        public async Task OnGetAsync()
        {
            //TODO: wire up autocomplete for agencies and institutions
            Data = await _mediator.Send(new Query() { ID = AccountID, Type = Type });
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
               
                return Page();
            }
            var save = await _mediator.Send(new Command() { Model = Data });
            return Redirect($"/Admin/AccountList");
        }

        public JsonResult OnGetAgenciesByType(int id)
        {
            var data = _mediator.Send(new JsonQueryAgencyType() { Id = id }).Result;
            return new JsonResult(data);
        }

        public JsonResult OnGetSubAgencies(int id)
        {
            var data = _mediator.Send(new JsonQuerySubAgencies() { Id = id }).Result;
            return new JsonResult(data);
        }

        public JsonResult OnGetAgenciesByState(int stateID, int typeID)
        {
            var data = _mediator.Send(new JsonQueryAgencyByState() { StateID = stateID, TypeID = typeID }).Result;
            return new JsonResult(data);
        }

        public class AdminAccountEditViewModelValidator : AbstractValidator<AdminEditUserViewModel>
        {
            public AdminAccountEditViewModelValidator()
            {
                
                RuleFor(x => x.Username).NotEmpty().Matches("^[^><&]+$");
                RuleFor(x => x.Firstname).NotEmpty().Matches("^[^><&]+$");
                RuleFor(x => x.Lastname).NotEmpty().NotEmpty().Matches("^[^><&]+$");
                RuleFor(x => x.Phone).NotEmpty().Matches("^[^><&]+$");
                RuleFor(x => x.PhoneExt).Matches("^[^><&]+$");
                RuleFor(x => x.Fax).Matches("^[^><&]+$");
                When(x => x.AccountType == "AO", () =>
               {
                   RuleFor(x => x.Agency).NotEmpty();
               });

                When(x => x.AccountType == "PI", () =>
                {
                    RuleFor(x => x.Institution).NotEmpty();
                });

            }

        }

        public class Query : IRequest<AdminEditUserViewModel>
        {
            public string Type { get; set; }
            public int ID { get; set; }
        }

        public class QueryHandler : IRequestHandler<Query, AdminEditUserViewModel>
        {
            private readonly ScholarshipForServiceContext _db;
            private readonly ICacheHelper _cache;

            public QueryHandler(ScholarshipForServiceContext db, ICacheHelper cache) 
            { 
                _db = db;
                _cache = cache;
            }
           
            public async Task<AdminEditUserViewModel> Handle(Query request, CancellationToken cancellationToken)
            {
                AdminEditUserViewModel model = new();
                model.AccountType = request.Type;
                if(request.Type == "AO")
                {
                    var data = await _db.AgencyUsers.AsNoTracking()
                        .Include(m => m.Address)
                        .Include(m => m.Agency)
                        .Include(m => m.ProfileStatus)
                        .Where(m => m.AgencyUserId == request.ID)
                        .Select(x => new
                        {
                            ID = x.AgencyUserId,
                            Username = x.UserName,
                            Email = x.Email,
                            Firstname = x.Firstname,
                            Lastname = x.Lastname,
                            Phone = x.Address.PhoneNumber,
                            PhoneExt = x.Address.PhoneExtension,
                            Fax = x.Address.Fax,
                            ProfileStatusID = x.ProfileStatusID,
                            ProfileStatus = x.ProfileStatus.Name,
                            Agency = x.AgencyID,
                            ParentAgencyID = x.Agency.ParentAgencyId,
                            AgencyTypeID = x.Agency.AgencyTypeId,                          

                        })
                        .FirstOrDefaultAsync();
                        
                        
                    if(data is not null)
                    {
                        var agencies = await _cache.GetAgenciesAsync();
                        model.AgencyTypeList = new SelectList(await _cache.GetAgencyTypesAsync(), nameof(AgencyType.AgencyTypeId), nameof(AgencyType.Name));
                        model.StateList = new SelectList(await _cache.GetStatesAsync(), nameof(State.StateId), nameof(State.Name));
                        if (data.ParentAgencyID.HasValue && data.ParentAgencyID.Value > 0)
                        {
                            //populate the agency lists

                            model.AgencyList = new SelectList(agencies.OrderBy(m => m.Name).Where(m => m.AgencyTypeId == data.AgencyTypeID).ToList(), nameof(OPM.SFS.Data.Agency.AgencyId), nameof(OPM.SFS.Data.Agency.Name));

                            model.SubAgencyList = new SelectList(agencies.Where(m => m.ParentAgencyId == data.ParentAgencyID.Value)
                                                            .OrderBy(m => m.Name).ToList(), nameof(OPM.SFS.Data.Agency.AgencyId), nameof(OPM.SFS.Data.Agency.Name));


                            model.Agency = agencies.Where(m => m.AgencyId == data.ParentAgencyID.Value).Select(m => m.AgencyId).FirstOrDefault();
                            model.SubAgency = data.Agency;
                        }
                        else
                        {
                            var filteredAgencies = agencies.Where(m => m.AgencyTypeId == data.AgencyTypeID).ToList();
                            model.AgencyList = new SelectList(filteredAgencies.OrderBy(m => m.Name).ToList(), nameof(OPM.SFS.Data.Agency.AgencyId), nameof(OPM.SFS.Data.Agency.Name));
                            model.Agency = data.Agency.Value;
                        }
                        model.AgencyType = data.AgencyTypeID;
                        model.ID = data.ID;                        
                        model.Username = data.Username;
                        model.Email = data.Email;
                        model.Firstname = data.Firstname;
                        model.Lastname = data.Lastname;
                        model.Phone = data.Phone;
                        model.PhoneExt = data.PhoneExt;
                        model.Fax = data.Fax;
                        model.ProfileStatusID = data.ProfileStatusID;
                        model.ProfileStatusList = new SelectList(await _cache.GetProfileStatusAsync(), nameof(ProfileStatus.ProfileStatusID), nameof(ProfileStatus.Name));
                      
                    }
                }
                else
                {
                    var data = await _db.AcademiaUsers.AsNoTracking()
                       .Include(m => m.Address)
                       .Include(m => m.Institution)
                       .Where(m => m.AcademiaUserId == request.ID)
                       .Select(x => new
                       {
                           ID = x.AcademiaUserId,
                           Username = x.UserName,
                           Email = x.Email,
                           Firstname = x.Firstname,
                           Lastname = x.Lastname,
                           Phone = x.Address.PhoneNumber,
                           PhoneExt = x.Address.PhoneExtension,
                           Fax = x.Address.Fax,
                           Institition = x.InstitutionID,
                           ProfileStatus = x.ProfileStatus.Name,
                           ProfileStatusID = x.ProfileStatusID

                       })
                       .FirstOrDefaultAsync();

                    if (data is not null)
                    {
                        model.ID = data.ID;
                        model.Institution = data.Institition;
                        model.Username = data.Username;
                        model.Email = data.Email;
                        model.Firstname = data.Firstname;
                        model.Lastname = data.Lastname;
                        model.Phone = data.Phone;
                        model.PhoneExt = data.PhoneExt;
                        model.Fax = data.Fax;
                        model.InstitutionList = new SelectList(await _cache.GetInstitutionsAsync(), nameof(Institution.InstitutionId), nameof(Institution.Name));
                        model.ProfileStatusID = data.ProfileStatusID;
                        model.ProfileStatusList = new SelectList(await _cache.GetProfileStatusAsync(), nameof(ProfileStatus.ProfileStatusID), nameof(ProfileStatus.Name)); 
                    }
                }


                return model;


            }
        }

        public class Command : IRequest<bool>
        {
            public AdminEditUserViewModel Model { get; set; }
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
                var agencies = await _cache.GetAgenciesAsync();
                var institutions = await _cache.GetInstitutionsAsync();
                TextInfo textinfo = new CultureInfo("en-US", false).TextInfo;
                if (request.Model.AccountType == "AO")
                {
                    var toUpdate = await _db.AgencyUsers.Where(m => m.AgencyUserId == request.Model.ID)
                        .Include(m => m.Address)
                        .FirstOrDefaultAsync();
                    toUpdate.AgencyID = request.Model.SubAgency > 0 ? request.Model.SubAgency : request.Model.Agency; 
                    toUpdate.UserName = request.Model.Username;
                    toUpdate.Email = request.Model.Email;
                    toUpdate.Firstname = textinfo.ToTitleCase(request.Model.Firstname.ToLower());
                    toUpdate.Lastname = textinfo.ToTitleCase(request.Model.Lastname.ToLower());
                    toUpdate.Address.PhoneNumber = request.Model.Phone;
                    toUpdate.Address.PhoneExtension = request.Model.PhoneExt;
                    toUpdate.Address.Fax = request.Model.Fax;
                    toUpdate.ProfileStatusID = request.Model.ProfileStatusID;

                    await _db.SaveChangesAsync();
                }
                else
                {
                    var toUpdate = await _db.AcademiaUsers.Where(m => m.AcademiaUserId == request.Model.ID)
                        .Include(m => m.Address)
                        .FirstOrDefaultAsync();
                    toUpdate.InstitutionID = request.Model.Institution;
                    toUpdate.UserName = request.Model.Username;
                    toUpdate.Email = request.Model.Email;
                    toUpdate.Firstname = textinfo.ToTitleCase(request.Model.Firstname.ToLower());
                    toUpdate.Lastname = textinfo.ToTitleCase(request.Model.Lastname.ToLower());
                    toUpdate.Address.PhoneNumber = request.Model.Phone;
                    toUpdate.Address.PhoneExtension = request.Model.PhoneExt;
                    toUpdate.Address.Fax = request.Model.Fax;
                    toUpdate.ProfileStatusID = request.Model.ProfileStatusID;
                    await _db.SaveChangesAsync();
                }
                await _auditLogger.LogAuditEvent($"Admin Update {request.Model.AccountType} Profile");
                return true;
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
    }
}
