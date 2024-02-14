using System;
using System.Collections.Generic;

#nullable disable

namespace OPM.SFS.Data
{
    public partial class Race
    {
        public Race()
        {
            StudentRaces = new HashSet<StudentRace>();
        }

        public int RaceId { get; set; }
        public string RaceName { get; set; }

        public virtual ICollection<StudentRace> StudentRaces { get; set; }
    }
}
