using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPM.SFS.Data
{
    [Table("AdminUserPasswordHistory")]
    public class AdminUserPasswordHistory
    {
        public int AdminUserPasswordHistoryID { get; set; }
        public int AdminUserID { get; set; }
        public string Password { get; set; }
        public DateTime DateInserted { get; set; }
        public AdminUser AdminUser { get; set; }
    }
}
