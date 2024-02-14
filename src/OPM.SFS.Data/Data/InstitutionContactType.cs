using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPM.SFS.Data
{
    public class InstitutionContactType
    {
        public InstitutionContactType()
        {
            InstitutionContacts = new HashSet<InstitutionContact>();
        }
        public int InstitutionContactTypeID { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }

        public virtual ICollection<InstitutionContact> InstitutionContacts { get; set; }
    }
}
