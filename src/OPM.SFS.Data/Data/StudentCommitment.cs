using System;
using System.Collections.Generic;

#nullable disable

namespace OPM.SFS.Data
{
    
    public partial class StudentCommitment
    {
        public StudentCommitment()
        {
            CommitmentStudentDocuments = new HashSet<CommitmentStudentDocument>();
        }
        public int StudentCommitmentId { get; set; }
        public int StudentId { get; set; }
        public int? AgencyId { get; set; }
        public int? CommitmentTypeId { get; set; }
        public int? AddressId { get; set; }
        public int? SupervisorContactId { get; set; }
        public int? MentorContactId { get; set; }
        public int? SalaryTypeId { get; set; }
        public string Justification { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? JobSearchTypeId { get; set; }
        public string JobTitle { get; set; }
        public decimal? SalaryMinimum { get; set; }
        public decimal? SalaryMaximum { get; set; }
        public string PayPlan { get; set; }
        public string Series { get; set; }
        public string Grade { get; set; }
        public int? CommitmentStatusId { get; set; }
        public int? LastUpdatedByAdminID { get; set; }
        public int? LastUpdatedByPIID { get; set; }
        public DateTime? DateInserted { get; set; }
        public string PIRecommendation { get; set; }
        public DateTime? DateSubmitted { get; set; }
        public DateTime? DateApproved { get; set; }
        public DateTime? LastModified { get; set; }
        public bool IsDeleted { get; set; }
        public string RejectNote { get; set; }
        public virtual Address Address { get; set; }
        public virtual Agency Agency { get; set; }
        public virtual CommitmentType CommitmentType { get; set; }      
        public virtual Contact MentorContact { get; set; }        
        public virtual SalaryType SalaryType { get; set; }
        public virtual Student Student { get; set; }
        public virtual Contact SupervisorContact { get; set; }
        public virtual JobSearchType JobSearchType { get; set; }
        public virtual CommitmentStatus CommitmentStatus { get; set; }
        public virtual ICollection<CommitmentStudentDocument> CommitmentStudentDocuments { get; set; }
        public virtual EmploymentVerification EmploymentVerification { get; set; }

    }
}
