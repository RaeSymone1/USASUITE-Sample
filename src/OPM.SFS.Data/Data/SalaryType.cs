using System;
using System.Collections.Generic;

#nullable disable

namespace OPM.SFS.Data
{
    public partial class SalaryType
    {
        public SalaryType()
        {
            StudentCommitments = new HashSet<StudentCommitment>();
        }

        public int SalaryTypeId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }

        public virtual ICollection<StudentCommitment> StudentCommitments { get; set; }
    }
}
