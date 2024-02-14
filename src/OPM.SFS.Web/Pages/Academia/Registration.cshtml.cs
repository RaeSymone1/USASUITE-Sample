using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OPM.SFS.Core.Shared;
using OPM.SFS.Data;
using OPM.SFS.Web.Models.Academia;
using OPM.SFS.Web.Shared;
using OPM.SFS.Web.SharedCode;
using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OPM.SFS.Web.Pages.Academia
{
    public class RegistrationModel : PageModel
    {
        [BindProperty]
        public AcademiaRegistrationViewModel Data { get; set; }

        [FromQuery(Name = "s")]
        public string IsSuccess { get; set; }

        private readonly IMediator _mediator;

        public RegistrationModel(IMediator mediator) => _mediator = mediator;


        public async Task OnGetAsync()
        {
            Data = await _mediator.Send(new Query() { });
           
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            var response = await _mediator.Send(new Command() { Model = Data, });
            if(response == false)
            {
                ModelState.AddModelError("", "A record with this User ID already exists!");
            }
            return RedirectToPage("RegistrationConfirm");
        }

        public class RegistrationValidator : AbstractValidator<AcademiaRegistrationViewModel>
        {
            public RegistrationValidator()
            {

                RuleFor(x => x.FirstName).NotEmpty().Matches("^[^><&]+$").WithName("First name");
                RuleFor(x => x.LastName).NotEmpty().Matches("^[^><&]+$").WithName("Last name");
                RuleFor(x => x.Role).GreaterThan(0);
                RuleFor(x => x.Institution).GreaterThan(0);
                RuleFor(x => x.Department).NotEmpty().Matches("^[^><&]+$");
                RuleFor(x => x.Email).NotEmpty().EmailAddress().Matches("^[^><&]+$");
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

        public class Query : IRequest<AcademiaRegistrationViewModel>
        {

        }

        public class QueryHandler : IRequestHandler<Query, AcademiaRegistrationViewModel>
        {
            private readonly ICacheHelper _cache;
            private readonly ScholarshipForServiceContext _efDB;

            public QueryHandler(ScholarshipForServiceContext efDB, ICacheHelper cache)
            {
                _efDB = efDB;
                _cache = cache;
            }

            public async Task<AcademiaRegistrationViewModel> Handle(Query request, CancellationToken cancellationToken)
            {
                AcademiaRegistrationViewModel model = new();
                model.StateList = new SelectList(await _cache.GetStatesAsync(), nameof(State.StateId), nameof(State.Name));
                model.RoleList = new SelectList(await _cache.GetAcademiaUserRoleAsync(), nameof(AcademiaUserRole.AcademiaUserRoleID), nameof(AcademiaUserRole.Role));
                var institutions = await _efDB.Institutions.Include(m => m.InstitutionType).Where(x => x.InstitutionType.Name == "4 Year").OrderBy(x => x.Name).ToListAsync();
                model.InstitutionList = new SelectList(institutions, nameof(Institution.InstitutionId), nameof(Institution.Name));
                return model;
            }
        }

        public class Command : IRequest<bool>
        {
            public AcademiaRegistrationViewModel Model { get; set; }
        }

        public class CommnandHandler : IRequestHandler<Command, bool>
        {

            private readonly IEmailerService _emailer;
            private readonly IAcademiaRepository _academiaRegRepo;

            public CommnandHandler(IEmailerService emailer, IAcademiaRepository academiaRegRepo)
            {

                _emailer = emailer;
                _academiaRegRepo = academiaRegRepo;
            }

            public async Task<bool> Handle(Command request, CancellationToken cancellationToken)
            {
                var checkEmailData = await _academiaRegRepo.GetExistingAcademiaUsers(request.Model.Email);
                if (checkEmailData == null)
                {
                    await _academiaRegRepo.AddNewAcademiaUser(request.Model);
                    string emailContent = $@"Hello Admins, <br /><br /> A Principle Investigator Registration has been submitted.";
                    await _emailer.SendEmailNoTemplateAsync("sfs@opm.gov", $"SFS Principle Investigator Registration: {request.Model.LastName}, {request.Model.FirstName}", emailContent);

                    return true;
                }

                return false;
                
            }
        }
    }
}
