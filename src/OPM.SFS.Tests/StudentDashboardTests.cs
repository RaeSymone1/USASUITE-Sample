using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using OPM.SFS.Web.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using OPM.SFS.Web.SharedCode;
using OPM.SFS.Core.DTO;
using static OPM.SFS.Web.SharedCode.CacheHelper;
using OPM.SFS.Data;
using OPM.SFS.Core.Shared;

namespace OPM.SFS.Tests
{
    [TestClass]
    public class StudentDashboardTests
    {
        [TestMethod]
        public void CommitmentVerificationOne_Should_Be_NA_When_Date_Is_Null()
        {
            var serviceOwedMock = new Mock<IServiceOwedService>();
            var referenceRepoMock = new Mock<IReferenceDataRepository>();
            var studentRepoMock = new Mock<IStudentRepository>();
            var cryptoRepMock = new Mock<ICryptoHelper>();
            var utiltiesMock = new Mock<IUtilitiesService>();
            StudentDashboardService _service = new StudentDashboardService(serviceOwedMock.Object, referenceRepoMock.Object, studentRepoMock.Object, cryptoRepMock.Object, utiltiesMock.Object);
            var results = _service.CalculateCommitmentVerificationOne(null, null, "");
            Assert.IsTrue(results == "N/A");
        }

        [TestMethod]
        public void CommitmentVerificationOne_Is_Not_Null()
        {
            var serviceOwedMock = new Mock<IServiceOwedService>();
            var referenceRepoMock = new Mock<IReferenceDataRepository>();
            var studentRepoMock = new Mock<IStudentRepository>();
            var cryptoRepMock = new Mock<ICryptoHelper>();
			var utiltiesMock = new Mock<IUtilitiesService>();
			StudentDashboardService _service = new StudentDashboardService(serviceOwedMock.Object, referenceRepoMock.Object, studentRepoMock.Object, cryptoRepMock.Object, utiltiesMock.Object);
            DateTime EOD = DateTime.Today;
            string correctDate = EOD.AddYears(1).ToShortDateString();
            var results = _service.CalculateCommitmentVerificationOne(EOD, null, "");
            Assert.IsTrue(results == correctDate);
        }

        [TestMethod]
        public void CommitmentVerificationTwo_Should_Be_NA_When_Date_Is_Null()
        {
            var serviceOwedMock = new Mock<IServiceOwedService>();
            var referenceRepoMock = new Mock<IReferenceDataRepository>();
            var studentRepoMock = new Mock<IStudentRepository>();
            var cryptoRepMock = new Mock<ICryptoHelper>();
			var utiltiesMock = new Mock<IUtilitiesService>();
			StudentDashboardService _service = new StudentDashboardService(serviceOwedMock.Object, referenceRepoMock.Object, studentRepoMock.Object, cryptoRepMock.Object, utiltiesMock.Object);
            var results = _service.CalculateCommitmentVerificationTwo(null, null, "");
            Assert.IsTrue(results == "N/A");
        }

        [TestMethod]
        public void CommitmentVerificationTwo_Is_Not_Null()
        {
            var serviceOwedMock = new Mock<IServiceOwedService>();
            var referenceRepoMock = new Mock<IReferenceDataRepository>();
            var studentRepoMock = new Mock<IStudentRepository>();
            var cryptoRepMock = new Mock<ICryptoHelper>();
			var utiltiesMock = new Mock<IUtilitiesService>();
			StudentDashboardService _service = new StudentDashboardService(serviceOwedMock.Object, referenceRepoMock.Object, studentRepoMock.Object, cryptoRepMock.Object, utiltiesMock.Object);
            DateTime EOD = DateTime.Today;
            string correctDate = EOD.AddYears(2).ToShortDateString();
            var results = _service.CalculateCommitmentVerificationTwo(EOD, null, "");
            Assert.IsTrue(results == correctDate);
        }

