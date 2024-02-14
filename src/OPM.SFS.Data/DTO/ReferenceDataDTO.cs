using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPM.SFS.Core.DTO
{
    public class AgencyReferenceDTO
    {
        public int AgencyId { get; set; }
        public int? AgencyTypeId { get; set; }
        public string Name { get; set; }
        public int? ParentAgencyId { get; set; }
        public int? StateID { get; set; }
        public string Workflow { get; set; }
    }
}
