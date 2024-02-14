using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPM.SFS.Core.DTO
{
    public class StudentProfileDTO
    {
        public int StudentUID { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Suffix { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Ssn { get; set; }
        public string Email { get; set; }
        public string AlternateEmail { get; set; }
        public List<FundingDTO> StudentInstitionFundings { get; set; }
        public List<SecurityClearanceDTO> StudentSecurityCertifications { get; set; }        
        public int ProfileStatusID { get; set; }
        public string LoginGovLinkID { get; set; }       
        public AddressDTO CurrentAddress { get; set; }
        public AddressDTO PermanentAddress { get; set; }
        public ContactDTO EmergencyContact { get; set; }
       

    }

    public class FundingDTO
    {
        public int StudentInstitutionFundingId { get; set; }
        public int? InstitutionId { get; set; }
        public int? MajorId { get; set; }
        public int? DegreeId { get; set; }
        public int? MinorId { get; set; }
        public int? SecondDegreeMajorId { get; set; }
        public int? SecondDegreeMinorId { get; set; }
        public string EnrolledSession { get; set; }
        public int? EnrolledYear { get; set; }
        public string FundingEndSession { get; set; }
        public int? FundingEndYear { get; set; }
        public DateTime? ExpectedGradDate { get; set; }
        public DateTime? InternshipAvailDate { get; set; }
        public DateTime? PostGradAvailDate { get; set; }
        public string TotalAcademicTerms { get; set; }
        public DateTime? DateLeftPGEarly { get; set; }
    }

    public class SecurityClearanceDTO
    {
        public int SecurityCertificationId { get; set; }
        public string SecurityCertificationName { get; set; }
    }

    public class AddressDTO
    {
        public string LineOne { get; set; }
        public string LineTwo { get; set; }
        public string City { get; set; }
        public int StateId { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string PhoneNumber { get; set; }
        public string PhoneExtension { get; set; }
        public string Fax { get; set; }
     
    }

    public class ContactDTO
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Relationship { get; set; }
        public string Phone { get; set; }
        public string PhoneExt { get; set; }
        public string Email { get; set; }
    }
}
