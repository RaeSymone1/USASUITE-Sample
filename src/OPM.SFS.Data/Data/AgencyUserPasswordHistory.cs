using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPM.SFS.Data
{

    [Table("AgencyUserPasswordHistory")]
    public class AgencyUserPasswordHistory
    {      
        public int AgencyUserPasswordHistoryID { get; set; }
        public int AgencyUserID { get; set; }
        public string Password { get; set; }
        public DateTime DateInserted { get; set; }
        public AgencyUser AgencyUser { get; set; }      
    }
}
