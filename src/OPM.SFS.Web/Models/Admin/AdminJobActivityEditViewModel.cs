using Microsoft.AspNetCore.Mvc.Rendering;

namespace OPM.SFS.Web.Models
{
    public class AdminJobActivityEditViewModel
    {
        public int StudentID { get; set; }
        public int JobActivityId { get; set; }
        public string Studentname { get; set; }
        public string University { get; set; }
        public string Degree { get; set; }
        public string Major { get; set; }
        public string GradDate { get; set; }
        public string DateApplied { get; set; }
        public string PositionTitle { get; set; }
        public string USAJControlNunber { get; set; }
        public int AgencyType { get; set; }
        public SelectList AgencyTypeList { get; set; }
        public string AgencyName { get; set; }
        public string DutyLocation { get; set; }
        public string ContactFirstname { get; set; }
        public string ContactLastname { get; set; }
        public string ContactEmailaddress { get; set; }
        public string Description { get; set; }
        public int Status { get; set; }
        public SelectList StatusList { get; set; }
        public string StatusOther { get; set; }
    }
}
