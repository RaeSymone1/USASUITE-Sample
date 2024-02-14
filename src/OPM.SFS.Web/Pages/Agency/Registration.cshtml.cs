using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OPM.SFS.Core.Shared;
using OPM.SFS.Data;
using OPM.SFS.Web.Models.Agency;
using OPM.SFS.Web.Shared;
using OPM.SFS.Web.SharedCode;

namespace OPM.SFS.Web.Pages.Agency
{
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public class RegistrationModel : PageModel
    {
        [BindProperty]
        public AgencyOfficialRegistrationViewModel Data { get; set; }

        [BindProperty(SupportsGet = true)]
        public int AgencyTypeID { get; set; }
        [BindProperty(SupportsGet = true)]
        public int ParentAgencyID { get; set; }

        private readonly IMediator _mediator;

        public RegistrationModel(IMediator mediator) => _mediator = mediator;

        public async Task OnGetAsync()
        {
            Data = await _mediator.Send(new Query());
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            var result = await _mediator.Send(new Command() { Model = Data, });
            if (result.IsSuccess == false)
            {
                ModelState.AddModelError("", result.Message);
                await _mediator.Send(new QuerySelectList() { Model = Data });
                return Page();
            }
            return Redirect("/Agency/RegistrationConfirm");
        }

        public JsonResult OnGetAgenciesByType()
        {
            var data = _mediator.Send(new JsonQueryAgencyType() { Id = AgencyTypeID }).Result;
            return new JsonResult(data);
        }

        public JsonResult OnGetAgenciesByState(int stateID, int typeID)
        {
            var data = _mediator.Send(new JsonQueryAgencyByState() { StateID = stateID, TypeID = typeID }).Result;
            return new JsonResult(data);
        }

        public JsonResult OnGetSubAgencies()
        {
            var data = _mediator.Send(new JsonQuerySubAgencies() { Id = ParentAgencyID }).Result;
            return new JsonResult(data);
        }

        public class AgencyOfficialRegistrationValidator : AbstractValidator<AgencyOfficialRegistrationViewModel>
        {
            public AgencyOfficialRegistrationValidator()
            {
                RuleFor(x => x.AgencyType).NotEmpty().GreaterThan(0);
                RuleFor(x => x.Agency).NotEmpty().GreaterThan(0);
                RuleFor(x => x.FirstName).NotEmpty().Matches("^[^><&]+$").WithName("First name");
                RuleFor(x => x.LastName).NotEmpty().Matches("^[^><&]+$").WithName("Last name");
                RuleFor(x => x.Role).NotEmpty().GreaterThan(0);
                RuleFor(x => x.Address1).NotEmpty().Matches("^[^><&]+$");
                RuleFor(x => x.Address2).Matches("^[^><&]+$");
                RuleFor(x => x.City).NotEmpty().Matches("^[^><&]+$");
                RuleFor(x => x.State).NotEmpty().GreaterThan(0);
                RuleFor(x => x.PostalCode).NotEmpty().Matches("^[^><&]+$").WithName("Postal Code");
                RuleFor(x => x.Phone).NotEmpty().Matches("^[^><&]+$");
                RuleFor(x => x.Extension).Matches("^[^><&]+$");
                RuleFor(x => x.Fax).Matches("^[^><&]+$");
                RuleFor(x => x.Email).NotEmpty().Matches("^[^><&]+$");              
            }
        }

        public class Query : IRequest<AgencyOfficialRegistrationViewModel>
		{

		}

		public class QueryHandler : IRequestHandler<Query, AgencyOfficialRegistrationViewModel>
		{
            private readonly ICacheHelper _cache;
            private readonly ScholarshipForServiceContext _efDB;

            public QueryHandler(ICacheHelper cache, ScholarshipForServiceContext db )
            {
                _cache = cache;
                _efDB = db;
            }

            public async Task<AgencyOfficialRegistrationViewModel> Handle(Query request, CancellationToken cancellationToken)
			{
                AgencyOfficialRegistrationViewModel model = new();
                model.StateList = new SelectList(await _cache.GetStatesAsync(), nameof(State.StateId), nameof(State.Name));
                model.RoleList = new SelectList(await _cache.GetAgencyUserRolesAsync(), nameof(AgencyUserRole.AgencyUserRoleID), nameof(AgencyUserRole.Role));
                model.AgencyTypeList = new SelectList(await _cache.GetAgencyTypesAsync(), nameof(AgencyType.AgencyTypeId), nameof(AgencyType.Name));
                

                return model;
			}
		}

        public class QuerySelectList : IRequest<AgencyOfficialRegistrationViewModel>
        {
            public AgencyOfficialRegistrationViewModel Model { get; set; }
        }

        public class QuerySelectListHandler : IRequestHandler<QuerySelectList, AgencyOfficialRegistrationViewModel>
        {
            private readonly ICacheHelper _cache;
            public QuerySelectListHandler(ICacheHelper cache)
            {
                _cache = cache;
            }

            public async Task<AgencyOfficialRegistrationViewModel> Handle(QuerySelectList request, CancellationToken cancellationToken)
            {
                request.Model.StateList = new SelectList(await _cache.GetStatesAsync(), nameof(State.StateId), nameof(State.Name));
                request.Model.RoleList = new SelectList(await _cache.GetAgencyUserRolesAsync(), nameof(AgencyUserRole.AgencyUserRoleID), nameof(AgencyUserRole.Role));
                request.Model.AgencyTypeList = new SelectList(await _cache.GetAgencyTypesAsync(), nameof(AgencyType.AgencyTypeId), nameof(AgencyType.Name));
                var agencies = await _cache.GetAgenciesAsync();
                if (request.Model.Agency > 0)
                {
                    request.Model.AgencyList = new SelectList(agencies.OrderBy(m => m.Name).Where(m => m.AgencyTypeId == request.Model.AgencyType), nameof(OPM.SFS.Data.Agency.AgencyId), nameof(OPM.SFS.Data.Agency.Name));
                }
                if (request.Model.SubAgency > 0)
                {
                    request.Model.SubAgencyList = new SelectList(agencies.OrderBy(m => m.Name).Where(m => m.ParentAgencyId == request.Model.Agency), nameof(OPM.SFS.Data.Agency.AgencyId), nameof(OPM.SFS.Data.Agency.Name));

                }
                return request.Model;
            }
        }

        public class Command : IRequest<CommnandHandler.CommandResult>
        {
            public AgencyOfficialRegistrationViewModel Model { get; set; }
        }

        public class CommnandHandler : IRequestHandler<Command, CommnandHandler.CommandResult>
        {
            private readonly ScholarshipForServiceContext _efDB;
            private readonly ICacheHelper _cache;
            private readonly ICryptoHelper _crypto;
            private readonly IEmailerService _emailer;

            public CommnandHandler(ScholarshipForServiceContext efDB, ICacheHelper cache, ICryptoHelper crypto, IEmailerService emailer)
            {
                _efDB = efDB;
                _cache = cache;
                _crypto = crypto;
                _emailer = emailer;
            }

            public async Task<CommandResult> Handle(Command request, CancellationToken cancellationToken)
            {
                var allAccountStatus = await _cache.GetProfileStatusAsync();
                var pendingID = allAccountStatus.Where(m => m.Name == "Pending").Select(m => m.ProfileStatusID).FirstOrDefault();
                TextInfo textinfo = new CultureInfo("en-US", false).TextInfo;



                var validDomains = await _efDB.AgencyTypes.Where(m => m.AgencyTypeId == request.Model.AgencyType).Select(m => m.ValidEmailDomain).FirstOrDefaultAsync();
                int i = request.Model.Email.LastIndexOf(".");
                int len = request.Model.Email.Length;
                var emailDomain = request.Model.Email.Substring(i, len-i);

                if (!validDomains.Contains(emailDomain.ToLower()))
                {
                    return new CommandResult() { IsSuccess = false, Message = "Email address is invalid." };
                }

                var checkEmailData = await _efDB.AgencyUsers.Where(m => m.Email == request.Model.Email)
                   .Select(m => new
                   {
                       ID = m.AgencyUserId
                   }).FirstOrDefaultAsync();
                if(checkEmailData != null && checkEmailData.ID > 0)
                {
                    return new CommandResult() { IsSuccess = false, Message = "An account with this User ID already exists." };
                }

                AgencyUser newUser = new();
              
                newUser.Firstname = textinfo.ToTitleCase(request.Model.FirstName.ToLower());
                newUser.Lastname = textinfo.ToTitleCase(request.Model.LastName.ToLower());
                newUser.AgencyUserRoleID = request.Model.Role;
                if (request.Model.SubAgency > 0)
                    newUser.AgencyID = request.Model.SubAgency;
                else
                    newUser.AgencyID = request.Model.Agency;
                newUser.DisplayContactInfo = request.Model.DisplayPermission == "ContactYes" ? true : false;
                newUser.Address = new Address()
                {
                    LineOne = request.Model.Address1,
                    LineTwo = request.Model.Address2,
                    City = request.Model.City,
                    StateId = request.Model.State,
                    PostalCode = request.Model.PostalCode,
                    PhoneNumber = request.Model.Phone,
                    PhoneExtension = request.Model.Extension,
                    Fax = request.Model.Fax
                };
                newUser.UserName = request.Model.Email;
                newUser.Email = request.Model.Email;
                newUser.WebsiteUrl = request.Model.Website;
                newUser.ProfileStatusID = pendingID;

                _efDB.AgencyUsers.Add(newUser);
                await _efDB.SaveChangesAsync();

                string emailContent = $@"Hello Admins, <br /><br /> An Agency Official Registration has been submitted.";
                await _emailer.SendEmailNoTemplateAsync("sfs@opm.gov", $"SFS Agency Official Registration: {request.Model.LastName}, {request.Model.FirstName}", emailContent);


                return new CommandResult() { IsSuccess = true};
                

            }

            public class CommandResult
            {
                public bool IsSuccess { get; set; }
                public string Message { get; set; }
            }
        }

        public class JsonQueryAgencyType : IRequest<IEnumerable<AgencyOfficialRegistrationViewModel.AgencyListDTO>>
        {
            public int Id { get; set; }
        }

        public class JsonQueryAgencyTypeHandler : IRequestHandler<JsonQueryAgencyType, IEnumerable<AgencyOfficialRegistrationViewModel.AgencyListDTO>>
        {
            //Get Agency Types
            private readonly ScholarshipForServiceContext _efDB;
            private readonly ICacheHelper _cache;
            public JsonQueryAgencyTypeHandler(ScholarshipForServiceContext efDB, ICacheHelper cache)
            {
                _efDB = efDB;
                _cache = cache;
            }
            public async Task<IEnumerable<AgencyOfficialRegistrationViewModel.AgencyListDTO>> Handle(JsonQueryAgencyType request, CancellationToken cancellationToken)
            {
                List<AgencyOfficialRegistrationViewModel.AgencyListDTO> data = new();
                var agencies = await _cache.GetAgenciesAsync();
                var agencyFiltered = agencies.Where(m => m.AgencyTypeId == request.Id )
                                             .Select(m => new { m.AgencyId, m.Name})
                                             .OrderBy(m => m.Name)
                                             .ToList();

                foreach (var agency in agencyFiltered)
                {
                    data.Add(new AgencyOfficialRegistrationViewModel.AgencyListDTO()
                    {
                        Id = agency.AgencyId,
                        Name = agency.Name                        
                    });
                }
                return data;
            }
        }

        public class JsonQueryAgencyByState : IRequest<IEnumerable<AgencyOfficialRegistrationViewModel.AgencyListDTO>>
        {
            public int StateID { get; set; }
            public int TypeID { get; set; }
        }

        public class JsonQueryAgencyByStateHandler : IRequestHandler<JsonQueryAgencyByState, IEnumerable<AgencyOfficialRegistrationViewModel.AgencyListDTO>>
        {
            //Get Agencies by State
            private readonly ScholarshipForServiceContext _efDB;
            private readonly ICacheHelper _cache;
            public JsonQueryAgencyByStateHandler(ScholarshipForServiceContext efDB, ICacheHelper cache)
            {
                _efDB = efDB;
                _cache = cache;
            }
            public async Task<IEnumerable<AgencyOfficialRegistrationViewModel.AgencyListDTO>> Handle(JsonQueryAgencyByState request, CancellationToken cancellationToken)
            {
                List<AgencyOfficialRegistrationViewModel.AgencyListDTO> data = new();
                var agencies = await _cache.GetAgenciesAsync();
                var agencyFiltered = agencies.Where(m => m.StateID == request.StateID && m.AgencyTypeId == request.TypeID)
                                             .Select(m => new { m.AgencyId, m.Name })
                                             .OrderBy(m => m.Name)
                                             .ToList();

                foreach (var agency in agencyFiltered)
                {
                    data.Add(new AgencyOfficialRegistrationViewModel.AgencyListDTO()
                    {
                        Id = agency.AgencyId,
                        Name = agency.Name
                    });
                }
                return data;
            }
        }

        public class JsonQuerySubAgencies : IRequest<IEnumerable<AgencyOfficialRegistrationViewModel.AgencyListDTO>>
        {
            public int Id { get; set; }
        }

        public class JsonQuerySubAgencyHandler : IRequestHandler<JsonQuerySubAgencies, IEnumerable<AgencyOfficialRegistrationViewModel.AgencyListDTO>>
        {
            //Get Sub-Agencies
            private readonly ScholarshipForServiceContext _efDB;
            private readonly ICacheHelper _cache;
            public JsonQuerySubAgencyHandler(ScholarshipForServiceContext efDB, ICacheHelper cache)
            {
                _efDB = efDB;
                _cache = cache;
            }

            public async Task<IEnumerable<AgencyOfficialRegistrationViewModel.AgencyListDTO>> Handle(JsonQuerySubAgencies request, CancellationToken cancellationToken)
            {
                List<AgencyOfficialRegistrationViewModel.AgencyListDTO> data = new();
                var agencies = await _cache.GetAgenciesAsync();
                var agenciesFiltered = agencies.Where(m => m.ParentAgencyId == request.Id)
                                             .Select(m => new { m.AgencyId, m.Name })
                                             .OrderBy(m => m.Name)
                                             .ToList();

                foreach (var agency in agenciesFiltered)
                {
                    data.Add(new AgencyOfficialRegistrationViewModel.AgencyListDTO()
                    {
                        Id = agency.AgencyId,
                        Name = agency.Name
                    });
                }
                return data;
            }
        }
    }

  
}
