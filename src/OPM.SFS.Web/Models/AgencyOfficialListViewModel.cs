using System.Collections.Generic;

namespace OPM.SFS.Web.Models
{
    public class AgencyOfficialListViewModel
    {
        public string RefererURL { get; set; }
        public List<AgencyOfficial> AgencyOfficials { get; set; }
               


        public class AgencyOfficial
        {
            
            public string Name { get; set; }
            public string Agency { get; set; }
            public string SubAgency { get; set; }
            public string AgencyType { get; set; }
            public string Email { get; set; }
            public string Phone { get; set; }
        }
    }

   
}
