using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OPM.SFS.Data;
using OPM.SFS.Web.Models;
using OPM.SFS.Web.Shared;
using OPM.SFS.Web.SharedCode;

namespace OPM.SFS.Web.Pages.Academia
{
    public class InstitutionsModel : PageModel
    {
        [BindProperty]
        public ParticipatingInstitutionsVM Data { get; set; }

        private readonly IMediator _mediator;

        public InstitutionsModel(IMediator mediator) => _mediator = mediator;

        public async Task OnGetAsync()
        {
            Data = await _mediator.Send(new Query() { });

        }

        public async Task<JsonResult> OnGetInstitutionSummary()
        {
            var summaryData = await _mediator.Send(new JsonQueryInstitionSummary() { });
            return new JsonResult(summaryData);
        }

        public class JsonQueryInstitionSummary : IRequest<Dictionary<string, ParticipatingInstitutionsVM.StateVM>>
        {     }

        public class JsonQueryInstitionSummaryHandler : IRequestHandler<JsonQueryInstitionSummary, Dictionary<string, ParticipatingInstitutionsVM.StateVM>>
        {
            private readonly ScholarshipForServiceContext _efDB;
            private readonly ICacheHelper _cache;

            public JsonQueryInstitionSummaryHandler(ScholarshipForServiceContext db, ICacheHelper cache )
            {
                _efDB = db;
                _cache = cache;
            }
            public async Task<Dictionary<string, ParticipatingInstitutionsVM.StateVM>> Handle(JsonQueryInstitionSummary request, CancellationToken cancellationToken)
            {
                var allStates = await _cache.GetStatesAsync();
                var _summary = await _efDB.Institutions.Where(m => m.IsActive == true)
                    .GroupBy(m => m.State.Abbreviation)
                    .Select(m => new { StateKey = m.Key, StateCount = m.Count() })
                    .ToListAsync();

                Dictionary<string, ParticipatingInstitutionsVM.StateVM> summaryVM = new();

                foreach (var state in allStates)
                {
                    var summary = _summary.Where(m => m.StateKey == state.Abbreviation).FirstOrDefault();
                    ParticipatingInstitutionsVM.StateVM d = new();
                    if (summary != null)
                    {                        
                        d.Color = "#0071BC";
                        d.Description = $"{summary.StateCount} participating institutions";
                        //d.Url = $"#{state.Name}";
                        d.Hover_color = "#bada55";
                        d.Hide = "no";
                        d.Name = state.Name;
                    }
                    else
                    {                       
                        d.Color = "#d1d1d1";
                        d.Description = $"No participating institutions at this time.";
                        //d.Url = $"#{state.Name}";
                        d.Hover_color = "#bada55";
                        d.Hide = "no";
						d.Name = state.Name;

					}
                    summaryVM.Add(state.Abbreviation, d);
                }
               
                return summaryVM;
            }
        }


        public class Query : IRequest<ParticipatingInstitutionsVM>
        {

        }

        public class QueryHandler : IRequestHandler<Query, ParticipatingInstitutionsVM>
        {
            private readonly ScholarshipForServiceContext _efDB;

            public QueryHandler(ScholarshipForServiceContext efDB)
            {
                _efDB = efDB;
            }
            public async Task<ParticipatingInstitutionsVM> Handle(Query request, CancellationToken cancellationToken)
            {             

                var _institutions = await _efDB.Institutions.Where(m => m.IsActive == true)
                    .Select(InstitutionContactItem.Projection)
                    .OrderBy(m => m.StateName).ToListAsync();                    

                ParticipatingInstitutionsVM model = new();
                model.FourYear = new();
                model.C3P = new();
                model.AllActiveInstitutions = new();

                foreach(var item in _institutions)
                {
                    
                    var institionItem = new ParticipatingInstitutionsVM.InstitutionDetails();
                    institionItem.ID = item.ID;
                    institionItem.Name = item.Name;
                    institionItem.Link = item.Link;
                    institionItem.IsAcceptingApplications = item.IsAcceptingApps;
                    institionItem.AddressLine = $"{item.City}, {item.State} {item.Postal}";
                    institionItem.ProgramPage = item.ProgramPage;
                    institionItem.Contacts = new();
                    institionItem.Type = item.Type;
                    foreach (var c in item.Contacts)
                    {
                        institionItem.Contacts.Add(new ParticipatingInstitutionsVM.InstitutionDetails.Contact()
                        {
                            Email = c.Email,
                            Name = c.Name,
                            Phone = c.Phone,
                            Role = c.ContactType
                        });
                    }
                    if (!model.AllActiveInstitutions.ContainsKey(item.StateName))
                        model.AllActiveInstitutions.Add(item.StateName, new List<ParticipatingInstitutionsVM.InstitutionDetails>() { institionItem });
                    else
                    {
                        var currentItems = model.AllActiveInstitutions[item.StateName];
                        currentItems.Add(institionItem);
                        model.AllActiveInstitutions[item.StateName] = currentItems.OrderBy(m => m.Name).ToList();
                    }                  
                }

                return model;
            }
        }
    }
}
