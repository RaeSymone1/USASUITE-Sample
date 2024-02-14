using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPM.SFS.Core.DTO
{
    public class StudentCommitmentDTO
    {       
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Ssn { get; set; }
        public int? AgencyTypeId { get; set; }
        public int? AgencyId { get; set; }
        public int? ParentAgencyId { get; set; }
        public int StudentId { get; set; }
        public int CommitmentId { get; set; }
        public string CommitmentApprovalWorkflowCode { get; set; }
        public string Justification { get; set; }
        public string JobTitle { get; set; }
        public int? CommitmentTypeId { get; set; }
        public int? SalaryTypeId { get; set; }        
        public string CommitmentStatusCode { get; set; }
        public string AgencyApprovalWorkflowCode { get; set; }
        public string CommitmentTypeCode { get; set; }
        public decimal? SalaryMinimum { get; set; }
        public decimal? SalaryMaximum { get; set; }
        public string PayPlan { get; set; }
        public string Series { get; set; }
        public string Grade { get; set; }
        public string AddressCity { get; set; }
        public int? AddressStateID { get; set; }
        public string AddressPostalCode { get; set; }
        public string AddressCountry { get; set; }
        public string SupervisorFirstName { get; set; }
        public string SupervisorLastName { get; set; }
        public string SupervisorEmailAddress { get; set; }
        public string SupervisorContactPhone { get; set; }      
        public string SupervisorPhoneExtension { get; set; }
        public string MentorFirstName { get; set; }
        public string MentorLastName { get; set; }
        public string MentorEmailAddress { get; set; }
        public string MentorPhone { get; set; }      
        public string MentorPhoneExtension { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? JobSearchTypeId { get; set; }
        public string StatusDisplay { get; set; }
        public string RejectNote { get; set; }
        public string PIRecommendation { get; set; }

    }

    public class INCommitmentDTO
    {
        public int CommitmentId { get; set; }
        public string Agency { get; set; }
        public string AgencyType { get; set; }
        public int? AgencyParentId { get; set; }
        public DateTime? INEOD { get; set; }
        public DateTime? DateApproved { get; set; }
        public DateTime? StartDate { get; set; }
        public string CommitmentType { get; set; }
    }

    public class PGCommitmentDTO
    {
        public int CommitmentId { get; set; }
        public string Agency { get; set; }
        public string AgencyType { get; set; }
        public int? AgencyParentId { get; set; }
        public DateTime? PGEOD { get; set; }
        public DateTime? DateApproved { get; set; }
        public DateTime? StartDate { get; set; }
        public string CommitmentType { get; set; }
    }
}
