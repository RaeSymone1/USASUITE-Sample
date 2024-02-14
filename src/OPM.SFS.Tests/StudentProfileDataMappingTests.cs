//using Bogus;
//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using Moq;
//using OPM.SFS.Core.DTO;
//using OPM.SFS.Core.Shared;
//using OPM.SFS.Data;
//using OPM.SFS.Web.Mappings;
//using OPM.SFS.Web.Models;
//using OPM.SFS.Web.SharedCode;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace OPM.SFS.Tests
//{
//    [TestClass]
//    public class StudentProfileDataMappingTests
//    {
//        private IReferenceDataRepository _repo;
//        private ICryptoHelper _crypto;


//        [TestMethod]
//        public void Test123()
//        {
//            var repoMock = new Mock<IReferenceDataRepository>();
//            var cryptoMock = new Mock<ICryptoHelper>();
//            var states = new List<Data.State>();
//            states.Add(new Data.State() { StateId = 1, Name = "Georgia" });
//            var degrees = new List<Data.Degree>();
//            degrees.Add(new Degree() { DegreeId = 1, Name = "Degree 1" });
//            degrees.Add(new Degree() { DegreeId = 2, Name = "Degree 2" });
//            degrees.Add(new Degree() { DegreeId = 3, Name = "Degree 3/Degree 1" });
//            degrees.Add(new Degree() { DegreeId = 4, Name = "Degree 4" });
//            degrees.Add(new Degree() { DegreeId = 5, Name = "Degree 5" });
//            repoMock.Setup(c => c.GetStatesAsync().Result).Returns(states);
//            repoMock.Setup(c => c.GetInstitutionsAsync().Result).Returns(new List<Data.Institution>());
//            repoMock.Setup(c => c.GetDegreesAsync().Result).Returns(degrees);
//            repoMock.Setup(c => c.GetDisciplinesAsync().Result).Returns(new List<Data.Discipline>());
//            cryptoMock.Setup(c => c.Decrypt(It.IsAny<string>(), It.IsAny<EncryptionKeys>())).Returns("1234567890");
//            repoMock.Setup(c => c.GetGlobalConfigurationsAsync().Result).Returns(SetGlobalConfigSettings());
//            repoMock.Setup(c => c.GetProfileStatusAsync().Result).Returns(new List<Data.ProfileStatus>());
//            repoMock.Setup(c => c.GetSecurityCertificationAsync().Result).Returns(new List<Data.SecurityCertification>());
//            repoMock.Setup(c => c.GetSessionsAsync().Result).Returns(new List<Data.SessionList>());
//            repoMock.Setup(c => c.GetProgramYearsAsync().Result).Returns(new List<Data.ProgramYear>());


//			_repo = repoMock.Object;
//            _crypto = cryptoMock.Object;

//            StudentProfileDTO testData = GenerateFakeData();

//            StudentProfileMappingHelper tester = new StudentProfileMappingHelper(_crypto, _repo);
//            var vm = tester.GetViewModelFromDTO(testData, 1).Result;

//            //verify post grad date formats
//            string correctDate = $"{testData.StudentInstitionFundings.FirstOrDefault().PostGradAvailDate.Value.Month}/{testData.StudentInstitionFundings.FirstOrDefault().PostGradAvailDate.Value.Year}";
//            Assert.IsTrue(vm.DateAvailPostGrad == correctDate);

//            correctDate = $"{testData.StudentInstitionFundings.FirstOrDefault().InternshipAvailDate.Value.Month}/{testData.StudentInstitionFundings.FirstOrDefault().InternshipAvailDate.Value.Year}";
//            Assert.IsTrue(vm.DateAvailIntern == correctDate);

//            correctDate = $"{testData.StudentInstitionFundings.FirstOrDefault().ExpectedGradDate.Value.Month}/{testData.StudentInstitionFundings.FirstOrDefault().ExpectedGradDate.Value.Year}";
//            Assert.IsTrue(vm.ExpectedGradDate == correctDate);
//        }

