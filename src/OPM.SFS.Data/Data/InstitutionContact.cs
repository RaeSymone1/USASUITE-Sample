using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPM.SFS.Data
{
    public class InstitutionContact
    {
        public int InstitutionContactId { get; set; }
        public int InstitutionId { get; set; }
        public int InstitutionContactTypeID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Title { get; set; }
        public string Phone { get; set; }
        public string PhoneExt { get; set; }
        public string Email { get; set; }
        public DateTime DateInserted { get; set; }
        public virtual InstitutionContactType InstitutionContactType { get; set; }

    }
}
