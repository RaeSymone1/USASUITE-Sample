using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPM.SFS.Data
{
    [Table("StudentAccountPasswordHistory")]
    public class StudentAccountPasswordHistory
    {
        public int StudentAccountPasswordHistoryID { get; set; }
        public int StudentAccountID { get; set; }
        public string Password { get; set; }
        public DateTime DateInserted { get; set; }
        public Student Student { get; set; }
        public string PasswordCrypto { get; set; }
    }
}
