using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPM.SFS.Data
{
    public class ProfileStatus
    {
        public int ProfileStatusID { get; set; }
        public string Name { get; set; }
        public bool IsDisabled { get; set; }
        public string Display { get; set; }
    }
}
