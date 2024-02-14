using System.Collections.Generic;
namespace OPM.SFS.Web.Models
{
    public class AdminPIAgencyListViewModel
    {
        public string AccountType { get; set; }
        public List<PIAgencyItem> PIAgency { get; set; }

        public class PIAgencyItem
        {
            public int UserID { get; set; }
            public string UserName { get; set; }
            public string Email { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string AgencyOrInstitution { get; set; }
            public string ProfileStatus { get; set; }
            public string Display { get; set; }

        }

    }
}