        [TestMethod]
        public void CommitmentVerificationOne_Should_Be_Valid_Date_When_Date_Is_Override()
        {
            var serviceOwedMock = new Mock<IServiceOwedService>();
            var referenceRepoMock = new Mock<IReferenceDataRepository>();
            var studentRepoMock = new Mock<IStudentRepository>();
            var cryptoRepMock = new Mock<ICryptoHelper>();
			var utiltiesMock = new Mock<IUtilitiesService>();
			StudentDashboardService _service = new StudentDashboardService(serviceOwedMock.Object, referenceRepoMock.Object, studentRepoMock.Object, cryptoRepMock.Object, utiltiesMock.Object);
            string OverrideDate = "11/9/2023";
            var results = _service.CalculateCommitmentVerificationOne(null, OverrideDate, null);
            Assert.IsTrue(results == OverrideDate);
        }

        [TestMethod]
        public void CommitmentVerificationTwo_Should_Be_Valid_Date_When_Date_Is_Override()
        {
            var serviceOwedMock = new Mock<IServiceOwedService>();
            var referenceRepoMock = new Mock<IReferenceDataRepository>();
            var studentRepoMock = new Mock<IStudentRepository>();
            var cryptoRepMock = new Mock<ICryptoHelper>();
			var utiltiesMock = new Mock<IUtilitiesService>();
			StudentDashboardService _service = new StudentDashboardService(serviceOwedMock.Object, referenceRepoMock.Object, studentRepoMock.Object, cryptoRepMock.Object, utiltiesMock.Object);
            string OverrideDate = "11/9/2023";
            var results = _service.CalculateCommitmentVerificationTwo(null, OverrideDate, null);
            Assert.IsTrue(results == OverrideDate);
        }

        [TestMethod]
        public void CommitmentVerificationOne_Should_Be_NA_When_ServiceOwed_Is_One()
        {
            var serviceOwedMock = new Mock<IServiceOwedService>();
            var referenceRepoMock = new Mock<IReferenceDataRepository>();
            var studentRepoMock = new Mock<IStudentRepository>();
            var cryptoRepMock = new Mock<ICryptoHelper>();
			var utiltiesMock = new Mock<IUtilitiesService>();
			StudentDashboardService _service = new StudentDashboardService(serviceOwedMock.Object, referenceRepoMock.Object, studentRepoMock.Object, cryptoRepMock.Object, utiltiesMock.Object);
            string OverrideDate = null;
            string ServiceOwed = "1";
            var results = _service.CalculateCommitmentVerificationOne(null, OverrideDate, ServiceOwed);
            Assert.IsTrue(results == "N/A");
        }

        [TestMethod]
        public void CommitmentVerificationOne_Should_Be_Valid_Date_When_ServiceOwed_Is_One_Point_Five()
        {
            var serviceOwedMock = new Mock<IServiceOwedService>();
            var referenceRepoMock = new Mock<IReferenceDataRepository>();
            var studentRepoMock = new Mock<IStudentRepository>();
            var cryptoRepMock = new Mock<ICryptoHelper>();
			var utiltiesMock = new Mock<IUtilitiesService>();
			StudentDashboardService _service = new StudentDashboardService(serviceOwedMock.Object, referenceRepoMock.Object, studentRepoMock.Object, cryptoRepMock.Object, utiltiesMock.Object);
            string OverrideDate = null;
            string ServiceOwed = "1.5";
            DateTime EOD = DateTime.Today;
            string correctDate = EOD.AddYears(1).ToShortDateString();
            var results = _service.CalculateCommitmentVerificationOne(EOD, OverrideDate, ServiceOwed);
            Assert.IsTrue(results == correctDate);
        }

        [TestMethod]
        public void CommitmentVerificationOne_Should_Be_Valid_Date_When_ServiceOwed_Is_Two()
        {
            var serviceOwedMock = new Mock<IServiceOwedService>();
            var referenceRepoMock = new Mock<IReferenceDataRepository>();
            var studentRepoMock = new Mock<IStudentRepository>();
            var cryptoRepMock = new Mock<ICryptoHelper>();
			var utiltiesMock = new Mock<IUtilitiesService>();
			StudentDashboardService _service = new StudentDashboardService(serviceOwedMock.Object, referenceRepoMock.Object, studentRepoMock.Object, cryptoRepMock.Object, utiltiesMock.Object);
            string OverrideDate = null;
            string ServiceOwed = "2";
            DateTime EOD = DateTime.Today;
            string correctDate = EOD.AddYears(1).ToShortDateString();
            var results = _service.CalculateCommitmentVerificationOne(EOD, OverrideDate, ServiceOwed);
            Assert.IsTrue(results == correctDate);
        }


