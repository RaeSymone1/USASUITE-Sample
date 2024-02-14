using System;
using System.Collections.Generic;

#nullable disable

namespace OPM.SFS.Data
{
    public partial class Institution
    {
        public Institution()
        {
            StudentInstitutionFundings = new HashSet<StudentInstitutionFunding>();
            InstitutionContacts = new HashSet<InstitutionContact>();
        }

        public int InstitutionId { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public string City { get; set; }
        public int? StateId { get; set; }
        public string PostalCode { get; set; }
        public bool IsAcceptingApplications { get; set; }
        public string HomePage { get; set; }
        public string ProgramPage { get; set; }
        public int? InstitutionTypeId { get; set; }
        public int? ParentInstitutionID { get; set; }
        public int? AcademicScheduleId { get; set; }
        public int? GrantNumber { get; set; }
        public DateTime? GrantExpirationDate { get; set; }

        public virtual InstitutionType InstitutionType { get; set; }
        public virtual AcademicSchedule AcademicSchedule { get; set; }
        public virtual State State { get; set; }

        public virtual ICollection<StudentInstitutionFunding> StudentInstitutionFundings { get; set; }
        public virtual ICollection<InstitutionContact> InstitutionContacts { get; set; }
    }
}
