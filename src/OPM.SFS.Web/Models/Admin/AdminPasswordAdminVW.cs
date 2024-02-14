using System.Collections.Generic;

namespace OPM.SFS.Web.Models
{
    public class AdminPasswordAdminVW
    {
        public string SearchOption { get; set; }
        public string SearchFilter { get; set; }
        public List<Account> Accounts { get; set; }
        public string SearchResultsMessage { get; set; }
        public bool ShowSuccessMessage { get; set; }
        public string Recipient { get; set; }
        public class Account
        {
            public int AccountID { get; set; }
            public string LastName { get; set; }
            public string FirstName { get; set; }
            public string Email { get; set; }
            public string Type { get; set; }
        }
    }
}
