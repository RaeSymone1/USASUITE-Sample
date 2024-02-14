using OPM.SFS.Core.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using OPM.SFS.Data;
using System.Globalization;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using OPM.SFS.Web.Models.Academia;
using Azure.Core;
using OPM.SFS.Web.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc.Rendering;
using OPM.SFS.Core.Shared;

namespace OPM.SFS.Web.SharedCode
{
    public interface IAcademiaRepository
    {
        Task<AcademiaUser> GetExistingAcademiaUsers(string email);
        Task AddNewAcademiaUser(AcademiaRegistrationViewModel data);
        Task<StudentJobActivityViewModel> GetStudentJobActivityReport(int StudentID);
        Task<AcademiaStudentProfileViewModel> GetStudentProfile(int StudentID);


    }
    public class AcademiaRepository : IAcademiaRepository
    {
        private readonly ScholarshipForServiceContext _efDB;
        private readonly ICryptoHelper _crypto;
        private readonly ICacheHelper _cache;
		private readonly IReferenceDataRepository _repo;


		public AcademiaRepository(ScholarshipForServiceContext efDB, ICacheHelper cache, ICryptoHelper crypto, IReferenceDataRepository repo)
		{
			_efDB = efDB;
			_cache = cache;
			_crypto = crypto;
			_repo = repo;
		}
		public async Task<AcademiaUser> GetExistingAcademiaUsers(string email)
        {
          var EmailData = await _efDB.AcademiaUsers.Where(m => m.Email == email).FirstOrDefaultAsync();
           return EmailData;
        }

        public async Task AddNewAcademiaUser(AcademiaRegistrationViewModel request)
        {
            var allAccountStatus = await _cache.GetProfileStatusAsync();
            TextInfo textinfo = new CultureInfo("en-US", false).TextInfo;
            var pendingID = allAccountStatus.Where(m => m.Name == "Pending").Select(m => m.ProfileStatusID).FirstOrDefault();

            AcademiaUser newUser = new()
            {
                Firstname = textinfo.ToTitleCase(request.FirstName.ToLower()),
                Lastname = textinfo.ToTitleCase(request.LastName.ToLower()),
                AcademiaUserRoleID = request.Role,
                InstitutionID = request.Institution,
                Department = request.Department,
                Address = new Address()
                {
                    LineOne = request.AddressLineOne,
                    LineTwo = request.AddressLineTwo,
                    City = request.City,
                    StateId = request.State,
                    PostalCode = request.PostalCode,
                    PhoneNumber = request.Phone,
                    PhoneExtension = request.Extension,
                    Fax = request.Fax
                },
                Email = request.Email,
                WebsiteUrl = request.Website,
                ProfileStatusID = pendingID
            };

            _efDB.AcademiaUsers.Add(newUser);
            await _efDB.SaveChangesAsync();
        }

        public async Task<StudentJobActivityViewModel> GetStudentJobActivityReport(int StudentID)
        {
            var data = await _efDB.Students.Where(m => m.StudentId == StudentID)
                .Include(m => m.StudentJobActivities)
                .ThenInclude(m => m.JobActivityStatus)
                .Include(m => m.StudentJobActivities)
                .ThenInclude(m => m.AgencyType)
                .Include(m => m.StudentInstitutionFundings).ThenInclude(m => m.Institution)
                .Include(m => m.StudentInstitutionFundings).ThenInclude(m => m.Degree)
                .FirstOrDefaultAsync();

            StudentJobActivityViewModel model = new();
            model.StudentID = StudentID;
            model.Studentname = $"{data.FirstName} {data.LastName}";
            model.University = data.StudentInstitutionFundings.FirstOrDefault().Institution.Name;
            model.DegreeMajor = data.StudentInstitutionFundings.FirstOrDefault().Degree.Name;
            model.GradDate = data.StudentInstitutionFundings.FirstOrDefault().ExpectedGradDate.HasValue ? $"{data.StudentInstitutionFundings.FirstOrDefault().ExpectedGradDate.Value.Month}/{data.StudentInstitutionFundings.FirstOrDefault().ExpectedGradDate.Value.Year}" : "";
            if (data.StudentJobActivities is not null && data.StudentJobActivities.Count > 0)
            {
                model.Items = new();
                foreach (var j in data.StudentJobActivities)
                {
                    model.Items.Add(new StudentJobActivityViewModel.JobActivity()
                    {
                        JobActivityId = j.StudentJobActivityId,
                        DateApplied = j.DateApplied.HasValue ? j.DateApplied.Value.ToShortDateString() : "",
                        PositionTitle = j.PositionTitle,
                        USAJOBSControlNum = j.UsajobscontrolNumber,
                        AgencyTypeName = j.AgencyType is not null ? j.AgencyType.Name : "",
                        AgencyName = j.Agency,
                        DutyLocation = j.DutyLocation,
                        ContactFullName = $"{j.ContactFirstName} {j.ContactLastName}",
                        ContactEmail = j.ContactEmail,
                        Description = j.Description,
                        StatusOther = j.StatusOther,
                        StatusName = j.JobActivityStatus.Name,
                        LastUpdated = j.LastUpdated.HasValue ? j.LastUpdated.Value.ToShortDateString() : null
                    });
                }
            }
            return model;
        }