        [TestMethod]
        public void CommitmentVerificationOne_Should_Be_Valid_Date_When_ServiceOwed_Greater_Than_Two()
        {
            var serviceOwedMock = new Mock<IServiceOwedService>();
            var referenceRepoMock = new Mock<IReferenceDataRepository>();
            var studentRepoMock = new Mock<IStudentRepository>();
            var cryptoRepMock = new Mock<ICryptoHelper>();
			var utiltiesMock = new Mock<IUtilitiesService>();
			StudentDashboardService _service = new StudentDashboardService(serviceOwedMock.Object, referenceRepoMock.Object, studentRepoMock.Object, cryptoRepMock.Object, utiltiesMock.Object);
            string OverrideDate = null;
            string ServiceOwed = "2.5";
            DateTime EOD = DateTime.Today;
            string correctDate = EOD.AddYears(1).ToShortDateString();
            var results = _service.CalculateCommitmentVerificationOne(EOD, OverrideDate, ServiceOwed);
            Assert.IsTrue(results == correctDate);
        }

        [TestMethod]
        public void CommitmentVerificationTwo_Should_Be_NA_When_ServiceOwed_Is_One()
        {
            var serviceOwedMock = new Mock<IServiceOwedService>();
            var referenceRepoMock = new Mock<IReferenceDataRepository>();
            var studentRepoMock = new Mock<IStudentRepository>();
            var cryptoRepMock = new Mock<ICryptoHelper>();
			var utiltiesMock = new Mock<IUtilitiesService>();
			StudentDashboardService _service = new StudentDashboardService(serviceOwedMock.Object, referenceRepoMock.Object, studentRepoMock.Object, cryptoRepMock.Object, utiltiesMock.Object);
            string OverrideDate = null;
            string ServiceOwed = "1";
            var results = _service.CalculateCommitmentVerificationTwo(null, OverrideDate, ServiceOwed);
            Assert.IsTrue(results == "N/A");
        }

        [TestMethod]
        public void CommitmentVerificationTwo_Should_Be_NA_When_ServiceOwed_Is_One_Point_Five()
        {
            var serviceOwedMock = new Mock<IServiceOwedService>();
            var referenceRepoMock = new Mock<IReferenceDataRepository>();
            var studentRepoMock = new Mock<IStudentRepository>();
            var cryptoRepMock = new Mock<ICryptoHelper>();
			var utiltiesMock = new Mock<IUtilitiesService>();
			StudentDashboardService _service = new StudentDashboardService(serviceOwedMock.Object, referenceRepoMock.Object, studentRepoMock.Object, cryptoRepMock.Object, utiltiesMock.Object);
            string OverrideDate = null;
            string ServiceOwed = "1.5";
            var results = _service.CalculateCommitmentVerificationTwo(null, OverrideDate, ServiceOwed);
            Assert.IsTrue(results == "N/A");
        }

        [TestMethod]
        public void CommitmentVerificationTwo_Should_Be_NA_When_ServiceOwed_Is_Two()
        {
            var serviceOwedMock = new Mock<IServiceOwedService>();
            var referenceRepoMock = new Mock<IReferenceDataRepository>();
            var studentRepoMock = new Mock<IStudentRepository>();
            var cryptoRepMock = new Mock<ICryptoHelper>();
			var utiltiesMock = new Mock<IUtilitiesService>();
			StudentDashboardService _service = new StudentDashboardService(serviceOwedMock.Object, referenceRepoMock.Object, studentRepoMock.Object, cryptoRepMock.Object, utiltiesMock.Object);
            string OverrideDate = null;
            string ServiceOwed = "2";
            var results = _service.CalculateCommitmentVerificationTwo(null, OverrideDate, ServiceOwed);
            Assert.IsTrue(results == "N/A");
        }

