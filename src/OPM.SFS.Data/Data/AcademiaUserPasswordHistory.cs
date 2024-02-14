using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPM.SFS.Data
{
    [Table("AcademiaUserPasswordHistory")]
    public class AcademiaUserPasswordHistory
    {
        public int AcademiaUserPasswordHistoryID { get; set; }
        public int AcademiaUserID { get; set; }
        public string Password { get; set; }
        public DateTime DateInserted { get; set; }
        public AcademiaUser AcademiaUser { get; set; }
    }
}
