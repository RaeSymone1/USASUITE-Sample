using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPM.SFS.Data
{
    public  class Citizenship
    {
        public Citizenship()
        {
            Students = new HashSet<Student>();
        }
        public int CitizenshipID { get; set; }
        public string Value { get; set; }
        public virtual ICollection<Student> Students { get; set; }
    }
}