        [TestMethod]
        public void CommitmentVerificationTwo_Should_Be_Valid_Date_When_ServiceOwed_Greater_Than_Two()
        {
            var serviceOwedMock = new Mock<IServiceOwedService>();
            var referenceRepoMock = new Mock<IReferenceDataRepository>();
            var studentRepoMock = new Mock<IStudentRepository>();
            var cryptoRepMock = new Mock<ICryptoHelper>();
			var utiltiesMock = new Mock<IUtilitiesService>();
			StudentDashboardService _service = new StudentDashboardService(serviceOwedMock.Object, referenceRepoMock.Object, studentRepoMock.Object, cryptoRepMock.Object, utiltiesMock.Object);
            string OverrideDate = null;
            string ServiceOwed = "2.5";
            DateTime EOD = DateTime.Today;
            string correctDate = EOD.AddYears(2).ToShortDateString();
            var results = _service.CalculateCommitmentVerificationTwo(EOD, OverrideDate, ServiceOwed);
            Assert.IsTrue(results == correctDate);
        }


        [TestMethod]
        public void CommitmentPhaseComplete_Should_Be_NA_When_Date_Is_Null()
        {
            var serviceOwedMock = new Mock<IServiceOwedService>();
            var referenceRepoMock = new Mock<IReferenceDataRepository>();
            var studentRepoMock = new Mock<IStudentRepository>();
            var cryptoRepMock = new Mock<ICryptoHelper>();
			var utiltiesMock = new Mock<IUtilitiesService>();
			StudentDashboardService _service = new StudentDashboardService(serviceOwedMock.Object, referenceRepoMock.Object, studentRepoMock.Object, cryptoRepMock.Object, utiltiesMock.Object);
            var results = _service.CalculateCommitmentPhaseComplete(null, 0);
            Assert.IsTrue(results == "N/A");
        }

