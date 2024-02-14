using Microsoft.AspNetCore.Mvc.Rendering;

namespace OPM.SFS.Web.Models.Admin
{
    public class AdminNewStudentVM 
    {
        public string ID { get; set; }
        public string Institution { get; set; }
        public string EnrolledSession { get; set; }
        public string EnrolledYear { get; set; }
        public string FundingEndSession { get; set; }
        public string FundingEndYear { get; set; }
        public string ExpectedGradDate { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Citizenship { get; set; }
        public string Degree { get; set; }
        public string Major { get; set; }
        public string Minor { get; set; }
        public string SecondMajor { get; set; }
        public string SecondMinor { get; set; }
        public string Contract { get; set; }
	}
}
