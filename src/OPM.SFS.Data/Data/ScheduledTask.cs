using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPM.SFS.Core.Data
{
    public class ScheduledTask
    {
        public int ScheduledTaskId { get; set; }
        public string Name { get; set; }
        public string Schedule { get; set; }
        public string State { get; set; }
        public DateTime? LastRunDate { get; set; }
        public DateTime? LastUpdated { get; set; }
        public bool IsDisabled { get; set; }

    }
}
