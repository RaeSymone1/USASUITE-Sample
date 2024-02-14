using OPM.SFS.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPM.SFS.Data
{
    public class Feature
    {
        public Feature()
        {
            StudentFeatures = new HashSet<StudentFeature>();
            AdminFeatures = new HashSet<AdminFeature>();
            AcademiaUserFeatures = new HashSet<AcademiaUserFeature>();
            AgencyUserFeature = new HashSet<AgencyUserFeature>();
        }

        public int FeatureID { get; set; }
        public string Name { get; set; }
        public bool IsEnabledSiteWide { get; set; }
        public DateTime LastModified { get; set; }
        public virtual ICollection<StudentFeature> StudentFeatures { get; set; }
        public virtual ICollection<AdminFeature> AdminFeatures { get; set; }
        public virtual ICollection<AcademiaUserFeature> AcademiaUserFeatures { get; set; }
        public virtual ICollection<AgencyUserFeature> AgencyUserFeature { get; set; }
       
    }
}
