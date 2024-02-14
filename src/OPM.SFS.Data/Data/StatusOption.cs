using System;
using System.Collections.Generic;

#nullable disable

namespace OPM.SFS.Data
{
    public partial class StatusOption
    {
        public int StudentStatusId { get; set; }
        public string Option { get; set; }
        public string Status { get; set; }
        public string Phase { get; set; }
        public string PostGradPlacementGroup { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
