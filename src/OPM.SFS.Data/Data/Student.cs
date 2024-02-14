using System;
using System.Collections.Generic;

#nullable disable

namespace OPM.SFS.Data
{
    public partial class Student
    {
        public Student()
        {
            StudentBuilderResumes = new HashSet<StudentBuilderResume>();
            StudentCommitments = new HashSet<StudentCommitment>();
            StudentRaces = new HashSet<StudentRace>();
            StudentDocuments = new HashSet<StudentDocument>();
            StudentSecurityCertifications = new HashSet<StudentSecurityCertification>();
            StudentJobActivities = new HashSet<StudentJobActivity>();
            StudentInstitutionFundings = new HashSet<StudentInstitutionFunding>();
        }

        public int StudentId { get; set; }  
        public int StudentUID { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }        
        public string LastName { get; set; }
        public string Suffix { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int? CurrentAddressId { get; set; }
        public int? PermanentAddressId { get; set; }
        public string Ssn { get; set; }
        public string AlternateEmail { get; set; }
        public int? EmergencyContactId { get; set; }     
        public string UserId { get; set; }
        public string UserIp { get; set; }
        public int? CitizenStatusId { get; set; }  
        public int? EthnicityId { get; set; }
        public int? GenderId { get; set; }
        public string IsMilitary { get; set; }
        public int? YearsInSecurityPosition { get; set; }
        public DateTime? DateAdded { get; set; }
        public DateTime? LastUpdated { get; set; }        
        public string UpdatedByID { get; set; }
        public int? SchoolTypeID { get; set; }
        public int? HighSchoolStateStateId { get; set; }
        public int? CitizenshipOptionID { get; set; }
        public int? CitizenshipID { get; set; }
        public int ProfileStatusID { get; set; }
        public string LoginGovLinkID { get; set; }
        public virtual Address CurrentAddress { get; set; }
        public virtual Contact EmergencyContact { get; set; }
        public virtual Ethnicity Ethnicity { get; set; }
        public virtual Gender Gender { get; set; }
        public virtual Address PermanentAddress { get; set; }

        public virtual ProfileStatus ProfileStatus { get; set; }
		public DateTime? RegistrationApprovalDate { get; set; }
		public virtual ICollection<StudentBuilderResume> StudentBuilderResumes { get; set; }
        public virtual ICollection<StudentCommitment> StudentCommitments { get; set; }
        public virtual ICollection<StudentRace> StudentRaces { get; set; }
        public virtual ICollection<StudentSecurityCertification> StudentSecurityCertifications { get; set; }
        public virtual ICollection<StudentDocument> StudentDocuments { get; set; }
        public virtual ICollection<StudentJobActivity> StudentJobActivities { get; set; }
        public virtual StudentAccount StudentAccount { get; set; }      
        public virtual State HighSchoolState { get; set; }
        public virtual SchoolType SchoolType { get; set; }
        public virtual Citizenship Citizenship { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public DateTime? InactiveAccountReminderSentDate { get; set; }
        public DateTime? CompleteRegistrationReminderSentDate { get; set; }
        public virtual ICollection<StudentInstitutionFunding> StudentInstitutionFundings { get; set; }

    }
}
