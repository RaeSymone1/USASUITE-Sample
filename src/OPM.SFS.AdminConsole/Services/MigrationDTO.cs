using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPM.SFS.AdminConsole.Services
{
    public record OldInstitutionType
    {
        public string InstitutionType { get; init; } = default!;

    }

    public record OldSecurityCertification
    {
        public string SecurityCertificationCode { get; set; }
        public string SecurityCertificationName { get; set; }
    }

    public record OldStudentCerts
    {
        public int nUserKeyID { get; set; }
        public string SecurityCertificationCode { get; set; }
    }

    public record OldState
    {
        public string Abbreviation { get; init; } = default!;
        public string Name { get; init; } = default!;
    }

    public record OldInstitutions
    {
        public int InstitutionID { get; set; }
        public string Name { get; set; }
        public short? SortOrder { get; set; }
        public bool? IsActive { get; set; }
        public int? ParentInstitutionID { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string HomePage { get; set; }
        public string SFSProgramPage { get; set; }
        public short? InstitutionTypeID { get; set; }
        public string InstitutionType { get; set; }
        public string ParentInstitution { get; set; }
        public string AcademicSchedule { get; set; }

    }

    public record OldInstitutionContact
    {
        public int PIContactID { get; set; }
        public int PIContactTypeID { get; set; }
        public string Name { get; set; }
        public string PIContactType { get; set; }
        public string ContactFirstName { get; set; }
        public string ContactLastName { get; set; }
        public string ContactTitle { get; set; }
        public string  ContactEmail { get; set; }
        public string ContactPhone { get; set; }
        public string ContactPhoneExt { get; set; }
        public DateTime? DateInserted { get; set; }

    }

    public record OldRace
    {
        public string Race { get; set; }
    }

    public record OldEthnicity
    {
        public string Ethnicity { get; set; }
    }

    public record OldGender
    {
        public string Gender { get; set; }
    }

    public record OldDegreeCodes
    {
        public string cDegreeCode { get; set; }
        public string cDegreeName { get; set; }
    }

    public record OldDisciplineCodes
    {
        public string cDisciplineCode { get; set; }
        public string cDisciplineName { get; set; }
    }

    public class OldStudents
    {
        public int nUserKeyID { get; set; }
        public string dDOB { get; set; }
        public string cCurrentPhone { get; set; }
        public string cCurrentPhoneExt { get; set; }
        public string cFaxPhone { get; set; }
        public string cOtherPhone { get; set; }
        public string cOtherPhoneExt { get; set; }
        public string cSchoolCode { get; set; }
        public string cEmail { get; set; }
        public string cEmailAlt { get; set; }
        public string cDegreeCode { get; set; }
        public string cDisciplineCode { get; set; }
        public string cGraduationDate { get; set; }
        public string cAvailableIntern { get; set; }
        public string PostGradAvailableDate { get; set; }
        public string dDAF { get; set; }
        public string dUpdated { get; set; }
        public string cStrongPassword { get; set; }
        public string cStrongSSN { get; set; }
        public string InstitutionName { get; set; }

        public string InitialFundingDate { get; set; }
        public string InstitutionID { get; set; }

        public string CitizenStatusLookupId { get; set; }
        public string FundingEndDate { get; set; }
        public string cEnrolledSession { get; set; }
        public string cFundingEndYear { get; set; }
        public string cFundingSession { get; set; }
        public string cStatus { get; set; }
        public string cPhase { get; set; }

        public string cContract { get; set; }
        public string cNotes { get; set; }
        public string cUserID { get; set; }
        public string cLastName { get; set; }
        public string cFirstName { get; set; }
        public string cMidName { get; set; }
        public string cDisabled { get; set; }
        public string AccountStatus { get; set; }
        public string cSuffix { get; set; }
        public string dLockoutDate { get; set; }
        public string nFailedLoginCount { get; set; }
        public string dFailedLoginDate { get; set; }
        public string dLastLoginDate { get; set; }
        public string cStrongMotherName { get; set; }
        public DateTime? StrongPasswordDate { get; set; }
        public string ROBExpirationDate { get; set; }
        public string FailedSecurityQuestionDate { get; set; }
        public string ProfileStatus { get; set; }

    }

    public class OldAddress
    {
        public string nUserKeyID { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Country { get; set; }
        public string PhoneNumber { get; set; }
        public string PhoneExtension { get; set; }
        public string FaxNumber { get; set; }
        public string AddressType { get; set; }


    }

    public class OldContact
    {
        public string nUserKeyID { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string PhoneExtension { get; set; }
        public string Relationship { get; set; }

    }

    public class OldBackgroundInformation
    {
        public string nUserKeyID { get; set; }
        public string GenderID { get; set; }
        public string EthnicityID { get; set; }
        public string RaceID { get; set; }
        public string HSGEDStateID { get; set; }
        public string SchoolTypeID { get; set; }

        public string IsMilitary { get; set; }
        public string YearsInSecurityPosition { get; set; }
        public string IsBackgroundDataCollected { get; set; }
        public string Gender { get; set; }
        public string Ethnicity { get; set; }
        public string SchoolType { get; set; }
    }

    public class OldPasswordHistory
    {
        public string nUserKeyID { get; set; }
        public string Password { get; set; }
        public DateTime PasswordDate { get; set; }
        public string cUserID { get; set; }

    }
        
    public class OldDocuments
    {
        public string nUserKeyID { get; set; }
        public string CommitmentID { get; set; }
        public string DocumentName { get; set; }
        public string DocumentPath { get; set; }
        public string IsDeleted { get; set; }
        public string DocumentTypeID { get; set; }
        public string DateCreated { get; set; }
        public string DocumentType { get; set; }
    }

    public class OldCommitments
    {
        public string nUserKeyID { get; set; }
        public string CommitmentID { get; set; }
        public string AgencyName { get; set; }
        public string JobTitle { get; set; }
        public string SalaryValue { get; set; }
        public string Series { get; set; }
        public string Grade { get; set; }
        public string PayPlan { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal? MinimumSalary { get; set; }
        public decimal? MaximumSalary { get; set; }
        public string JobSearchValue { get; set; }
        public string TentativeJobOfferDocumentId { get; set; }
        public string FinalJobOfferDocumentId { get; set; }
        public string PositionDescriptionDocumentId { get; set; }
        public string EVFDocumentId { get; set; }
        public string ApprovalValue { get; set; }
        public string LastApprovedByAccountId { get; set; }
        public string Justification { get; set; }
        public string PIRecommendation { get; set; }
        public DateTime? DateInserted { get; set; }
        public DateTime? DateSubmitted { get; set; }
        public DateTime? LastApprovedStatusModifiedDate { get; set; }
        public string EVFDocument { get; set; }
        public string PositionDescriptionDocument { get; set; }
        public string TenativeDocument { get; set; }
        public string FinalJobDocument { get; set; }
        public string LastUpdateUserID { get; set; }
        public string LastUpdateUserType { get; set; }
        public int? AddressId { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string Country { get; set; }
        public string PhoneNumber { get; set; }
        public string PhoneExtension { get; set; }
        public string FaxNumber { get; set; }
        public int? MentorStudentContactId { get; set; }
        public string MentorFirstName { get; set; }
        public string MentorLastName { get; set; }
        public string MentorEmail { get; set; }
        public string MentorPhone { get; set; }
        public string MentorPhoneExt { get; set; }
        public int? ManagerStudentContactId { get; set; }
        public string ManagerFirstName { get; set; }
        public string ManagerLastName { get; set; }
        public string ManagerEmail { get; set; }
        public string ManagerPhone { get; set; }
        public string ManagerPhoneExt { get; set; }
        public int? CommitmentLookupId { get; set; }
        public string CommitmentType { get; set; }

    }

    public class OldResume
    {        
        public string nUserKeyID { get; set; }
        public string dDAF { get; set; }
        public string dUpdated { get; set; }
        public string cCoursework { get; set; }
        public string cJobSkills { get; set; }
        public string cSupplemental { get; set; }
        public string cCertificates { get; set; }
        public string cHonors { get; set; }
        public string cObjective { get; set; }

    }

    public class OldJobActivity
    {
        public int nActivityReportID { get; set; }
        public int nUserKeyID { get; set; }
        public DateTime? dDate { get; set; }
        public string cJob { get; set; }
        public string cAgencyName { get; set; }
        public string cContactFirstName { get; set; }
        public string cContactLastName { get; set; }
        public string cContactPhoneNumber { get; set; }
        public string cContactEmail { get; set; }
        public string cDescription { get; set; }
        public string cStatus { get; set; }
        public int? nEnteredBy { get; set; }
        public DateTime? dEntered { get; set; }
        public int? nEditedBy { get; set; }
        public DateTime? dEdited { get; set; }
        public string cDutyLocation { get; set; }
        public int? nAgencyTypeID { get; set; }
        public long? nUSAJOBSControlNumber { get; set; }
        public string AgencyType { get; set; }

    }

    public class OldWorkExperienct
    {
        public string nUserKeyID { get; set; }
        public string cEmployer { get; set; }
        public string cAddress1 { get; set; }
        public string cAddress2 { get; set; }
        public string cAddress3 { get; set; }
        public string cCity { get; set; }
        public string cState { get; set; }
        public string cZip { get; set; }
        public string cCountry { get; set; }
        public string cFromDate { get; set; }
        public string cToDate { get; set; }
        public string cPosTitle { get; set; }
        public string cSeries { get; set; }
        public string cPayPlan { get; set; }
        public string cGrade { get; set; }
        public string cSalary { get; set; }
        public string cSupName { get; set; }
        public string cSupPhone { get; set; }
        public string cSupPhoneExt { get; set; }
        public string cHoursPerWeek { get; set; }
        public string cDuties { get; set; }
        public string nEmRecID { get; set; }
      
    }

    public class OldEducation
    {
        public string nUserKeyID { get; set; }
        public string cSchoolName { get; set; }
        public string cCity { get; set; }
        public string cZip { get; set; }
        public string cCountry { get; set; }
        public string cDegree { get; set; }
        public string cGPA { get; set; }
        public string cOutOf { get; set; }
        public string cMajor { get; set; }
        public string cMinor { get; set; }
        public string cYear { get; set; }
        public string cCreditsEarned { get; set; }
        public string cCreditType { get; set; }
        public string cHonors { get; set; }
        public string cState { get; set; }
        public string SchoolType { get; set; }

    }

    public class OldStudentRace
    {
        public string nUserKeyID { get; set; }
        public string Race { get; set; }
        public string DateInserted { get; set; }
    }

    public class OldStudentStatus
    {
        public string  Type { get; set; }
        public string Status { get; set; }
        public string  Phase { get; set; }
    }

    public class OldSchoolType
    {
        public string SchoolType { get; set; }
    }

    public class OldLookup
    {
        public string Code { get; set; }
        public string Value { get; set; }
        public string DisplayValue { get; set; }
        public string Description { get; set; }
        public string InsertedDate { get; set; }
    }
    public class OldEmailTemplate
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }

    public class OldRegistrationCode
    {
        public string QuarterName { get; set; }
        public string Code { get; set; }
        public DateTime QuarterStartDate { get; set; }
    }

    public class OldAgency
    {
        public int AgencyId { get; set; }
        public int AgencyLookupId { get; set; }
        public int? ParentAgencyId { get; set; }
        public string AgencyName { get; set; }
        public int AddressId { get; set; }
        public bool? HasPayplanSeriesGrade { get; set; }
        public bool IsAgencyPIVEnabled { get; set; }
        public string PIVAgencyCode { get; set; }
        public string PIVOrganizationIdentifier { get; set; }
        public DateTime? InsertedDate { get; set; }
        public string AgencyCode { get; set; }
        public bool? IsDisabled { get; set; }
        public string State { get; set; }
        public string StateId { get; set; }
        public string AgencyTypeCode { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string City { get; set; }       
        public string County { get; set; }
        public string Country { get; set; }
        public string Zipcode { get; set; }
        public string ZipExt { get; set; }
        public string ParentAgency { get; set; }
    }

    public class OldAgencyUserRole
    {
        public string AgencyOfficialRole { get; set; }
    }

    public class OldPIUserRole
    {
        public string PrincipalInvestigatorRole { get; set; }
    }

    public class OldAgencyUsers
    {
        public int nUserKeyid { get; set; }
        public string cUserID { get; set; }
        public string cLastName { get; set; }
        public string cFirstName { get; set; }
        public string cEmail { get; set; }
        public string cStrongPassword { get; set; }
        public DateTime? StrongPasswordDate { get; set; }
        public string cAddress1 { get; set; }
        public string cAddress2 { get; set; }
        public string cCity { get; set; }
        public string cState { get; set; }
        public string cZip { get; set; }
        public string cCountry { get; set; }
        public string cPhone { get; set; }
        public string cPhoneExt { get; set; }
        public string cFax { get; set; }
        public string cWebAdr { get; set; }
        public bool cDisplayContactInfo { get; set; }
        public string cComments { get; set; }
        public DateTime? dDAF { get; set; }
        public DateTime? dUpdated { get; set; }
        public string AgencyOfficialRole { get; set; }
        public string AgencyName { get; set; }
        public bool? IsDisabled { get; set; }
        public DateTime? dFailedLoginDate { get; set; }
        public DateTime? dLastLoginDate { get; set; }
        public DateTime? dLockoutDate { get; set; }
        public int? nFailedLoginCount { get; set; }
        public string cStatus { get; set; }

    }

    public class OldPIUsers
    {
        public int nUserKeyid { get; set; }
        public string cUserID { get; set; }
        public string cEmail { get; set; }
        public string cFirstName { get; set; }
        public string cLastName { get; set; }
        public string cDepartment { get; set; }
        public string cStrongPassword { get; set; }
        public DateTime? StrongPasswordDate { get; set; }
        public string cAddress1 { get; set; }
        public string cAddress2 { get; set; }
        public string cCity { get; set; }
        public string cState { get; set; }
        public string cZip { get; set; }
        public string cCountry { get; set; }
        public string cPhone { get; set; }
        public string cPhoneExt { get; set; }
        public string cFax { get; set; }
        public string cWebAdr { get; set; }
        public DateTime? dDAF { get; set; }
        public DateTime? dUpdated { get; set; }
        public string PrincipalInvestigatorRole { get; set; }
        public string Institution { get; set; }
        public bool? IsDisabled { get; set; }
        public DateTime? dFailedLoginDate { get; set; }
        public DateTime? dLastLoginDate { get; set; }
        public DateTime? dLockoutDate { get; set; }
        public int? nFailedLoginCount { get; set; }
        public string cStatus { get; set; }

    }

    public class OldAdminUsers
    {
        public string cUserID { get; set; }
        public string EmailAddress { get; set; }
        public string Phone { get; set; }
        public string PhoneExt { get; set; }
        public string Fax { get; set; }
        public string cFirstName { get; set; }
        public string cLastName { get; set; }
        public DateTime? Updated { get; set; }
        public string cStrongPassword { get; set; }
        public DateTime? cStrongPasswordDate { get; set; }
        public int? nFailedLoginCount { get; set; }
        public DateTime? dLastLoginDate { get; set; }
        public DateTime? dLockoutDate { get; set; }
   
    }

}
