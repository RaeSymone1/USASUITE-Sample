using Microsoft.AspNetCore.Mvc.Rendering;

namespace OPM.SFS.Web.Models
{
    public class EducationViewModel
    {
        public int EducationId { get; set; }
        public string SchoolName { get; set; }
        public SelectList SchoolTypeList { get; set; }
        public int SchoolType { get; set; }
        public string City { get; set; }
        public SelectList StateList { get; set; }
        public int State { get; set; }
        public string Zipcode { get; set; }
        public string Country { get; set; }
        public SelectList DegreeList { get; set; }
        public string Degree { get; set; }
        public string DegreeText { get; set; }
        public string Year { get; set; }
        public string GPA { get; set; }
        public string TotalPoints { get; set; }
        public string Credits { get; set; }
        public string CreditType { get; set; }
        public string CreditTypeOther { get; set; }
        public string Major { get; set; }
        public string Minor { get; set; }
        public string Honors { get; set; }
    }
}