//        private StudentProfileDTO GenerateFakeData()
//        {
//            var institionId = 0;
//            var degreeId = 0;
//            var majorId = 0;
//            var sessions = new[] { "fall", "spring", "summer", "winter" };
//            var testFunding = new Faker<FundingDTO>()
//                .RuleFor(i => i.InstitutionId, f => institionId++)
//                .RuleFor(i => i.DegreeId, f => f.Random.Number(1, 5))
//                .RuleFor(i => i.MajorId, f => majorId++)
//                .RuleFor(i => i.EnrolledSession, f => f.PickRandom(sessions))
//                .RuleFor(i => i.EnrolledYear, f => f.Random.Number(2019, 2025))
//                .RuleFor(i => i.FundingEndSession, f => f.PickRandom(sessions))
//                .RuleFor(i => i.FundingEndYear, f => f.Random.Number(2019, 2025))
//                .RuleFor(c => c.InternshipAvailDate, f => f.Date.Future(yearsToGoForward: 2))
//                .RuleFor(c => c.PostGradAvailDate, f => f.Date.Future(yearsToGoForward: 2))
//                .RuleFor(i => i.ExpectedGradDate, f => f.Date.Between(DateTime.Today, DateTime.Now.AddYears(2)));

//            var secruityId = 0;
//            var testSecruityClearance = new Faker<SecurityClearanceDTO>()
//                .RuleFor(s => s.SecurityCertificationId, f => secruityId++)
//                .RuleFor(s => s.SecurityCertificationName, f => f.Lorem.Word());

//            var testAddress = new Faker<AddressDTO>()
//                .RuleFor(a => a.LineOne, f => f.Address.StreetAddress())
//                .RuleFor(a => a.City, f => f.Address.City())
//                .RuleFor(a => a.PostalCode, f => f.Address.ZipCode())
//                .RuleFor(a => a.Country, f => f.Address.Country())
//                .RuleFor(a => a.PhoneNumber, f => f.Phone.PhoneNumber());

//            var testContact = new Faker<ContactDTO>()
//                .RuleFor(c => c.FirstName, f => f.Name.FirstName())
//                .RuleFor(c => c.LastName, f => f.Name.LastName())
//                .RuleFor(c => c.Relationship, f => f.Lorem.Word())
//                .RuleFor(c => c.Phone, f => f.Phone.PhoneNumber())
//                .RuleFor(c => c.Email, f => f.Internet.Email());


//            var testStudentProfile = new Faker<StudentProfileDTO>()
//                .RuleFor(c => c.StudentUID, f => f.Random.Int(1, 100))
//                .RuleFor(c => c.FirstName, f => f.Name.FirstName())
//                .RuleFor(c => c.MiddleName, f => f.Name.FirstName())
//                .RuleFor(c => c.LastName, f => f.Name.LastName())
//                .RuleFor(i => i.DateOfBirth, f => f.Date.Past(yearsToGoBack: 20))
//                .RuleFor(i => i.Ssn, f => f.Lorem.Word())
//                .RuleFor(c => c.Email, f => f.Internet.Email())
//                .RuleFor(c => c.AlternateEmail, f => f.Internet.Email())
//                .RuleFor(c => c.ProfileStatusID, f => f.Random.Int(1, 10))
//                .RuleFor(c => c.LoginGovLinkID, f => f.Random.Guid().ToString())
//                .RuleFor(c => c.StudentInstitionFundings, f => testFunding.Generate(3).ToList())
//                .RuleFor(c => c.StudentSecurityCertifications, f => testSecruityClearance.Generate(4).ToList())
//                .RuleFor(c => c.CurrentAddress, f => testAddress.Generate(1).FirstOrDefault())
//                .RuleFor(c => c.PermanentAddress, f => testAddress.Generate(1).FirstOrDefault())
//                .RuleFor(c => c.EmergencyContact, f => testContact.Generate(1).FirstOrDefault());
               

//            var generatedProfile = testStudentProfile.Generate();
//            return generatedProfile;
//        }

//        private List<GlobalConfiguration> SetGlobalConfigSettings()
//        {
//            GlobalConfiguration encryptKey = new() { Key = "Salt", Value = "test", Type = "Encryption" };
//            GlobalConfiguration encryptPhrase = new() { Key = "Passphrase", Value = "test", Type = "Encryption" };
//            GlobalConfiguration encryptKeySize = new() { Key = "Keysize", Value = "0", Type = "Encryption" };
//            GlobalConfiguration encryptVect = new() { Key = "InitVect", Value = "test", Type = "Encryption" };
//            GlobalConfiguration encryptPasswordIt = new() { Key = "PasswordIterations", Value = "0", Type = "Encryption" };
//            List<GlobalConfiguration> globalConfigurations = new();
//            globalConfigurations.Add(encryptKey);
//            globalConfigurations.Add(encryptPhrase);
//            globalConfigurations.Add(encryptKeySize);
//            globalConfigurations.Add(encryptVect);
//            globalConfigurations.Add(encryptPasswordIt);
//            return globalConfigurations;
//        }

//    }
//}
