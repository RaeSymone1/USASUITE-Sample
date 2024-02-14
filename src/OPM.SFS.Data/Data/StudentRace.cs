using System;
using System.Collections.Generic;

#nullable disable

namespace OPM.SFS.Data
{
    public partial class StudentRace
    {
        public int StudentRaceD { get; set; }
        public int StudentId { get; set; }
        public int RaceId { get; set; }
        public DateTime DateInserted { get; set; }

        public virtual Race Race { get; set; }
        public virtual Student Student { get; set; }
    }
}
