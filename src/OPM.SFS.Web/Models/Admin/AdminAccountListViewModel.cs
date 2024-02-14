using System.Collections.Generic;

namespace OPM.SFS.Web.Models
{
    public class AdminAccountListViewModel
    {

        public string AccountType { get; set; }

        public List<AccountItems> Accounts { get; set; }

        public class AccountItems
        {
            public int AccountID { get; set; }
            public string Username { get; set; }
            public string Email { get; set; }
            public string FirstName { get; set; }
            public string Lastname { get; set; }
            public string AgencyOrInstitution { get; set; }
            public string ProfileStatus { get; set; }
            public string Display { get; set; }

        }

    }
}
