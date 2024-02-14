using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Routing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using OPM.SFS.Core.DTO;
using OPM.SFS.Web.Pages.Student;
using OPM.SFS.Web.SharedCode;
using WebOptimizer;

namespace OPM.SFS.Tests
{
    [TestClass]
    public class NextVerificationDueDateTests
    {

        [TestMethod]
        public void GetNextValidationDueDate_Should_Return_NA_Due_To_No_PG_Commitments()
        {
            var studentRepoMock = new Mock<IStudentRepository>();

            var maindata = new CommitmentVerificationDTO()
            {
                ServiceOwed = "1",
                PGVerificationOneDue = "02/23/2024",
                PGVerificationOneComplete = "",
                PGVerificationTwoDue = "",
                PGVerificationTwoComplete = "",
                SOCDueDate = "",
                TotalServiceObligation = ""
            };
            var commitmentData = TestData.GetFiveInternshipRecords_For_EVF();


            GetCommitmentsVerificationHandler _service = new GetCommitmentsVerificationHandler(studentRepoMock.Object);

            //Act
            string result = _service.GetNextVerificationDueDate(maindata,commitmentData);

            //Assert
            Assert.AreSame("N/A", result);
        }

        [TestMethod]
        public void GetNextValidationDueDate_Should_Return_NA_Due_To_No_PG_One_Due_Date_Nor_SOC_Due_Date()
        {
            var studentRepoMock = new Mock<IStudentRepository>();

            var maindata = new CommitmentVerificationDTO()
            {
                ServiceOwed = "1",
                PGVerificationOneDue = "",
                PGVerificationOneComplete = "",
                PGVerificationTwoDue = "",
                PGVerificationTwoComplete = "",
                SOCDueDate = "",
                TotalServiceObligation = ""
            };
            var commitmentData = TestData.GetTwoInternshipTwoPGRecords_For_EVF();


            GetCommitmentsVerificationHandler _service = new GetCommitmentsVerificationHandler(studentRepoMock.Object);

            //Act
            string result = _service.GetNextVerificationDueDate(maindata, commitmentData);

            //Assert
            Assert.AreSame("N/A", result);
        }
        [TestMethod]
        public void GetNextValidationDueDate_Should_Return_SOC_Due_Date_Due_To_No_PG_One_Due_Date()
        {
            var studentRepoMock = new Mock<IStudentRepository>();

            var maindata = new CommitmentVerificationDTO()
            {
                ServiceOwed = "1",
                PGVerificationOneDue = "",
                PGVerificationOneComplete = "",
                PGVerificationTwoDue = "",
                PGVerificationTwoComplete = "",
                SOCDueDate = "03/24/2024",
                TotalServiceObligation = ""
            };
            var commitmentData = TestData.GetTwoInternshipTwoPGRecords_For_EVF();


            GetCommitmentsVerificationHandler _service = new GetCommitmentsVerificationHandler(studentRepoMock.Object);

            //Act
            string result = _service.GetNextVerificationDueDate(maindata, commitmentData);

            //Assert
            Assert.AreSame(maindata.SOCDueDate, result);
        }
        [TestMethod]
        public void GetNextValidationDueDate_Should_Return_PG_One_Due_Date_When_No_PG_One_Completed()
        {
            var studentRepoMock = new Mock<IStudentRepository>();

            var maindata = new CommitmentVerificationDTO()
            {
                ServiceOwed = "2",
                PGVerificationOneDue = "08/24/2024",
                PGVerificationOneComplete = "",
                PGVerificationTwoDue = "",
                PGVerificationTwoComplete = "",
                SOCDueDate = "03/24/2024",
                TotalServiceObligation = ""
            };
            var commitmentData = TestData.GetTwoInternshipTwoPGRecords_For_EVF();


            GetCommitmentsVerificationHandler _service = new GetCommitmentsVerificationHandler(studentRepoMock.Object);

            //Act
            string result = _service.GetNextVerificationDueDate(maindata, commitmentData);

            //Assert
            Assert.AreSame(maindata.PGVerificationOneDue, result);
        }
        [TestMethod]
        public void GetNextValidationDueDate_Should_Return_SOC_Due_Date_Due_To_No_PG_Two_Due_Date()
        {
            var studentRepoMock = new Mock<IStudentRepository>();

            var maindata = new CommitmentVerificationDTO()
            {
                ServiceOwed = "2",
                PGVerificationOneDue = "08/24/2024",
                PGVerificationOneComplete = "08/14/2024",
                PGVerificationTwoDue = "",
                PGVerificationTwoComplete = "",
                SOCDueDate = "03/24/2024",
                TotalServiceObligation = ""
            };
            var commitmentData = TestData.GetTwoInternshipTwoPGRecords_For_EVF();


            GetCommitmentsVerificationHandler _service = new GetCommitmentsVerificationHandler(studentRepoMock.Object);

            //Act
            string result = _service.GetNextVerificationDueDate(maindata, commitmentData);

            //Assert
            Assert.AreSame(maindata.SOCDueDate, result);
        }

        [TestMethod]
        public void GetNextValidationDueDate_Should_Return_PG_Two_Due_Date_When_No_PG_Two_Completed()
        {
            var studentRepoMock = new Mock<IStudentRepository>();

            var maindata = new CommitmentVerificationDTO()
            {
                ServiceOwed = "2",
                PGVerificationOneDue = "08/24/2024",
                PGVerificationOneComplete = "08/14/2024",
                PGVerificationTwoDue = "08/24/2025",
                PGVerificationTwoComplete = "",
                SOCDueDate = "03/30/2025",
                TotalServiceObligation = ""
            };
            var commitmentData = TestData.GetTwoInternshipTwoPGRecords_For_EVF();


            GetCommitmentsVerificationHandler _service = new GetCommitmentsVerificationHandler(studentRepoMock.Object);

            //Act
            string result = _service.GetNextVerificationDueDate(maindata, commitmentData);

            //Assert
            Assert.AreSame(maindata.PGVerificationTwoDue, result);
        }
        [TestMethod]
        public void GetNextValidationDueDate_Should_Return_SOC_Due_Date_When_PG_Two_Completed()
        {
            var studentRepoMock = new Mock<IStudentRepository>();

            var maindata = new CommitmentVerificationDTO()
            {
                ServiceOwed = "2",
                PGVerificationOneDue = "08/24/2024",
                PGVerificationOneComplete = "08/14/2024",
                PGVerificationTwoDue = "08/24/2025",
                PGVerificationTwoComplete = "08/14/2025",
                SOCDueDate = "03/30/2025",
                TotalServiceObligation = ""
            };
            var commitmentData = TestData.GetTwoInternshipTwoPGRecords_For_EVF();


            GetCommitmentsVerificationHandler _service = new GetCommitmentsVerificationHandler(studentRepoMock.Object);

            //Act
            string result = _service.GetNextVerificationDueDate(maindata, commitmentData);

            //Assert
            Assert.AreSame(maindata.SOCDueDate, result);
        }
    }
}
