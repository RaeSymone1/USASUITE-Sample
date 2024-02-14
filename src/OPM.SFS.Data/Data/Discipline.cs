using System;
using System.Collections.Generic;

#nullable disable

namespace OPM.SFS.Data
{
    public partial class Discipline
    {
        public Discipline()
        {
             
        }

        public int DisciplineId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public DateTime? DateInserted { get; set; }
        public DateTime? LastModified { get; set; }

    }
}
