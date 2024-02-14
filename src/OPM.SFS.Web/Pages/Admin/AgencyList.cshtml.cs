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
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OPM.SFS.Web.Pages.Admin
{
    [Authorize(Roles = "AD")]
    public class AgencyListModel : PageModel
    {
        [BindProperty]
        public AdminAgencyListViewModel Data { get; set; }

        [FromQuery(Name = "t")]
        public int TypeID { get; set; } = 0;

       

        private readonly IMediator _mediator;

        public AgencyListModel(IMediator mediator) => _mediator = mediator;
		
        public async Task OnGetAsync()
        {
            Data = await _mediator.Send(new Query() {  AgencyTypeID = TypeID});
        }

        public  PartialViewResult OnGetFilterTablePartial(int typeid, int stateID)
        {
            var filteredAgencies = _mediator.Send(new QueryFilterTable() { TypeID = typeid, StateID = stateID }).Result;
            return new PartialViewResult
            {

                ViewName = "_AdminAgencyListPartial",
                ViewData = new ViewDataDictionary<List<AdminAgencyListViewModel.AgencyListItem>>(ViewData, filteredAgencies)
            };
        }


        public class Query : IRequest<AdminAgencyListViewModel>
		{
            public int AgencyTypeID { get; set; }
        }

		public class QueryHandler : IRequestHandler<Query, AdminAgencyListViewModel>
		{
            private readonly ScholarshipForServiceContext _efDB;
            private readonly ICacheHelper _cache;

            public QueryHandler(ScholarshipForServiceContext efDB, ICacheHelper cache)
            {
                _efDB = efDB;
                _cache = cache;
            }

            public async Task<AdminAgencyListViewModel> Handle(Query request, CancellationToken cancellationToken)
			{
                //Make the data table a partial page so that it can be updated without reloading the full page 
                //when changing the agency type
                //For reference - https://www.learnrazorpages.com/razor-pages/ajax/partial-update
                AdminAgencyListViewModel data = new();
                data.Agencies = new();
              
                var allAgencies = await _efDB.Agencies
                    .Select(m => new
                    {
                        AgencyID = m.AgencyId,
                        Name = m.Name,
                        Type = m.AgencyType.Name,
                        IsDisabled = m.IsDisabled,
                        ParentID = m.ParentAgencyId
                    }).ToListAsync();

                var allAgenciesToDisplay = allAgencies.Where(m => m.IsDisabled != true);

                var agencyTypes = await _cache.GetAgencyTypesAsync();
                data.StateList = new SelectList(await _cache.GetStatesAsync(), nameof(State.StateId), nameof(State.Name));

                if (request.AgencyTypeID == 0)
                {
                    var defaultFilter = agencyTypes.Where(m => m.Code == "FederalExec").FirstOrDefault().AgencyTypeId;
                    data.FilterAgencyType = defaultFilter;
                }
                else
                    data.FilterAgencyType = request.AgencyTypeID;

                //var theAgencies = allAgencies.Where(m => m.IsDisabled != true).ToList();
                
               
                data.AgencyTypeList = new SelectList(agencyTypes, nameof(AgencyType.AgencyTypeId), nameof(AgencyType.Name));
                
                foreach (var item in allAgenciesToDisplay)
				{
                    AdminAgencyListViewModel.AgencyListItem a = new();
                    a.AgencyID = item.AgencyID;
                    if(item.ParentID.HasValue && item.ParentID.Value > 0)
					{
                        a.AgencyName = allAgencies.Where(m => m.AgencyID == item.ParentID).FirstOrDefault().Name;
                        a.SubAgency = item.Name;
					}
                    else
					{
                        a.AgencyName = item.Name;
                        a.SubAgency = "";
					}
                    a.AgencyType = item.Type;
                    data.Agencies.Add(a);
				}
                return data;
			}
		}

        public class QueryFilterTable : IRequest<List<AdminAgencyListViewModel.AgencyListItem>>
        {
            public int TypeID { get; set; }
            public int StateID { get; set; }
        }

        public class QueryFilterTableHandler : IRequestHandler<QueryFilterTable, List<AdminAgencyListViewModel.AgencyListItem>>
        {
            private readonly ScholarshipForServiceContext _efDB;
            private readonly ICacheHelper _cache;

            public QueryFilterTableHandler(ScholarshipForServiceContext efDB, ICacheHelper cache)
            {
                _efDB = efDB;
                _cache = cache;
            }

            public async Task<List<AdminAgencyListViewModel.AgencyListItem>> Handle(QueryFilterTable request, CancellationToken cancellationToken)
            {
                var theAgencies = await _efDB.Agencies
                  .Where(m => m.IsDisabled == false && m.AgencyTypeId == request.TypeID)
                  .WhereIf(request.StateID > 0, m => m.Address.StateId == request.StateID)
                  .Select(m => new
                  {
                      AgencyId = m.AgencyId,
                      ParentAgencyId = m.ParentAgencyId,
                      Name = m.Name,
                      AgencyType = m.AgencyType.Name
                  })
                  .ToListAsync();

                var allAgencies = await _cache.GetAgenciesWithDisabledAsync();
                var model = new List<AdminAgencyListViewModel.AgencyListItem>();

                foreach (var item in theAgencies)
                {
                    AdminAgencyListViewModel.AgencyListItem a = new();
                    a.AgencyID = item.AgencyId;
                    if (item.ParentAgencyId.HasValue && item.ParentAgencyId.Value > 0)
                    {
                        a.AgencyName = allAgencies.Where(m => m.AgencyId == item.ParentAgencyId).FirstOrDefault().Name;
                        a.SubAgency = item.Name;
                    }
                    else
                    {
                        a.AgencyName = item.Name;
                        a.SubAgency = "";
                    }
                    a.AgencyType = item.AgencyType;
                    model.Add(a);
                }
                return model;
            }
        }

    }
}