        [TestMethod]
        public void CommitmentPhaseComplete_When_EOD_And_ServiceOwed_Is_Set()
        {
            var serviceOwedMock = new Mock<IServiceOwedService>();
            var referenceRepoMock = new Mock<IReferenceDataRepository>();
            var studentRepoMock = new Mock<IStudentRepository>();
            var cryptoRepMock = new Mock<ICryptoHelper>();
			var utiltiesMock = new Mock<IUtilitiesService>();
			StudentDashboardService _service = new StudentDashboardService(serviceOwedMock.Object, referenceRepoMock.Object, studentRepoMock.Object, cryptoRepMock.Object, utiltiesMock.Object);
            DateTime EOD = Convert.ToDateTime("5/9/2022");
            string validPhaseComplete = "11/9/2023";
            decimal serviceOwed = 1.5M;
            var results = _service.CalculateCommitmentPhaseComplete(EOD, serviceOwed);
            Assert.IsTrue(validPhaseComplete == results);
        }
        [TestMethod]
        public void CalculateServiceOwed_Should_Be_NA_When_Enrolled_Year_Is_Null()
        {
            var serviceOwedMock = new Mock<IServiceOwedService>();
            var referenceRepoMock = new Mock<IReferenceDataRepository>();
            var studentRepoMock = new Mock<IStudentRepository>();
            var cryptoRepMock = new Mock<ICryptoHelper>();
			var utiltiesMock = new Mock<IUtilitiesService>();
			StudentDashboardService _service = new StudentDashboardService(serviceOwedMock.Object, referenceRepoMock.Object, studentRepoMock.Object, cryptoRepMock.Object, utiltiesMock.Object);
            var results = _service.GetServiceOwed("Semester", "N/A", null, "Fall", "2024", "Spring");
            Assert.IsTrue(results == "N/A");
        }
        [TestMethod]
        public void CalculateServiceOwed_Should_Be_NA_When_Enrolled_Session_Is_Null()
        {
            var serviceOwedMock = new Mock<IServiceOwedService>();
            var referenceRepoMock = new Mock<IReferenceDataRepository>();
            var studentRepoMock = new Mock<IStudentRepository>();
            var cryptoRepMock = new Mock<ICryptoHelper>();
			var utiltiesMock = new Mock<IUtilitiesService>();
			StudentDashboardService _service = new StudentDashboardService(serviceOwedMock.Object, referenceRepoMock.Object, studentRepoMock.Object, cryptoRepMock.Object, utiltiesMock.Object);
            var results = _service.GetServiceOwed("Semester", "N/A", "2022", null, "2024", "Spring");
            Assert.IsTrue(results == "N/A");
        }
        [TestMethod]
        public void CalculateServiceOwed_Should_Be_NA_When_Funding_End_Year_Is_Null()
        {
            var serviceOwedMock = new Mock<IServiceOwedService>();
            var referenceRepoMock = new Mock<IReferenceDataRepository>();
            var studentRepoMock = new Mock<IStudentRepository>();
            var cryptoRepMock = new Mock<ICryptoHelper>();
			var utiltiesMock = new Mock<IUtilitiesService>();
			StudentDashboardService _service = new StudentDashboardService(serviceOwedMock.Object, referenceRepoMock.Object, studentRepoMock.Object, cryptoRepMock.Object, utiltiesMock.Object);
            var results = _service.GetServiceOwed("Semester", "N/A", "2022", "Fall", null, "Spring");
            Assert.IsTrue(results == "N/A");
        }
        [TestMethod]
        public void CalculateServiceOwed_Should_Be_NA_When_Funding_End_Session_Is_Null()
        {
            var serviceOwedMock = new Mock<IServiceOwedService>();
            var referenceRepoMock = new Mock<IReferenceDataRepository>();
            var studentRepoMock = new Mock<IStudentRepository>();
            var cryptoRepMock = new Mock<ICryptoHelper>();
			var utiltiesMock = new Mock<IUtilitiesService>();
			StudentDashboardService _service = new StudentDashboardService(serviceOwedMock.Object, referenceRepoMock.Object, studentRepoMock.Object, cryptoRepMock.Object, utiltiesMock.Object);
            var results = _service.GetServiceOwed("Semester", "N/A", "2022", "Fall", "2024", null);
            Assert.IsTrue(results == "N/A");
        }
        [TestMethod]
        public void CalculateServiceOwed_Should_Be_NA_When_Funding_End_Year_Is_NA()
        {
            var serviceOwedMock = new Mock<IServiceOwedService>();
            var referenceRepoMock = new Mock<IReferenceDataRepository>();
            var studentRepoMock = new Mock<IStudentRepository>();
            var cryptoRepMock = new Mock<ICryptoHelper>();
			var utiltiesMock = new Mock<IUtilitiesService>();
			StudentDashboardService _service = new StudentDashboardService(serviceOwedMock.Object, referenceRepoMock.Object, studentRepoMock.Object, cryptoRepMock.Object, utiltiesMock.Object);
            var results = _service.GetServiceOwed("Semester", "N/A", "2022", "Fall", "N/A", "Spring");
            Assert.IsTrue(results == "N/A");
        }
        [TestMethod]
        public void CalculateServiceOwed_Should_Be_NA_When_Enrolled_Year_Is_NA()
        {
            var serviceOwedMock = new Mock<IServiceOwedService>();
            var referenceRepoMock = new Mock<IReferenceDataRepository>();
            var studentRepoMock = new Mock<IStudentRepository>();
            var cryptoRepMock = new Mock<ICryptoHelper>();
			var utiltiesMock = new Mock<IUtilitiesService>();
			StudentDashboardService _service = new StudentDashboardService(serviceOwedMock.Object, referenceRepoMock.Object, studentRepoMock.Object, cryptoRepMock.Object, utiltiesMock.Object);
            var results = _service.GetServiceOwed("Semester", "N/A", "N/A", "Fall", "2024", "Spring");
            Assert.IsTrue(results == "N/A");
        }
        [TestMethod]
        public void CalculatePGEmploymentDate_Should_Be_NA_When_ExpectedGradDate_Is_Null()
        {
            var serviceOwedMock = new Mock<IServiceOwedService>();
            var referenceRepoMock = new Mock<IReferenceDataRepository>();
            var studentRepoMock = new Mock<IStudentRepository>();
            var cryptoRepMock = new Mock<ICryptoHelper>();
			var utiltiesMock = new Mock<IUtilitiesService>();
			StudentDashboardService _service = new StudentDashboardService(serviceOwedMock.Object, referenceRepoMock.Object, studentRepoMock.Object, cryptoRepMock.Object, utiltiesMock.Object);
            var results = _service.CalculatePGEmploymentDate(null, 12, 12);
            Assert.IsTrue(results == "N/A");
        }

