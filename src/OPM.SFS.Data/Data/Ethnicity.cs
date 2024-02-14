using System;
using System.Collections.Generic;

#nullable disable

namespace OPM.SFS.Data
{
    public partial class Ethnicity
    {
        public Ethnicity()
        {
            Students = new HashSet<Student>();
        }

        public int EthnicityId { get; set; }
        public string EthnicityName { get; set; }

        public virtual ICollection<Student> Students { get; set; }
    }
}
