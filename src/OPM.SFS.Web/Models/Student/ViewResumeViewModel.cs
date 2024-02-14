using System.Collections.Generic;

namespace OPM.SFS.Web.Models
{
    public class ViewResumeViewModel
    {
        public string FullName { get; set; }
        public string FullAddress { get; set; }
        public string CityStateZip { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string AltEmail { get; set; }
        public string Objective { get; set; }
        public List<ViewWorkExperience> WorkExperience { get; set; }
        public List<ViewEduction> Education { get; set; }
        public string Courses { get; set; }
        public string JobRelatedSkills { get; set; }
        public string Certifications { get; set; }
        public string Honors { get; set; }
        public string Supplemental { get; set; }


    }

    public class ViewWorkExperience
    {
        public string Employer { get; set; }
        public string AddressLine { get; set; }
        public string CityStateZip { get; set; }
        public string Title { get; set; }
        public string Duties { get; set; }
        public string SupervisorFullName { get; set; }
        public string Dates { get; set; }
        public string Salary { get; set; }
        public string Hours { get; set; }
    }

    public class ViewEduction
    {
        public string School { get; set; }
        public string AddressLine { get; set; }
        public string Degree { get; set; }
        public string Major { get; set; }
        public string Minor { get; set; }
        public string Honors { get; set; }
        public string Year { get; set; }

    }
}
