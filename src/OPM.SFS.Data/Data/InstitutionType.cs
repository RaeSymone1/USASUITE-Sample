using System;
using System.Collections.Generic;

#nullable disable

namespace OPM.SFS.Data
{
    public partial class InstitutionType
    {
        public InstitutionType()
        {
            Institutions = new HashSet<Institution>();
        }

        public int InstitutionTypeId { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Institution> Institutions { get; set; }
    }
}
