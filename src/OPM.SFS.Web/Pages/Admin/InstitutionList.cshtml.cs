using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OPM.SFS.Data;
using OPM.SFS.Web.Models;
using OPM.SFS.Web.SharedCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OPM.SFS.Web.Pages.Admin
{
    [Authorize(Roles = "AD")]
    public class InstitutionListModel : PageModel
    {
        [BindProperty]
        public AdminInstitutionListViewModel Data { get; set; }

        private readonly IMediator _mediator;

        public InstitutionListModel(IMediator mediator) => _mediator = mediator;


        public async Task OnGetAsync()
        {
            Data = await _mediator.Send(new Query());
        }

        public class Query : IRequest<AdminInstitutionListViewModel>
        {

        }

        public class QueryHandler : IRequestHandler<Query, AdminInstitutionListViewModel>
        {
            private readonly ScholarshipForServiceContext _efDB;
            private readonly ICacheHelper _cache;

            public QueryHandler(ScholarshipForServiceContext efDB, ICacheHelper cache)
            {
                _efDB = efDB;
                _cache = cache;
            }

            public async Task<AdminInstitutionListViewModel> Handle(Query request, CancellationToken cancellationToken)
            {
                AdminInstitutionListViewModel model = new();
                model.Institutions = new();
                var allInstitution = await _cache.GetInstitutionsAsync();
                var data = await _efDB.Institutions
                    .Select(m => new
                    {
                        ID = m.InstitutionId,
                        Name = m.Name,
                        Type = m.InstitutionType.Name,
                        GrantNumber = m.GrantNumber,
                        GrantExpirationDate = m.GrantExpirationDate,
                        ParentInstitutionID = m.ParentInstitutionID,
                        IsAcceptingApps = m.IsAcceptingApplications,
                        IsActive = m.IsActive,
                        Contacts = m.InstitutionContacts.Select(c => new
                        {
                            ContactType = c.InstitutionContactType.Name,
                            Name = $"{c.FirstName} {c.LastName}",
                            Phone = c.Phone,
                            Email = c.Email
                        })
                    }).OrderBy(x => x.Name).ToListAsync();

                foreach (var item in data)
                {
                    AdminInstitutionListViewModel.InstiutionItem i = new();
                    i.InstitutionID = item.ID;
                    i.Institution = item.Name;
                    i.InstitutionType = item.Type;
                    if(item.ParentInstitutionID.HasValue && item.ParentInstitutionID > 0)
                    {
                        var x = allInstitution.Where(m => m.InstitutionId == item.ParentInstitutionID).FirstOrDefault();
                        i.GrantNumber = x.GrantNumber;
                        i.GrantExpirationDate = x.GrantExpirationDate.HasValue ? x.GrantExpirationDate.Value.ToShortDateString() : "";
                    }
                    else
                    {
                        i.GrantNumber = item.GrantNumber;
                        i.GrantExpirationDate = item.GrantExpirationDate.HasValue ? item.GrantExpirationDate.Value.ToShortDateString() : "";
                    }
                    i.IsAcceptingApplicataions = item.IsAcceptingApps ? "Yes" : "No";
                    i.IsActive = item.IsActive ? "Yes" : "No";
                    i.Contacts = new();
                    foreach (var c in item.Contacts)
                    {
                        i.Contacts.Add(new AdminInstitutionListViewModel.InstiutionItem.Contact()
                        {
                            Email = c.Email,
                            Name = c.Name,
                            Phone = c.Phone,
                            Role = c.ContactType
                        });
                    }
                    model.Institutions.Add(i);
                }

                return model;
            }
        }
    }
}
