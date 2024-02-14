using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPM.SFS.Data
{
    public class RegistrationCode
    {
        public int RegistrationCodeID { get; set; }
        public string QuarterName { get; set; }
        public string Code { get; set; }
        public DateTime QuarterStartDate { get; set; }
    }
}
