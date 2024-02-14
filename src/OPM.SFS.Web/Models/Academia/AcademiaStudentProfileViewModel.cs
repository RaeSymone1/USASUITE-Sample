using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace OPM.SFS.Web.Models.Academia
{
	public class AcademiaStudentProfileViewModel
	{
        public int StudentId { get; set; }
        public string Firstname { get; set; }
        public string Middlename { get; set; }
        public string Lastname { get; set; }
        public string ProfileStatus { get; set; }
        public string Suffix { get; set; }
        public string Email { get; set; }
        public string AlternateEmail { get; set; }
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
        public List<Document> SavedDocuments { get; set; }

        public class Document
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Type { get; set; }
            public string DateCreated { get; set; }
        }
		public class Funding
		{
			public int ID { get; set; }
			public string University { get; set; }
			public string Major { get; set; }
			public string DegreeProgram { get; set; }
			public string Minor { get; set; }
			public string SecondDegreeMajor { get; set; }
			public string SecondDegreeMinor { get; set; }
			public string ExpectedGradDate { get; set; }
			public string DateAvailIntern { get; set; }
			public string DateAvailPostGrad { get; set; }
			public bool ShowSecondDegreeInfo { get; set; }
		}
		public class Certificate
        {
            public string Name { get; set; }
            public int ID { get; set; }
            public bool Selected { get; set; }
        }
    }  

  
}

