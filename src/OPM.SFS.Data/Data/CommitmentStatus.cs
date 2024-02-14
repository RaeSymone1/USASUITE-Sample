using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPM.SFS.Data
{
    public class CommitmentStatus
    {
        public CommitmentStatus()
        {
            StudentCommitments = new HashSet<StudentCommitment>();
        }

        public int CommitmentStatusID { get; set; }
        public string Code { get; set; }
        public string Value { get; set; }
        public string StudentDisplay { get; set; }
        public string AdminDisplay { get; set; }
        public string Description { get; set; }
        public bool IsDisabled { get; set; }
        public virtual ICollection<StudentCommitment> StudentCommitments { get; set; }
    }
}
