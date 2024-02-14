using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPM.SFS.Data
{
    public class JobActivityStatus
    {
        public JobActivityStatus()
        {
            StudentJobActivities = new HashSet<StudentJobActivity>();
        }

        public int JobActivityStatusID { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public virtual ICollection<StudentJobActivity> StudentJobActivities { get; set; }
    }
}
