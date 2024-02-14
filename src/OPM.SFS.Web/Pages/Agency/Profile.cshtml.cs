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
using Microsoft.Extensions.Configuration;
using OPM.SFS.Data;
using OPM.SFS.Web.Infrastructure.Extensions;
using OPM.SFS.Web.Models.Agency;
using OPM.SFS.Web.SharedCode;

namespace OPM.SFS.Web.Pages.Agency
{
    [Authorize(Roles = "AO")]
    public class ProfileModel : PageModel
    {
        [FromQuery(Name = "s")]
        public string IsSuccess { get; set; }

        [BindProperty]
        public AgencyOfficialProfileViewModel Data { get; set; }

        private readonly IMediator _mediator;

        public ProfileModel(IMediator mediator) => _mediator = mediator;

        public async Task OnGetAsync()
        {
            Data = await _mediator.Send(new Query() { UserID = User.GetUserId() });
            if (!string.IsNullOrWhiteSpace(IsSuccess) && IsSuccess == "true") Data.ShowSuccessMessage = true;
        }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            var response = await _mediator.Send(new Command() { Model = Data, UserID = User.GetUserId() });
            return RedirectToPage("Profile", new { s = "true" });
        }

        public class ProfileValidator : AbstractValidator<AgencyOfficialProfileViewModel>
        {
            public ProfileValidator()
            {

                RuleFor(x => x.FirstName).NotEmpty().Matches("^[^><&]+$").WithName("First name");
                RuleFor(x => x.LastName).NotEmpty().Matches("^[^><&]+$").WithName("Last name");
                RuleFor(x => x.Role).GreaterThan(0);                
                RuleFor(x => x.Address1).NotEmpty().Matches("^[^><&]+$").WithName("Address");
                RuleFor(x => x.Address2).Matches("^[^><&]+$").WithName("Address 2");
                RuleFor(x => x.City).NotEmpty().Matches("^[^><&]+$");
                RuleFor(x => x.State).GreaterThan(0);
                RuleFor(x => x.PostalCode).NotEmpty().Matches("^[^><&]+$");
                RuleFor(x => x.Phone).NotEmpty().Matches("^[^><&]+$");
                RuleFor(x => x.Extension).Matches("^[^><&]+$");
                RuleFor(x => x.Fax).Matches("^[^><&]+$");
                RuleFor(x => x.Website).Matches("^[^><&]+$");

            }
        }

        public class Query : IRequest<AgencyOfficialProfileViewModel>
        {
            public int UserID { get; set; }
        }

        public class QueryHandler : IRequestHandler<Query, AgencyOfficialProfileViewModel>
        {
            private readonly ICacheHelper _cache;
            private readonly ScholarshipForServiceContext _efDB;
            private readonly IConfiguration _appSettings;

            public QueryHandler(ICacheHelper cache, ScholarshipForServiceContext db, IConfiguration appSettings)
            {
                _cache = cache;
                _efDB = db;
                _appSettings = appSettings;
            }

            public async Task<AgencyOfficialProfileViewModel> Handle(Query request, CancellationToken cancellationToken)
            {              

                var profileData = await _efDB.AgencyUsers.Where(m => m.AgencyUserId == request.UserID)
                    .Select(m => new AgencyOfficialProfileViewModel()
                    {
                         AgencyType = m.Agency.AgencyType.Name,
                         Agency = m.Agency.Name,
                         FirstName = m.Firstname,
                         LastName = m.Lastname,
                         Role = m.AgencyUserRoleID.Value,
                         Address1 = m.Address.LineOne,
                         Address2 = m.Address.LineTwo,
                         City = m.Address.City,
                         State = m.Address.StateId,
                         PostalCode = m.Address.PostalCode,
                         Phone = m.Address.PhoneNumber,
                         Extension = m.Address.PhoneExtension,
                         Fax = m.Address.Fax,
                         Email = m.Email,
                         Website = m.WebsiteUrl                        

                    }).FirstOrDefaultAsync();

                profileData.StateList = new SelectList(await _cache.GetStatesAsync(), nameof(State.StateId), nameof(State.Name));
                profileData.RoleList = new SelectList(await _cache.GetAgencyUserRolesAsync(), nameof(AgencyUserRole.AgencyUserRoleID), nameof(AgencyUserRole.Role));
                profileData.LoginGovEditUrl = _appSettings["LoginGov:EditUrl"];
                return profileData;
            }
        }

        public class Command : IRequest<bool>
        {
            public AgencyOfficialProfileViewModel Model { get; set; }
            public int UserID { get; set; }
        }

        public class CommandHandler : IRequestHandler<Command, bool>
        {
            private readonly ICacheHelper _cache;
            private readonly ScholarshipForServiceContext _efDB;

            public CommandHandler(ICacheHelper cache, ScholarshipForServiceContext db)
            {
                _cache = cache;
                _efDB = db;
            }


            public async Task<bool> Handle(Command request, CancellationToken cancellationToken)
            {
                var profileData = await _efDB.AgencyUsers.Where(m => m.AgencyUserId == request.UserID)
                    .Include(m => m.Address)
                    .FirstOrDefaultAsync();
                TextInfo textinfo = new CultureInfo("en-US", false).TextInfo;
                profileData.Firstname = textinfo.ToTitleCase(request.Model.FirstName.ToLower());
                profileData.Lastname = textinfo.ToTitleCase(request.Model.LastName.ToLower());
                profileData.AgencyUserRoleID = request.Model.Role;
                profileData.WebsiteUrl = request.Model.Website;
                if(profileData.Address == null)
                {
                    profileData.Address = new();                   
                }               
                profileData.Address.LineOne = request.Model.Address1;
                profileData.Address.LineTwo = request.Model.Address2;
                profileData.Address.City = request.Model.City;
                profileData.Address.StateId = request.Model.State;
                profileData.Address.PostalCode = request.Model.PostalCode;
                profileData.Address.PhoneNumber = request.Model.Phone;
                profileData.Address.PhoneExtension = request.Model.Extension;
                profileData.Address.Fax = request.Model.Fax;                
                await _efDB.SaveChangesAsync();
                return true;

            }
        }

    }

    
}
