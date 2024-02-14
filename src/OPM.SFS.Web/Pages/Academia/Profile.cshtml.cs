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
using OPM.SFS.Web.Models;
using OPM.SFS.Web.Infrastructure.Extensions;
using Microsoft.Extensions.Configuration;
using OPM.SFS.Web.SharedCode;

namespace OPM.SFS.Web.Pages.Academia
{
    [Authorize(Roles = "PI")]
    [ValidateAntiForgeryToken]
    public class ProfileModel : PageModel
    {
        [BindProperty]
        public AcademiaProfileViewModel Data { get; set; }

        [FromQuery(Name = "s")]
        public string IsSuccess { get; set; }

        private readonly IMediator _mediator;
        public ProfileModel(IMediator mediator) => _mediator = mediator;

        public async Task OnGetAsync()
        {
            Data = await _mediator.Send(new Query() { ID = User.GetUserId() });
            if (!string.IsNullOrWhiteSpace(IsSuccess) && IsSuccess == "true") Data.ShowSuccessMessage = true;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {               
                return Page();
            }
            var response = await _mediator.Send(new Command() { Model = Data, ID = User.GetUserId() });
            return RedirectToPage("Profile", new { s = "true" });
        }

       public class ProfileValidator : AbstractValidator<AcademiaProfileViewModel>
        {
            public ProfileValidator()
            {

                RuleFor(x => x.FirstName).NotEmpty().Matches("^[^><&]+$").WithName("First name");
                RuleFor(x => x.LastName).NotEmpty().Matches("^[^><&]+$").WithName("Last name");
                RuleFor(x => x.Role).GreaterThan(0);
                RuleFor(x => x.Department).NotEmpty().Matches("^[^><&]+$");
                RuleFor(x => x.AddressLineOne).NotEmpty().Matches("^[^><&]+$").WithName("Address");
                RuleFor(x => x.AddressLineTwo).Matches("^[^><&]+$").WithName("Address 2");
                RuleFor(x => x.City).NotEmpty().Matches("^[^><&]+$");
                RuleFor(x => x.State).GreaterThan(0);
                RuleFor(x => x.PostalCode).NotEmpty().Matches("^[^><&]+$");
                RuleFor(x => x.Phone).NotEmpty().Matches("^[^><&]+$");              
                RuleFor(x => x.Extension).Matches("^[^><&]+$");
                RuleFor(x => x.Fax).Matches("^[^><&]+$");
                RuleFor(x => x.Website).Matches("^[^><&]+$");              

            }
        }

        public class Query : IRequest<AcademiaProfileViewModel>
        {
            public int ID { get; set; }
        }

        public class QueryHandler : IRequestHandler<Query, AcademiaProfileViewModel>
        {
            private readonly ScholarshipForServiceContext _efDB;
            private readonly ICacheHelper _cache;
            private readonly IConfiguration _appSettings;


            public QueryHandler(ScholarshipForServiceContext efDB, ICacheHelper cache, IConfiguration appSettings )
            {
                _efDB = efDB;
                _cache = cache;
                _appSettings = appSettings;
             
            }

            public async Task<AcademiaProfileViewModel> Handle(Query request, CancellationToken cancellationToken)
            {
                var vm = new AcademiaProfileViewModel();
                vm.StateList = new SelectList(await _cache.GetStatesAsync(), nameof(State.StateId), nameof(State.Name));
                vm.RoleList = new SelectList(await _cache.GetAcademiaUserRoleAsync(), nameof(AcademiaUserRole.AcademiaUserRoleID), nameof(AcademiaUserRole.Role));

                var piData = await _efDB.AcademiaUsers.Where(m => m.AcademiaUserId == request.ID)
                    .Select(m => new
                    {
                        FirstName = m.Firstname,
                        LastName = m.Lastname,
                        Role = m.AcademiaUserRole.AcademiaUserRoleID,
                        Institution = m.Institution.Name,
                        Department = m.Department,
                        Address = m.Address.LineOne,
                        AddressTwo = m.Address.LineTwo,
                        City = m.Address.City,
                        State = m.Address.State.StateId,
                        PostalCode = m.Address.PostalCode,
                        Phone = m.Address.PhoneNumber,
                        Extension = m.Address.PhoneExtension,
                        Fax = m.Address.Fax,
                        Email = m.Email,
                        Website = m.WebsiteUrl
                    })
                .FirstOrDefaultAsync();

                vm.FirstName = piData.FirstName;
                vm.LastName = piData.LastName;
                vm.Role = piData.Role;
                vm.Institution = piData.Institution;
                vm.Department = piData.Department;
                vm.AddressLineOne = piData.Address;
                vm.AddressLineTwo = piData.AddressTwo;
                vm.City = piData.City;
                vm.State = piData.State;
                vm.PostalCode = piData.PostalCode;
                vm.Phone = piData.Phone;
                vm.Extension = piData.Extension;
                vm.Fax = piData.Fax;
                vm.Email = piData.Email;
                vm.Website = piData.Website;
                vm.LoginGovEditUrl = _appSettings["LoginGov:EditUrl"];
                return vm;
            }
        }

        public class Command : IRequest<bool>
        {
            public int ID { get; set; }
            public AcademiaProfileViewModel Model { get; set; }
        }

        public class CommandHandler : IRequestHandler<Command, bool>
        {
            private readonly ScholarshipForServiceContext _efDB;
            private readonly ICacheHelper _cache;

            public CommandHandler(ScholarshipForServiceContext efDB, ICacheHelper cache)
            {
                _efDB = efDB;
                _cache = cache;

            }

            public async Task<bool> Handle(Command request, CancellationToken cancellationToken)
            {
                var piData = await _efDB.AcademiaUsers.Where(m => m.AcademiaUserId == request.ID)
                    .Include(m => m.Address)
                    .FirstOrDefaultAsync();

                TextInfo textinfo = new CultureInfo("en-US", false).TextInfo;
                piData.Firstname = textinfo.ToTitleCase(request.Model.FirstName.ToLower());
                piData.Lastname = textinfo.ToTitleCase(request.Model.LastName.ToLower());
                piData.AcademiaUserRoleID = request.Model.Role;
                piData.Department = request.Model.Department;
                piData.WebsiteUrl = request.Model.Website;
                if(piData.Address is null)
                {
                    Address a = new();
                    a.LineOne = request.Model.AddressLineOne;
                    a.LineTwo = request.Model.AddressLineTwo;
                    a.City = request.Model.City;
                    a.StateId = request.Model.State;
                    a.PostalCode = request.Model.PostalCode;
                    a.PhoneNumber = request.Model.Phone;
                    a.PhoneExtension = request.Model.Extension;
                    a.Fax = request.Model.Fax;
                    piData.Address = a;
                }
                else
                {
                    piData.Address.LineOne = request.Model.AddressLineOne;
                    piData.Address.LineTwo = request.Model.AddressLineTwo;
                    piData.Address.City = request.Model.City;
                    piData.Address.StateId = request.Model.State;
                    piData.Address.PostalCode = request.Model.PostalCode;
                    piData.Address.PhoneNumber = request.Model.Phone;
                    piData.Address.PhoneExtension = request.Model.Extension;
                    piData.Address.Fax = request.Model.Fax;
                }
                await _efDB.SaveChangesAsync();
                return true;
            }
        }
    }

    
}
