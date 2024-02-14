using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace OPM.SFS.Web.Models
{
    public class InstitutionEditVM
    {
        public int InstitutionID { get; set; }
        public string Institution { get; set; }
        public int Type { get; set; }
        public SelectList TypeList { get; set; }
        public int? ParentInstitution { get; set; }
        public SelectList ParentInstititionList { get; set; }
        public string City { get; set; }
        public int State { get; set; }
        public SelectList StateList { get; set; }
        public string PostalCode { get; set; }
        public string HomePage { get; set; }
        public string ProgramPage { get; set; }
        public int AcademicSchedule { get; set; }
        public SelectList AcademicScheduleList { get; set; }
        public List<Contact> Contacts { get; set; }
        public bool IsAcceptingApplications { get; set; }
        public bool IsActive { get; set; }
        public int? GrantNumber { get; set; }
        public string GrantExpiration { get; set; }
        public bool ShowSuccessMessage { get; set; }
        public string SuccessMessage { get; set; }

        public class Contact
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public int Title { get; set; }
            public SelectList TitleList { get; set; }
            public string Phone { get; set; }
            public string Email { get; set; }
            public int Type { get; set; }
            public SelectList TypeList { get; set; }
        }
    }
}