        [TestMethod]
        public async Task ConsolidateCommitmentNotes_Should_Return_No_Notes_When_Commitments_Less_Than_ThreeAsync()
        {
            var serviceOwedMock = new Mock<IServiceOwedService>();
            var referenceRepoMock = new Mock<IReferenceDataRepository>();
            var studentRepoMock = new Mock<IStudentRepository>();
            var cryptoRepMock = new Mock<ICryptoHelper>();
			var utiltiesMock = new Mock<IUtilitiesService>();
			StudentDashboardService _service = new StudentDashboardService(serviceOwedMock.Object, referenceRepoMock.Object, studentRepoMock.Object, cryptoRepMock.Object, utiltiesMock.Object);
            var results = await _service.ConsolidateCommitmentNotes(TestData.GetOneInternshipRecords(), TestData.GetOnePGCommitmentRecord());
            Assert.AreEqual("", results);
       
        }

        [TestMethod]
        public async Task ConsolidateCommitmentNotes_Should_Return_Notes_No_Note_When_Two_Internships_And_One_PostGrad_Commitments_Reported()
        {
            var serviceOwedMock = new Mock<IServiceOwedService>();
            var referenceRepoMock = new Mock<IReferenceDataRepository>();
            var studentRepoMock = new Mock<IStudentRepository>();
            var cryptoRepMock = new Mock<ICryptoHelper>();
			var utiltiesMock = new Mock<IUtilitiesService>();
			StudentDashboardService _service = new StudentDashboardService(serviceOwedMock.Object, referenceRepoMock.Object, studentRepoMock.Object, cryptoRepMock.Object, utiltiesMock.Object);
            var results = await _service.ConsolidateCommitmentNotes(TestData.GetTwoInternshipRecords(), TestData.GetOnePGCommitmentRecord());
            Assert.IsTrue(results == "");
        }

        [TestMethod]
        public async Task ConsolidateCommitmentNotes_Should_Return_Notes_Note_When_Three_Internships_Commitments_Reported()

        {
            var serviceOwedMock = new Mock<IServiceOwedService>();
            var referenceRepoMock = new Mock<IReferenceDataRepository>();
            var studentRepoMock = new Mock<IStudentRepository>();
            var cryptoRepMock = new Mock<ICryptoHelper>();
            var utilities = new UtilitiesService();
			StudentDashboardService _service = new StudentDashboardService(serviceOwedMock.Object, referenceRepoMock.Object, studentRepoMock.Object, cryptoRepMock.Object, utilities);
            var results = await _service.ConsolidateCommitmentNotes(TestData.GetThreeInternshipRecords(), null);
            Assert.AreEqual(TestData.ExpectedResult_For_3_Internships().Trim(), results.Trim());
        }

        [TestMethod]
        public async Task ConsolidateCommitmentNotes_Should_Return_Notes_When_Commitments_Greater_OrEqual_Fivesync()
        {
            var serviceOwedMock = new Mock<IServiceOwedService>();
            var referenceRepoMock = new Mock<IReferenceDataRepository>();
            referenceRepoMock.Setup(m => m.GetAgenciesWithDisabledAsync()).Returns(Task.FromResult(TestData.GetParentAgencies()));
            var studentRepoMock = new Mock<IStudentRepository>();
            var cryptoRepMock = new Mock<ICryptoHelper>();
			var utilities = new UtilitiesService();
			StudentDashboardService _service = new StudentDashboardService(serviceOwedMock.Object, referenceRepoMock.Object, studentRepoMock.Object, cryptoRepMock.Object, utilities);
            var results = await _service.ConsolidateCommitmentNotes(TestData.GetFiveInternshipRecords(), TestData.GetFivePGCommitmentRecord());
            Assert.IsTrue(results.Trim() == TestData.ExpectedResult_For_5_Commitments().Trim());
        }

