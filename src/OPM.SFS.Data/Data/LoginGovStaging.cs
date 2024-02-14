using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPM.SFS.Data
{
    public partial class LoginGovStaging
    {
        public Guid LoginGovStagingID { get; set; }
        public int AccountID { get; set; }
        public string AccountType { get; set; }
        public string LoginGovLinkID { get; set; }
        public DateTime DateInserted { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}
