using OPM.SFS.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPM.SFS.Core.Data
{
    public class StudentFeature
    {
        public int StudentFeatureId { get; set; }
        public int StudentId { get; set; }
        public int FeatureId { get; set; }
        public bool IsEnabled { get; set; }
        public Feature Feature { get; set; }
    }
}
