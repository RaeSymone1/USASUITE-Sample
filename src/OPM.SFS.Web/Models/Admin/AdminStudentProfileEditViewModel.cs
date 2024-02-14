using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace OPM.SFS.Web.Models
{
    public class AdminStudentProfileEditViewModel
    {
        public int StudentID { get; set; }
        public int StudentUID { get; set; }
        public string Firstname { get; set; }
        public string Middlename { get; set; }
        public string Lastname { get; set; }
        public string Suffix { get; set; }
        public string DateOfBirth { get; set; }
        public string Last4SSN { get; set; }
        public string Email { get; set; }
        public string AlternateEmail { get; set; }
        public string University { get; set; }
    
        public string TotalAcademicTerms { get; set; }
        public string DateLeftPGEarly { get; set; }
        public string Discipline { get; set; }
        public string DegreeProgram { get; set; }
        public string SecurityClearance { get; set; }
        public string CurrAddress1 { get; set; }
        public string CurrAddress2 { get; set; }
        public string CurrCity { get; set; }
        public int CurrStateID { get; set; }
        public bool CurrOmitState { get; set; }
        public string CurrPostalCode { get; set; }
        public string CurrCountry { get; set; }
        public string CurrPhone { get; set; }
        public string CurrExtension { get; set; }
        public string CurrFax { get; set; }
        public string CurrOtherPhone { get; set; }
        public string CurrOtherExtension { get; set; }
        public bool CurrUseCurretAddressAsPerm { get; set; }
        public string PermAddress1 { get; set; }
        public string PermAddress2 { get; set; }
        public string PermCity { get; set; }
        public int PermStateID { get; set; }
        public bool PermOmitState { get; set; }
        public string PermPostalCode { get; set; }
        public string PermCountry { get; set; }
        public string PermPhone { get; set; }
        public string PermExtension { get; set; }
        public string PermFax { get; set; }
        public string PermOtherPhone { get; set; }
        public string PermOtherExtension { get; set; }
        public bool PermUseCurretAddressAsPerm { get; set; }
        public string ContactFirstname { get; set; }
        public string ContactMiddlename { get; set; }
        public string ContactLastname { get; set; }
        public string ContactRelationship { get; set; }
        public string ContactEmail { get; set; }
        public string ContactPhone { get; set; }
        public string ContactExtension { get; set; }
        public List<int> SelectedCertificates { get; set; }
        public List<Certificate> Certificates { get; set; }
        public List<Funding> Fundings { get; set; }
        public SelectList StateList { get; set; }
        public SelectList YearList { get; set; }
        public SelectList SessionList { get; set; }
        public SelectList ProfileStatusList { get; set; }
        public SelectList Institutions { get; set; }
        public SelectList Disciplines { get; set; }
        public SelectList Degrees { get; set; }
        public int ProfileStatusID { get; set; }
        public bool IsPendingRegistrationApproval { get; set; }

        public string LoginGovID { get; set; }
        public bool ShowSuccessMessage { get; set; }
        public string SuccessMessage { get; set; }


        public class Certificate
        {
            public string Name { get; set; }
            public int ID { get; set; }
            public bool Selected { get; set; }
        }
		public class Funding
		{
            public int ID { get; set; }
			public int? SelectedCollege { get; set; }
			public int? SelectedDiscipline { get; set; }
			public int? SelectedDegree { get; set; }
			public string EnrolledSession { get; set; }
			public int? EnrolledYear { get; set; }
			public string FundingEndSession { get; set; }
			public int? FundingEndYear { get; set; }
			public string ExpectedGradDate { get; set; }
			public string DateAvailIntern { get; set; }
			public string DateAvailPostGrad { get; set; }
			public int? SelectedMinor { get; set; }
			public int? SelectedSecondDegreeMajor { get; set; }
			public int? SelectedSecondDegreeMinor { get; set; }
			public bool ShowSecondDegreeInfo { get; set; }
		}
		public class SelectListOption
		{
			public string ID { get; set; }
			public string Value { get; set; }
		}
	}
}
