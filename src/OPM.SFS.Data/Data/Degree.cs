using System;
using System.Collections.Generic;

#nullable disable

namespace OPM.SFS.Data
{
    public partial class Degree
    {
        public Degree()
        {
            StudentInstitutionFundings = new HashSet<StudentInstitutionFunding>();
        }

        public int DegreeId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public DateTime? DateInserted { get; set; }
        public DateTime? LastModified { get; set; }

        public virtual ICollection<StudentInstitutionFunding> StudentInstitutionFundings { get; set; }
    }
}
