using Microsoft.AspNetCore.Mvc.Rendering;

namespace OPM.SFS.Web.Models.Academia
{
    public class AcademiaRegistrationViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Role { get; set; }
        public SelectList RoleList { get; set; }
        public int Institution { get; set; }
        public SelectList InstitutionList { get; set; }
        public string Department { get; set; }
        public string AddressLineOne { get; set; }
        public string AddressLineTwo { get; set; }
        public string City { get; set; }
        public int State { get; set; }
        public SelectList StateList { get; set; }
        public string PostalCode { get; set; }
        public string Phone { get; set; }
        public string Extension { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
        public bool ShowSuccessMessage { get; set; }
      
    }
}
