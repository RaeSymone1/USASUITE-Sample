using System;
using System.Collections.Generic;

#nullable disable

namespace OPM.SFS.Data
{
    public partial class Education
    {
        public int EducationId { get; set; }
        public int StudentBuilderResumeId { get; set; }
        public string SchoolName { get; set; }
        public int? SchoolTypeID { get; set; }
        public string CityName { get; set; }
        public int? StateId { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string Degree { get; set; }
        public string DegreeOther { get; set; }
        public int? CompletionYear { get; set; }
        public string Gpa { get; set; }
        public string Gpamax { get; set; }
        public string TotalCredits { get; set; }
        public string CreditType { get; set; }
        public string CreditTypeOther { get; set; }
        public string Major { get; set; }
        public string Minor { get; set; }
        public string Honors { get; set; }
        public string UserId { get; set; }
        public virtual StudentBuilderResume StudentBuilderResume { get; set; }
        public virtual SchoolType SchoolType { get; set; }
        public virtual State State { get; set; }
    }
}
