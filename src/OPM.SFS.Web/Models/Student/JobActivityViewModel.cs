using System.Collections.Generic;

namespace OPM.SFS.Web.Models
{
    public class JobActivityViewModel
    {
        public string Studentname { get; set; }
        public string University { get; set; }
        public string DegreeMajor { get; set; }
        public string GradDate { get; set; }
        public bool ShowSuccessMessage { get; set; }
        public List<JobActivity> Items { get; set; }      
        
    }

    public class JobActivity
    {
        public int JobActivityId { get; set; }
        public int UserId { get; set; }
        public string DateApplied { get; set; }
        public string PositionTitle { get; set; }
        public string USAJOBSControlNum { get; set; }
        public string AgencyTypeName { get; set; }
        public string AgencyName { get; set; }
        public string DutyLocation { get; set; }
        public string ContactFirstName { get; set; }
        public string ContactLastName { get; set; }
        public string ContactEmail { get; set; }
        public string Description { get; set; }
        public string CurrentStatus { get; set; }
        public string Fullname { get; set; }
        public string LastUpdated { get; set; }
        public string SelectedAgencyType { get; set; }
    }
}