        [TestMethod]
        public async Task GenerateSingleCommitmentNote_Should_Return_Note_When_3_Internships_Reported()
        {
            var serviceOwedMock = new Mock<IServiceOwedService>();
            var referenceRepoMock = new Mock<IReferenceDataRepository>();
            var studentRepoMock = new Mock<IStudentRepository>();
            var cryptoRepMock = new Mock<ICryptoHelper>();
			var utilities = new UtilitiesService();
			StudentDashboardService _service = new StudentDashboardService(serviceOwedMock.Object, referenceRepoMock.Object, studentRepoMock.Object, cryptoRepMock.Object, utilities);
            studentRepoMock.Setup(m => m.GetInternshipsByStudentID(5)).Returns(Task.FromResult(TestData.GetInternshipData()));
            var note = await _service.GenerateSingleCommitmentNote(3, 5, "Internship");
            var expectedNote = $"{DateTime.UtcNow.ToShortDateString()} Third IN with Agency Three with start date of 3/1/2023; ";
            Assert.IsTrue(note.Trim() == expectedNote.Trim());
          
        }

        //[TestMethod]
        //public async Task GenerateRegistrationCode_Should_Returned_Code_When_UUID_Not_NULL()
        //{
        //    var serviceOwedMock = new Mock<IServiceOwedService>();
        //    var referenceRepoMock = new Mock<IReferenceDataRepository>();
        //    referenceRepoMock.Setup(m => m.GetGlobalConfigurationsAsync()).Returns(Task.FromResult(GetGlobalConfigurations()));
        //    var studentRepoMock = new Mock<IStudentRepository>();
        //    string UUID = "0459023986";
        //    var cryptoRepMock = new Mock<ICryptoHelper>();
        //    StudentDashboardService _service = new StudentDashboardService(serviceOwedMock.Object, referenceRepoMock.Object, studentRepoMock.Object, cryptoRepMock.Object);
        //    var results = await _service.GenerateRegistrationCode(UUID);
        //    Assert.IsFalse(results == "");
        //}


        //[TestMethod]
        //public async Task GenerateRegistrationCode_Should_Returned_Null_When_UUID_Null()
        //{
        //    var serviceOwedMock = new Mock<IServiceOwedService>();
        //    var referenceRepoMock = new Mock<IReferenceDataRepository>();
        //    referenceRepoMock.Setup(m => m.GetGlobalConfigurationsAsync()).Returns(Task.FromResult(GetGlobalConfigurations()));
        //    var studentRepoMock = new Mock<IStudentRepository>();
        //    string UUID = null;
        //    var cryptoRepMock = new Mock<ICryptoHelper>();
        //    StudentDashboardService _service = new StudentDashboardService(serviceOwedMock.Object, referenceRepoMock.Object, studentRepoMock.Object, cryptoRepMock.Object);
        //    var results = await _service.GenerateRegistrationCode(UUID);
        //    Assert.IsTrue(results == null);
        //}

        //private List<GlobalConfiguration> GetGlobalConfigurations()
        //{
        //    List<GlobalConfiguration> globalConfigurations = new List<GlobalConfiguration>();
        //    globalConfigurations.Add(new GlobalConfiguration() { GlobalConfigurationID = 1234, Key = "Salt", LastModified = DateTime.UtcNow, Type = "Encryption", Value = "jcVi#tqgywnu6qj" });
        //    globalConfigurations.Add(new GlobalConfiguration() { GlobalConfigurationID = 1234, Key = "Passphrase", LastModified = DateTime.UtcNow, Type = "Encryption", Value = "fqcgnpCw]h1bpru" });
        //    globalConfigurations.Add(new GlobalConfiguration() { GlobalConfigurationID = 1234, Key = "Keysize", LastModified = DateTime.UtcNow, Type = "Encryption", Value = "256" });
        //    globalConfigurations.Add(new GlobalConfiguration() { GlobalConfigurationID = 1234, Key = "InitVect", LastModified = DateTime.UtcNow, Type = "Encryption", Value = "sdrhtyjjyS53Nwqp" });
        //    globalConfigurations.Add(new GlobalConfiguration() { GlobalConfigurationID = 1234, Key = "PasswordIterations", LastModified = DateTime.UtcNow, Type = "Encryption", Value = "2" });

        //    return globalConfigurations;
        //}

    }
}