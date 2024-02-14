using System;
using System.Collections.Generic;

#nullable disable

namespace OPM.SFS.Data
{
    public partial class AgencyType
    {
        public AgencyType()
        {
            Agencies = new HashSet<Agency>();
        }

        public int AgencyTypeId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public DateTime? DateInserted { get; set; }
        public string ValidEmailDomain { get; set; }

        public virtual ICollection<Agency> Agencies { get; set; }
    }
}
