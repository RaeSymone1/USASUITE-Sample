using System;
using System.Collections.Generic;

#nullable disable

namespace OPM.SFS.Data
{
    public partial class State
    {
        public State()
        {
            Addresses = new HashSet<Address>();
        }

        public int StateId { get; set; }
        public string Name { get; set; }
        public string Abbreviation { get; set; }
        public DateTime? DateInserted { get; set; }

        public virtual ICollection<Address> Addresses { get; set; }
    }
}
