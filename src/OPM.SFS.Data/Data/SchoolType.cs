using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPM.SFS.Data
{
    public class SchoolType
    {
        public SchoolType()
        {
            Students = new HashSet<Student>();
        }
        public int SchoolTypeID { get; set; }
        public string SchoolTypeName { get; set; }

        public virtual ICollection<Student> Students { get; set; }
    }
}
