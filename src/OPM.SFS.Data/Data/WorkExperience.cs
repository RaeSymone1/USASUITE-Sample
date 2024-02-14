using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace OPM.SFS.Data
{
    public partial class WorkExperience
    {
        public int WorkExperienceId { get; set; }
        public int StudentBuilderResumeId { get; set; }
        public int? AddressId { get; set; }
        public string Employer { get; set; }
        public string Start { get; set; }
        public string End { get; set; }
        public string Title { get; set; }
        public string Series { get; set; }
        public string PayPlan { get; set; }
        public string Grade { get; set; }
        public string Salary { get; set; }
        public string HoursPerWeek { get; set; }
        public string SupervisorName { get; set; }
        public string SupervisorPhone { get; set; }
        public string SupervisorPhoneExt { get; set; }
        [Column(TypeName = "varchar(max)")]
        public string Duties { get; set; }
        public string UserId { get; set; }
        public virtual StudentBuilderResume StudentBuilderResume { get; set; }
        public virtual Address Address { get; set; }
    }
}
