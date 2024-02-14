using Microsoft.AspNetCore.Mvc.Rendering;

namespace OPM.SFS.Web.Models
{
    public class WorkExperienceViewModel
    {
        public int WorkExperienceID { get; set; }
        public string Employer { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string City { get; set; }
        public int State { get; set; }
        public string ZipCode { get; set; }
        public string Country { get; set; }
        public string Title { get; set; }
        public string Series { get; set; }
        public string Payplan { get; set; }
        public string Grade { get; set; }
        public string Salary { get; set; }
        public string HoursPerWeek { get; set; }
        public string Supervisor { get; set; }
        public string SupervisorPhone { get; set; }
        public string SupervisorPhoneExt { get; set; }
        public string Duties { get; set; }
        public string Start { get; set; }
        public string End { get; set; }
        public SelectList StateList { get; set; }
    }
}
