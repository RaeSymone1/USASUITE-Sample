using System.Collections.Generic;

namespace OPM.SFS.Web.Models
{
    public class StudentDashboardViewModel
    {

        public string ID { get; set; }
        public string FundingID { get; set; }
        public string StudentID { get; set; }
        public string Studentname { get; set; }
        public string Institution { get; set; }
        public string InstitutionID { get; set; }

        public string SchoolType { get; set; }
        public string EnrolledSession { get; set; }
        public string FundEndDate { get; set; }
        public string GraduationDateMonth { get; set; }
        public string GraduationDateYear { get; set; }
        public string Status { get; set; }
        public string ProgramPhase { get; set; }
        public string EmailAddress { get; set; }
        public string Placeholder { get; set; } = "TBD";
        public string TotalAcademicTerms { get; set; }

        public string Degree { get; set; }
        public string Major { get; set; }
        public string Minor { get; set; }
        public string SecondDegreeMajor { get; set; }
        public string SecondDegreeMinor { get; set; }
        public string AcademicSchedule { get; set; }
        public string ServiceOwed { get; set; }
        public string Contract { get; set; }
        public string Registered { get; set; }
        public string DeferralAgreementReceived { get; set; }
        public string INReported { get; set; }
        public string PGReported { get; set; }
        public string PGEOD { get; set; }
        public string PGDaysBetween { get; set; }
        public string ExtensionTypes { get; set; }
        public string PGEmploymentDueDate { get; set; }
        public string DateLeftPGEarly { get; set; }
        public string ReleasePackageSent { get; set; }
        public string Citizenship { get; set; }
        public string LastUpdateReceived { get; set; }
        public string FollowupDate { get; set; }
        public string FollowupAction { get; set; }
        public string AltEmail { get; set; }
        public string EnrolledYear { get; set; }
        public string FundingEndSession { get; set; }
        public string FundingEndYear { get; set; }
        public string EnrolledSessionDisplay { get; set; }
        public string FundingEndDisplay { get; set; }
        public string StudentNote { get; set; }
        public string StatusOption { get; set; }       
        public string EditUrl { get; set; }
        public string INAgencyType { get; set; }
        public string INHDQAgencyName { get; set; }
        public string INSubAgencyName { get; set; }
        public string INEOD { get; set; }
        public string INReportedWebsite { get; set; }
        public string AddINAgencyType { get; set; }
        public string AddINHDQAgencyName { get; set; }
        public string AddINSubAgencyName { get; set; }
        public string AddINReportedWebsite { get; set; }
        public string PGAgencyType { get; set; }
        public string PGHDQAgencyName { get; set; }
        public string PGSubAgencyName { get; set; }
        public string PGReportedWebsite { get; set; }
        public string AddPGAgencyType { get; set; }
        public string AddPGHDQAgencyName { get; set; }
        public string AddPGSubAgencyName { get; set; }
        public string AddPGReportedWebsite { get; set; }
        public string FollowupActionType { get; set; }
        public string PGVerificationOneDue { get; set; }
        public string PGVerificationOneComplete { get; set; }
        public string PGVerificationTwoDue { get; set; }
        public string PGVerificationTwoComplete { get; set; }
        public string InstitutionGroup { get; set; }
        public string CommitmentPhaseComplete { get; set; }
        public string ReleasePackageDueDate { get; set; }
        public string Amount { get; set; }
        public string PGPlacementCategory { get; set; }
        public string RegistrationCode { get; set; }
        public string ProfileStatus { get; set; }
		public string SOCVerificationComplete { get; set; }
	}
}
