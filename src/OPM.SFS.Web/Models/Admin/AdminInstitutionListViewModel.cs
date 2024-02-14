using System.Collections.Generic;

namespace OPM.SFS.Web.Models
{
    public class AdminInstitutionListViewModel
    {
        public List<InstiutionItem> Institutions { get; set; }

        public class InstiutionItem
        {
            public int InstitutionID { get; set; }
            public string Institution { get; set; }
            public string InstitutionType { get; set; }
            public int? GrantNumber { get; set; }
            public string GrantExpirationDate { get; set; }
            public int? ParentInstitutionID { get; set; }
            public string IsAcceptingApplicataions { get; set; }
            public string IsActive { get; set; }
            public List<Contact> Contacts { get; set; }

            public class Contact
            {
                public string Name { get; set; }
                public string Phone { get; set; }
                public string Role { get; set; }
                public string Email { get; set; }
            }
        }
        

    }
}
