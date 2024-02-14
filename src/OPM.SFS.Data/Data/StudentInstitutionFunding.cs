using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPM.SFS.Data
{
    public partial class StudentInstitutionFunding
    {
        public int StudentInstitutionFundingId { get; set; }
        public int StudentId { get; set; }
        public int? InstitutionId { get; set; }
        public int? MajorId { get; set; }
        public int? DegreeId { get; set; }
        public int? MinorId { get; set; }
        public int? SecondDegreeMajorId { get; set; }
        public int? SecondDegreeMinorId { get; set; }
        public int? ExtensionTypeID { get; set; }
		public int? FollowUpTypeOptionID { get; set; }
        public int? ContractId { get; set; }
        public int? StatusID { get; set; }
		public DateTime? ExpectedGradDate { get; set; }
        public DateTime? InternshipAvailDate { get; set; }
        public DateTime? PostGradAvailDate { get; set; }
        public string EnrolledSession { get; set; }
        public int? EnrolledYear { get; set; }
        public decimal? FundingAmount { get; set; }
        public DateTime? InitialFundingDate { get; set; }
        public int? FundingEndYear { get; set; }
        public string FundingEndSession { get; set; }
        public string Notes { get; set; }
        public string TotalAcademicTerms { get; set; }
        public double? ServiceOwed { get; set; }
        public string InternshipReported { get; set; }
        public string PostGradReported { get; set; }
        public string InternshipAgencyType { get; set; }
        public string InternshipAgencyName { get; set; }
        public string InternshipSubAgencyName { get; set; }
        public DateTime? InternshipEOD { get; set; }
        public string AdditionalInternshipAgencyType { get; set; }
        public string AdditionalInternshipAgencyName { get; set; }
        public string AdditionalInternshipSubAgencyName { get; set; }
        public string AdditionalInternshipReportedWebsite { get; set; }
        public string PostGradAgencyType { get; set; }
        public string PostGradAgencyName { get; set; }
        public string PostGradSubAgencyName { get; set; }
        public string AdditionalPostGradAgencyType { get; set; }
        public string AdditionalPostGradAgencyName { get; set; }
        public string AdditionalPostGradSubAgencyName { get; set; }
        public string AdditionalPostGradReportedWebsite { get; set; }
        public DateTime? PostGradEOD { get; set; }
        public DateTime? PGEmploymentDueDate { get; set; }
        public DateTime? PGVerificationOneDueDate { get; set; }
        public DateTime? PGVerificationOneCompleteDate { get; set; }
        public DateTime? PGVerificationTwoDueDate { get; set; }
        public DateTime? PGVerificationTwoCompleteDate { get; set; }
        public DateTime? CommitmentPhaseComplete { get; set; }
		public DateTime? SOCVerificationComplete { get; set; }
		public DateTime? FollowUpDate { get; set; }
        public DateTime? DatePendingReleaseCollectionInfo { get; set; }
        public DateTime? DateReleasedCollectionPackage { get; set; }
        public string FollowUpAction { get; set; }
        public virtual Student Student { get; set; }
        public virtual Institution Institution { get; set; }
        public virtual Discipline Major { get; set; }
        public virtual Degree Degree { get; set; }
        public virtual Discipline Minor { get; set; }
        public DateTime? DateLeftPGEarly { get; set; }
        public DateTime? PostGradVerificationReminderSentDate { get; set; }
        public DateTime? ServiceObligationCompleteReminderSentDate { get; set; }
        public virtual Discipline SecondDegreeMajor { get; set; }
        public virtual Discipline SecondDegreeMinor { get; set; }
        public virtual ExtensionType ExtensionType { get; set; }
		public virtual FollowUpTypeOption FollowUpTypeOption { get; set; }
        public virtual StatusOption Status { get; set; }
        public virtual Contract Contract { get; set; }
		
	}
}