        public async Task<AcademiaStudentProfileViewModel> GetStudentProfile(int StudentID)
        {
            var GlobalConfigSettings = await _cache.GetGlobalConfigurationsAsync();
            var data = await _efDB.Students.AsNoTracking()
                .Where(m => m.StudentId == StudentID)
                .Include(m => m.CurrentAddress)
                .Include(m => m.PermanentAddress)
                .Include(m => m.EmergencyContact)
                .Include(m => m.StudentSecurityCertifications)
                .Include(m => m.StudentInstitutionFundings).ThenInclude(m => m.Institution)
				.Include(m => m.StudentInstitutionFundings).ThenInclude(m => m.Degree)
                .Include(m => m.StudentInstitutionFundings).ThenInclude(m => m.Major)
                .Include(m => m.StudentInstitutionFundings).ThenInclude(m => m.Minor)
				.Include(m => m.ProfileStatus)
                .FirstOrDefaultAsync();
            var vm = new AcademiaStudentProfileViewModel();
            vm.SavedDocuments = new();
            vm.StateList = new SelectList(await _cache.GetStatesAsync(), nameof(State.StateId), nameof(State.Name));
			var degreeList = await _repo.GetDegreesAsync();
			vm.Fundings = new();
			if (data != null)
            {
                vm.StudentId = data.StudentId;
                vm.Firstname = data.FirstName;
                vm.Middlename = data.MiddleName;
                vm.Lastname = data.LastName;
                vm.ProfileStatus = data.ProfileStatus.Name;
                vm.Suffix = data.Suffix;
                vm.Email = data.Email;
                vm.AlternateEmail = data.AlternateEmail;

                if (data.StudentInstitutionFundings != null)
                {
					foreach (var f in data.StudentInstitutionFundings)
					{
						var degreeName = degreeList.Where(m => m.DegreeId == f.DegreeId).Select(m => m.Name).FirstOrDefault();


						vm.Fundings.Add(new AcademiaStudentProfileViewModel.Funding()
						{
							ID = f.StudentInstitutionFundingId,
							University = f.Institution.Name,
							Major = f.Major != null ? $"{f.Major.Name}" : "",
							DegreeProgram = f.Degree != null ? $"{f.Degree.Name}" : "",

							ExpectedGradDate = f.ExpectedGradDate.HasValue ? $"{f.ExpectedGradDate.Value.Month}/{f.ExpectedGradDate.Value.Year}" : "",
							DateAvailIntern = f.InternshipAvailDate.HasValue ? $"{f.InternshipAvailDate.Value.Month}/{f.InternshipAvailDate.Value.Year}" : "",
							DateAvailPostGrad = f.PostGradAvailDate.HasValue ? $"{f.PostGradAvailDate.Value.Month}/{f.PostGradAvailDate.Value.Year}" : "",
							Minor = f.Minor != null ? $"{f.Minor.Name}" : "",
							SecondDegreeMajor = f.SecondDegreeMajor != null ? $"{f.SecondDegreeMajor.Name}" : "",
							SecondDegreeMinor = f.SecondDegreeMinor != null ? $"{f.SecondDegreeMinor.Name}" : "",
							ShowSecondDegreeInfo = degreeName.Contains("/") ? true : false
						});

					}
				}

                vm.Certificates = new();
                var certs = await _cache.GetSecurityCertificationAsync();
                foreach (var c in certs)
                {
                    if (data.StudentSecurityCertifications.Where(m => m.SecurityCertificationId == c.SecurityCertificationId).Any())
                    {
                        vm.Certificates.Add(new AcademiaStudentProfileViewModel.Certificate()
                        {
                            ID = c.SecurityCertificationId,
                            Name = c.SecurityCertificationName,
                            Selected = true
                        });
                    }
                    else
                    {
                        vm.Certificates.Add(new AcademiaStudentProfileViewModel.Certificate()
                        {
                            ID = c.SecurityCertificationId,
                            Name = c.SecurityCertificationName,
                            Selected = false
                        });
                    }
                }

                if (data.CurrentAddress != null)
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

                if (data.PermanentAddress != null)
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

                if (data.EmergencyContactId.HasValue && data.EmergencyContactId.Value > 0)
                {

                    vm.ContactFirstname = data.EmergencyContact.FirstName;
                    vm.ContactMiddlename = data.EmergencyContact.MiddleName;
                    vm.ContactLastname = data.EmergencyContact.LastName;
                    vm.ContactRelationship = data.EmergencyContact.Relationship;
                    vm.ContactEmail = data.EmergencyContact.Email;
                    vm.ContactPhone = data.EmergencyContact.Phone;
                    vm.ContactExtension = data.EmergencyContact.PhoneExt;
                }
            }

            var documents = await _efDB.StudentDocuments.Where(m => m.StudentId == StudentID && m.IsDeleted != true)
                .Where(m => !string.IsNullOrWhiteSpace(m.FilePath)) //exclude builder resumes
                .Select(m => new
                {
                    ID = m.StudentDocumentId,
                    Name = m.FileName,
                    Type = m.DocumentType.Name,
                    DateCreated = m.DateCreated.HasValue ? m.DateCreated.Value.ToShortDateString() : ""
                }).ToListAsync();

            foreach (var d in documents)
            {
                vm.SavedDocuments.Add(new AcademiaStudentProfileViewModel.Document()
                {
                    Id = d.ID,
                    Name = d.Name.Contains('_') ? d.Name.Substring(d.Name.IndexOf("_") + 1) : d.Name,
                    Type = d.Type,
                    DateCreated = d.DateCreated
                });
            }
            return vm;
        }
    }
}
