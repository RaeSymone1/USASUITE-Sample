using System.Collections.Generic;

namespace OPM.SFS.Web.Models
{
    public class BuilderResumeViewModel
    {
    
        public string FullName { get; set; }
        public string FullAddress { get; set; }
        public string Phone { get; set; }
        public string OtherPhone { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public string AltEmail { get; set; }
        public string Objective { get; set; }
        public string Coursework { get; set; }
        public string OtherQuals { get; set; }
        public string JobRelatedSkils { get; set; }
        public string Certs { get; set; }
        public string Awards { get; set; }
        public string Supplemental { get; set; }
        public string Honors { get; set; }
        public List<WorkExperience> WorkExperienceList { get; set; }
        public List<Education> EducationList { get; set; }
        public bool ShowSuccessMessage { get; set; }
    }

    public class WorkExperience
    {
        public int WorkExperienceId { get; set; }
        public string Employer { get; set; }
        public string EmploymentDate { get; set; }
        public string Title { get; set; }
    }

    public class Education
    {
        public int EducationId { get; set; }
        public string Degree { get; set; }
        public string Major { get; set; }
        public string School { get; set; }
    }

}
