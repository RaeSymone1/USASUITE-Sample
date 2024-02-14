using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPM.SFS.Core.DTO
{
    public class StudentDashboardDTO
    {
        public int StudentUID { get; set; }
        public int StudentID { get; set; }
        public int StudentInstitutionFundingId { get; set; }
        public string Firstname { get; set; }
        public string LastName { get; set; }
        public string Institution { get; set; }
        public string InstitutionID { get; set; }
        public string SchoolType { get; set; }
        public string AcademicSchedule { get; set; }
        public string EnrolledSession { get; set; }
        public string EnrolledYear { get; set; }
        public string FundingEndSession { get; set; }
        public string FundingEndYear { get; set; }
        public string FundingAmount { get; set; }
        public string GraduationDate { get; set; }

        public string TotalAcademicTerms { get; set; }
        public string DateLeftPGEarly { get; set; }
        public string StatusOption { get; set; }
        public string ProgramPhase { get; set; }
        public string PlacementCategory { get; set; }
        public string Status { get; set; }
        public string EmailAddress { get; set; }
        public string Degree { get; set; }
        public string Major { get; set; }
        public string Minor { get; set; }
        public string SecondDegreeMajor { get; set; }
        public string SecondDegreeMinor { get; set; }
        public string AltEmail { get; set; }
        public string ExtensionType { get; set; }
        public int ExtensionMonths { get; set; }
        public DateTime? PGEmploymentDueDate { get; set; }
        public string PGVerificationOneDueDate { get; set; }
        public string PGVerificationTwoDueDate { get; set; }
        public string PGVerificationOneCompleteDate { get; set; }
        public string PGVerificationTwoCompleteDate { get; set; }
        public string CommitmentPhaseComplete { get; set; }
        public string Citizenship { get; set; }
        public DateTime? LastUpdated { get; set; }
        public string Contract { get; set; }
        public string Notes { get; set; }
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
        public string ProfileStatus { get; set; }
        public string FollowUpDate { get; set;}
        public string FollowUpType { get; set; }
        public string FollowUpAction { get; set; }
        public string DatePendingReleaseCollectionInfo { get; set; }
        public string DateReleasedCollectionPackage { get; set; }
		public string SOCVerificationComplete { get; set; }
	}
}
