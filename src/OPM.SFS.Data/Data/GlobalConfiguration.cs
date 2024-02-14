using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPM.SFS.Data
{
    public class GlobalConfiguration
    {
        public int GlobalConfigurationID { get; set; }
        [StringLength(50)]
        public string Key { get; set; }
        [StringLength(150)]
        public string Value { get; set; }
        [StringLength(150)]
        public string Type { get; set; }
        public DateTime LastModified { get; set; }
    }
}
