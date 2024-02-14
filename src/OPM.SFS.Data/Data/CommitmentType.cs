using System;
using System.Collections.Generic;

#nullable disable

namespace OPM.SFS.Data
{
    public partial class CommitmentType
    {
        public CommitmentType()
        {
            StudentCommitments = new HashSet<StudentCommitment>();
        }

        public int CommitmentTypeId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }

        public virtual ICollection<StudentCommitment> StudentCommitments { get; set; }
    }
}
