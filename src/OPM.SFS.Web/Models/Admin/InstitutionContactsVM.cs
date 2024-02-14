using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace OPM.SFS.Web.Models
{
    public class InstitutionContactsVM
    {
        public string Institution { get; set; }
        public int InstitutionID { get; set; }
        public int InstitutionContactID { get; set; }
        public List<Contact> Contacts { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Title { get; set; }
        public string Phone { get; set; }
        public string PhoneExt { get; set; }
        public string Email { get; set; }
        public int ContactType { get; set; }
        public SelectList ContactTypeList { get; set; }
        public bool ShowSuccessMessage { get; set; }
        public string SuccessMessage { get; set; }


        public class Contact
        {
            public int ContactID { get; set; }
            public string Name { get; set; }
            public string Phone { get; set; }
            public string Role { get; set; }
            public string Email { get; set; }
        }
    }
}
