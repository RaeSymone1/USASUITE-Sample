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
    public class InstitutionContactsModel : PageModel
    {
        [BindProperty]
        public InstitutionContactsVM Data { get; set; }

        [FromQuery(Name = "iid")]
        public int InstitutionID { get; set; } = 0;

        [FromQuery(Name = "cid")]
        public int ContactID { get; set; } = 0;

        public string IsSuccess { get; set; }

        private readonly IMediator _mediator;

        public InstitutionContactsModel(IMediator mediator) => _mediator = mediator;

        public async Task OnGetAsync()
        {
            Data = await _mediator.Send(new Query() { InstitutionID = InstitutionID, ContactID = ContactID});
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {

                return Page();
            }
            var save = await _mediator.Send(new Command() { Model = Data });
            Data = await _mediator.Send(new Query() { InstitutionID = Data.InstitutionID, ContactID = ContactID });
            if (save.IsSuccess == true)
            {
                Data.ShowSuccessMessage = true;
                Data.SuccessMessage = save.ErrorMessage;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostDeleteAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            var result = await _mediator.Send(new CommandDelete() { Model = Data });
            Data = await _mediator.Send(new Query() { InstitutionID = Data.InstitutionID, ContactID = ContactID });
            if (result.IsSuccess == true)
            {
                Data.ShowSuccessMessage = true;
                Data.SuccessMessage = result.ErrorMessage;
            }
            return Page();
        }

        public class InstitutionContactsVMValidator : AbstractValidator<InstitutionContactsVM>
        {
            public InstitutionContactsVMValidator()
            {
                RuleFor(x => x.FirstName).NotEmpty().Matches("^[^><&]+$");
                RuleFor(x => x.LastName).NotEmpty().Matches("^[^><&]+$");
                RuleFor(x => x.ContactType).NotNull().NotEqual(0);
                RuleFor(x => x.Phone).NotEmpty().Matches("^[^><&]+$");
                RuleFor(x => x.Email).NotEmpty().Matches("^[^><&]+$");
                RuleFor(x => x.Title).Matches("^[^><&]+$");
                RuleFor(x => x.PhoneExt).Matches("^[^><&]+$");
            }

        }

        public class Query : IRequest<InstitutionContactsVM>
        {
            public int InstitutionID { get; set; }
            public int ContactID { get; set; }
        }

        public class QueryHandler : IRequestHandler<Query, InstitutionContactsVM>
        {
            private readonly ScholarshipForServiceContext _db;
            private readonly ICacheHelper _cache;

            public QueryHandler(ScholarshipForServiceContext db, ICacheHelper cache)
            {
                _db = db;
                _cache = cache;
            }

            public async Task<InstitutionContactsVM> Handle(Query request, CancellationToken cancellationToken)
            {
                InstitutionContactsVM model = new();
                model.Contacts = new();
                var instituionData = await _db.Institutions.Where(m => m.InstitutionId == request.InstitutionID)
                    .Select(m => m.Name).FirstOrDefaultAsync();

                var contacts = await _db.InstitutionContact.Where(m => m.InstitutionId == request.InstitutionID)
                    .Select(m => new
                    {
                        ID = m.InstitutionContactId,
                        FirstName = m.FirstName,
                        LastName = m.LastName,
                        Phone = m.Phone,
                        Email = m.Email,
                        Role = m.InstitutionContactType.Name,
                        TypeID = m.InstitutionContactTypeID,
                        Title = m.Title,
                        PhoneExt = m.PhoneExt
                    })
                    .ToListAsync();
                model.InstitutionID = request.InstitutionID;
                model.Institution = instituionData;
                model.ContactTypeList = new SelectList(await _cache.GetInstituionContactTypeAsync(), nameof(InstitutionContactType.InstitutionContactTypeID), nameof(InstitutionContactType.Name));
                foreach (var contact in contacts)
                {
                    model.Contacts.Add(new InstitutionContactsVM.Contact()
                    {
                        Name = $"{contact.LastName}, {contact.FirstName}",
                        ContactID = contact.ID,
                        Phone = contact.Phone,
                        Email = contact.Email,
                        Role = contact.Role
                    });
                }

                if(request.ContactID > 0)
                {
                    var theContact = contacts.Where(m => m.ID == request.ContactID).FirstOrDefault();                    
                    model.ContactType = theContact.TypeID;
                    model.FirstName = theContact.FirstName;
                    model.LastName = theContact.LastName;
                    model.Title = theContact.Title;
                    model.Email = theContact.Email;
                    model.Phone = theContact.Phone;
                    model.PhoneExt = theContact.PhoneExt;
                    model.InstitutionContactID = request.ContactID;
                }
                return model;          
            }
        }

        public class Command : IRequest<CommandResult>
        {
            public InstitutionContactsVM Model { get; set; }
        }

        public class CommandHandler : IRequestHandler<Command, CommandResult>
        {
            private readonly ScholarshipForServiceContext _db;
            private readonly ICacheHelper _cache;

            public CommandHandler(ScholarshipForServiceContext db, ICacheHelper cache)
            {
                _db = db;
                _cache = cache;
            }

            public async Task<CommandResult> Handle(Command request, CancellationToken cancellationToken)
            {
                
                if(request.Model.InstitutionContactID > 0)
                {                    
                    var theContact = await _db.InstitutionContact.Where(m => m.InstitutionContactId == request.Model.InstitutionContactID).FirstOrDefaultAsync();
                    theContact.InstitutionContactTypeID = request.Model.ContactType; 
                    theContact.FirstName = request.Model.FirstName;
                    theContact.LastName = request.Model.LastName;
                    theContact.Title = request.Model.Title;
                    theContact.Phone = request.Model.Phone;
                    theContact.PhoneExt = request.Model.PhoneExt;
                    theContact.Email = request.Model.Email;
                    await _db.SaveChangesAsync();
                    return new CommandResult() { IsSuccess = true, ErrorMessage = "Institution contact updated successfully." };
                }
                else
                {
                    var theInstitution = await _db.Institutions
                    .Include(m => m.InstitutionContacts)
                    .Where(m => m.InstitutionId == request.Model.InstitutionID)
                    .FirstOrDefaultAsync();
                    InstitutionContact newContact = new();
                    newContact.InstitutionId = request.Model.InstitutionID;
                    newContact.InstitutionContactTypeID = request.Model.ContactType;
                    newContact.FirstName = request.Model.FirstName;
                    newContact.LastName = request.Model.LastName;
                    newContact.Title = request.Model.Title;
                    newContact.Phone = request.Model.Phone;
                    newContact.PhoneExt = request.Model.PhoneExt;
                    newContact.Email = request.Model.Email;
                    newContact.DateInserted = DateTime.UtcNow;
                    theInstitution.InstitutionContacts.Add(newContact);
                    await _db.SaveChangesAsync();
                    return new CommandResult() { IsSuccess = true, ErrorMessage = "Institution contact added successfully." };
                }
                
            }
        }

        public class CommandResult
        {
            public bool IsSuccess { get; set; }
            public string ErrorMessage { get; set; }
        }

        public class CommandDelete : IRequest<CommandResult>
        {
            public InstitutionContactsVM Model { get; set; }
        }

        public class CommandDeleteHandler : IRequestHandler<CommandDelete, CommandResult>
        {
            private readonly ScholarshipForServiceContext _db;
            private readonly ICacheHelper _cache;


            public CommandDeleteHandler(ScholarshipForServiceContext db, ICacheHelper cache)
            {
                _db = db;
                _cache = cache;
            }

            public async Task<CommandResult> Handle(CommandDelete request, CancellationToken cancellationToken)
            {
                if (request.Model.InstitutionContactID > 0 && request.Model.InstitutionID > 0)
                {
                    var data = await _db.InstitutionContact.FirstOrDefaultAsync(m => m.InstitutionContactId == request.Model.InstitutionContactID);
                    _db.InstitutionContact.Remove(data);
                    await _db.SaveChangesAsync();
                    return new CommandResult() { IsSuccess = true, ErrorMessage = "Institution contact delete successfully." };
                }
                return new CommandResult() { IsSuccess = true, ErrorMessage = "Institution contact delete failed." };
            }
        }
    }
}
