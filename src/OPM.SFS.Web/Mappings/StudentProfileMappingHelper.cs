using Microsoft.AspNetCore.Mvc.Rendering;
using OPM.SFS.Core.DTO;
using OPM.SFS.Core.Shared;
using OPM.SFS.Data;
using OPM.SFS.Web.Models;
using OPM.SFS.Web.SharedCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPM.SFS.Web.Mappings
{
    public interface IStudentProfileMappingHelper
    {
        Task<AdminStudentProfileEditViewModel> GetViewModelFromDTO(StudentProfileDTO data, int studentId);
    }

    public class StudentProfileMappingHelper : IStudentProfileMappingHelper
    {
        
        private readonly ICryptoHelper _crypto;
        private readonly IReferenceDataRepository _repo;

        public StudentProfileMappingHelper(ICryptoHelper crypto, IReferenceDataRepository repo)
        {
            
            _crypto = crypto;
            _repo = repo;
        }

        public async Task<AdminStudentProfileEditViewModel> GetViewModelFromDTO(StudentProfileDTO data, int studentId)
        {
            var vm = new AdminStudentProfileEditViewModel();
            var degreeList = await _repo.GetDegreesAsync();
            var GlobalConfigSettings = await _repo.GetGlobalConfigurationsAsync();
            vm.StateList = new SelectList(await _repo.GetStatesAsync(), nameof(State.StateId), nameof(State.Name));
            vm.Institutions = new SelectList(await _repo.GetInstitutionsAsync(), nameof(Institution.InstitutionId), nameof(Institution.Name));
            vm.Degrees = new SelectList(degreeList, nameof(Degree.DegreeId), nameof(Degree.Name));
            vm.Disciplines = new SelectList(await DisciplineSortForNAAsync(), nameof(Discipline.DisciplineId), nameof(Discipline.Name));
            vm.SessionList = new SelectList(await _repo.GetSessionsAsync(), nameof(SessionList.Name), nameof(SessionList.Name));
            vm.YearList = new SelectList(await _repo.GetProgramYearsAsync(), nameof(ProgramYear.Name), nameof(ProgramYear.Name));
            vm.StudentID = studentId;
            vm.StudentUID = data.StudentUID;
            vm.Firstname = data.FirstName;
            vm.Middlename = data.MiddleName;
            vm.Lastname = data.LastName;
            vm.Suffix = data.Suffix;
            vm.DateOfBirth = data.DateOfBirth.ToShortDateString();
            var ssn = "000000000";
            if (!string.IsNullOrWhiteSpace(data.Ssn))
            {
                ssn = _crypto.Decrypt(data.Ssn, GlobalConfigSettings);
            }
            vm.Last4SSN = ssn.Substring(ssn.Length - 4);
            vm.Email = data.Email;
            vm.AlternateEmail = data.AlternateEmail;
            vm.Fundings = new();
            foreach (var f in data.StudentInstitionFundings)
			{
                var degreeName = degreeList.Where(m => m.DegreeId == f.DegreeId).Select(m => m.Name).FirstOrDefault();


                vm.Fundings.Add(new AdminStudentProfileEditViewModel.Funding() {
                    ID = f.StudentInstitutionFundingId,
                    SelectedCollege = f.InstitutionId,
                    SelectedDiscipline = f.MajorId,
                    SelectedDegree = f.DegreeId,
                    EnrolledSession = f.EnrolledSession,
                    EnrolledYear = f.EnrolledYear != null ? f.EnrolledYear : 0,
                    FundingEndSession = f.FundingEndSession,
                    FundingEndYear = f.FundingEndYear != null ? f.FundingEndYear : 0,
                    ExpectedGradDate = f.ExpectedGradDate.HasValue ? $"{f.ExpectedGradDate.Value.Month}/{f.ExpectedGradDate.Value.Year}" : "",
                    DateAvailIntern = f.InternshipAvailDate.HasValue ? $"{f.InternshipAvailDate.Value.Month}/{f.InternshipAvailDate.Value.Year}" : "",
                    DateAvailPostGrad = f.PostGradAvailDate.HasValue ? $"{f.PostGradAvailDate.Value.Month}/{f.PostGradAvailDate.Value.Year}" : "",
                    SelectedMinor = f.MinorId,
                    SelectedSecondDegreeMajor = f.SecondDegreeMajorId,
                    SelectedSecondDegreeMinor = f.SecondDegreeMinorId,
                    ShowSecondDegreeInfo = degreeName.Contains("/") ? true : false });
            
			}
            vm.Certificates = new();
            vm.ProfileStatusID = data.ProfileStatusID;
            var profileStatusList = await _repo.GetProfileStatusAsync();
			vm.ProfileStatusList = new SelectList(profileStatusList, nameof(ProfileStatus.ProfileStatusID), nameof(ProfileStatus.Name));
            var status = profileStatusList.Where(m => m.ProfileStatusID == data.ProfileStatusID).Select(m => m.Name).FirstOrDefault();
            vm.IsPendingRegistrationApproval = status == "Not Registered" || status == "Pending";
            vm.LoginGovID = data.LoginGovLinkID;

            var certs = await _repo.GetSecurityCertificationAsync();
            foreach (var c in certs)
            {
                if (data.StudentSecurityCertifications.Where(m => m.SecurityCertificationId == c.SecurityCertificationId).Any())
                {
                    vm.Certificates.Add(new AdminStudentProfileEditViewModel.Certificate()
                    {
                        ID = c.SecurityCertificationId,
                        Name = c.SecurityCertificationName,
                        Selected = true
                    });
                }
                else
                {
                    vm.Certificates.Add(new AdminStudentProfileEditViewModel.Certificate()
                    {
                        ID = c.SecurityCertificationId,
                        Name = c.SecurityCertificationName,
                        Selected = false
                    });
                }
            }


            if (data.CurrentAddress is not null)
            {
                vm.CurrAddress1 = data.CurrentAddress.LineOne;
                vm.CurrAddress2 = data.CurrentAddress.LineTwo;
                vm.CurrCity = data.CurrentAddress.City;
                vm.CurrStateID = data.CurrentAddress.StateId;
                vm.CurrPostalCode = data.CurrentAddress.PostalCode;
                vm.CurrCountry = data.CurrentAddress.Country;
                vm.CurrPhone = data.CurrentAddress.PhoneNumber;
                vm.CurrExtension = data.CurrentAddress.PhoneExtension;
                vm.CurrFax = data.CurrentAddress.Fax;
            }
            if (data.PermanentAddress is not null)
            {
                vm.PermAddress1 = data.PermanentAddress.LineOne;
                vm.PermAddress2 = data.PermanentAddress.LineTwo;
                vm.PermCity = data.PermanentAddress.City;
                vm.PermPostalCode = data.PermanentAddress.PostalCode;
                vm.PermCountry = data.PermanentAddress.Country;
                vm.PermStateID = data.PermanentAddress.StateId;
                vm.PermPhone = data.PermanentAddress.PhoneNumber;
                vm.PermExtension = data.PermanentAddress.PhoneExtension;
            }

            if (data.EmergencyContact is not null)
            {

                vm.ContactFirstname = data.EmergencyContact.FirstName;
                vm.ContactMiddlename = data.EmergencyContact.MiddleName;
                vm.ContactLastname = data.EmergencyContact.LastName;
                vm.ContactRelationship = data.EmergencyContact.Relationship;
                vm.ContactEmail = data.EmergencyContact.Email;
                vm.ContactPhone = data.EmergencyContact.Phone;
                vm.ContactExtension = data.EmergencyContact.PhoneExt;
            }

            return vm;
        } 
        
        public async Task<List<Discipline>> DisciplineSortForNAAsync()
        {
            var disciplines = await _repo.GetDisciplinesAsync();
            disciplines = disciplines.OrderBy(m => m.Name).ToList();
            var naIndex = disciplines.FindIndex(x => x.Name == "N/A");
            var naValue = disciplines[naIndex];
            disciplines.Insert(0, naValue);
            disciplines.RemoveAt(naIndex);
            return disciplines;

        }

    }
}
