using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPM.SFS.Data
{
    public class AuditEventLog
    {
        public int AuditEventLogID { get; set; }
        public string Message { get; set; }
        public string LogEvent { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
