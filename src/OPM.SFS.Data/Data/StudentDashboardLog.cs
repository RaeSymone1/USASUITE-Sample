using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPM.SFS.Core.Data
{
    public class StudentDashboardLog
    {
        public int StudentDashboardLogID { get; set; }
        public string BeforeChange { get; set; }
        public string AfterChange { get; set; }
        public int AdminID { get; set; }
        public DateTime TimeStamp { get; set; }

    }
}
