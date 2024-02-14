using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace OPM.SFS.Web.Models
{
    public class AdminAgencyListViewModel
    {
        public SelectList AgencyTypeList { get; set; }
        public int FilterAgencyType { get; set; }
        public List<AgencyListItem> Agencies { get; set; }
        public int StateFilter { get; set; }
        public SelectList StateList { get; set; }

        public class AgencyListItem
        {
            public int AgencyID { get; set; }
            public string AgencyType { get; set; }
            public string AgencyName { get; set; }
            public string SubAgency { get; set; }
        }
    }
}
