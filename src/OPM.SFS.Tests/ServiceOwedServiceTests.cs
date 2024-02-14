using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Routing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using OPM.SFS.Web.SharedCode;

namespace OPM.SFS.Tests
{
    [TestClass]
    public class ServiceOwedServiceTests
    {
        [TestMethod]
        public void CalculateServiceOwedbyDateTime_Should_Fail_Due_To_Invalid_Institution_Academic_Schedule_Exeception()
        {
            //Arrange
            string institutiontype = "School Time";
            DateTime Startdate = new DateTime(2022, 05, 17);
            DateTime EndDate = new DateTime(2026, 05, 17);
            ServiceOwedService _service = new ServiceOwedService();

            //Act
            var result = _service.CalculateServiceOwedbyDateTime(institutiontype, Startdate, EndDate);

            //Assert
            Assert.AreSame("Invalid Institution Academic Schedule", result.ex);
        }

        [TestMethod]
        public void CalculateServiceOwedbyDateTime_Should_Fail_Due_To_Invalid_Fudning_End_Date_Exeception()
        {
            //Arrange
            string institutiontype = "Trimester";
            DateTime Startdate = new DateTime(2022, 05, 17);
            DateTime EndDate = new DateTime(2022, 05, 20);
            ServiceOwedService _service = new ServiceOwedService();

            //Act
            var result = _service.CalculateServiceOwedbyDateTime(institutiontype, Startdate, EndDate);

            //Assert
            Assert.AreSame("Invalid Funding Date Entered, Please enter a Funding Date that is at least one year more than your funding start date", result.ex);
        }

        [TestMethod]
        public void CalculateServiceOwedbyDateTime_Should_Fail_Due_To_Service_Owed_Limit_Exceded_Exeception()
        {
            //Arrange
            string institutiontype = "Trimester";
            DateTime Startdate = new DateTime(2022, 05, 17);
            DateTime EndDate = new DateTime(2027, 05, 20);
            ServiceOwedService _service = new ServiceOwedService();

            //Act
            var result = _service.CalculateServiceOwedbyDateTime(institutiontype, Startdate, EndDate);

            //Assert
            Assert.AreSame("Maximum threshold for service owed was exceeded", result.ex);
        }

        [TestMethod]
        public void CalculateServiceOwedbyDateTime_Should_Succeed_Correct_Academic_Schedule_Correct_Start_and_End_Dates()
        {
            //Arrange
            string institutiontype = "Trimester";
            DateTime Startdate = new DateTime(2022, 05, 17);
            DateTime EndDate = new DateTime(2024, 11, 17);
            ServiceOwedService _service = new ServiceOwedService();

            //Act

            var result = _service.CalculateServiceOwedbyDateTime(institutiontype, Startdate, EndDate);

            //Assert
            Assert.IsTrue(result.ServiceTime >= 1 & result.ServiceTime <= 3);
        }
        [TestMethod]
        public void CalculateServiceOwedbySeason_Should_Fail_Due_To_Invalid_Institution_Academic_Schedule_Exeception()
        {
            //Arrange
            string institutiontype = "School Time";
            string StartSeason = "Spring";
            int StartYear = 2020;
            string EndSeason = "Summer";
            int EndYear = 2023;
            ServiceOwedService _service = new ServiceOwedService();

            //Act
            var result = _service.CalculateServiceOwedbySeason(institutiontype, StartSeason, StartYear, EndSeason, EndYear);

            //Assert
            Assert.AreSame("Invalid Institution Academic Schedule", result.ex);
        }

        [TestMethod]
        public void CalculateServiceOwedbySeason_Should_Fail_Due_To_Invalid_Funding_End_Date_Exeception()
        {
            //Arrange
            string institutiontype = "Trimester";
            string StartSeason = "Fall";
            int StartYear = 2022;
            string EndSeason = "Winter";
            int EndYear = 2021;
            ServiceOwedService _service = new ServiceOwedService();

            //Act
            var result = _service.CalculateServiceOwedbySeason(institutiontype, StartSeason, StartYear, EndSeason, EndYear);

            //Assert
            Assert.AreSame("Invalid Funding Date Entered, Please enter a Funding Date that is at least one year more than your funding start date", result.ex);
        }

        [TestMethod]
        public void CalculateServiceOwedbySeason_Should_Fail_Due_To_Service_Owed_Limit_Exceded_Exeception()
        {
            //Arrange
            string institutiontype = "Trimester";
            string StartSeason = "Spring";
            int StartYear = 2020;
            string EndSeason = "Summer";
            int EndYear = 2026;
            ServiceOwedService _service = new ServiceOwedService();

            //Act
            var result = _service.CalculateServiceOwedbySeason(institutiontype, StartSeason, StartYear, EndSeason, EndYear);

            //Assert
            Assert.AreSame("Maximum threshold for service owed was exceeded", result.ex);
        }

        [TestMethod]
        public void CalculateServiceOwedbySeason_Should_Succeed_Correct_Academic_Schedule_Correct_Start_and_End_Dates()
        {
            //Arrange
            string institutiontype = "Trimester";
            string StartSeason = "Spring";
            int StartYear = 2020;
            string EndSeason = "Summer";
            int EndYear = 2022;
            ServiceOwedService _service = new ServiceOwedService();

            //Act

            var result = _service.CalculateServiceOwedbySeason(institutiontype, StartSeason, StartYear, EndSeason, EndYear);

            //Assert
            Assert.IsTrue(result.ServiceTime >= 1 & result.ServiceTime <= 3);
        }

        [TestMethod]
        public void CalculateServiceOwedbyTerms_Should_Fail_Due_To_Invalid_Institution_Academic_Schedule_Exeception()
        {
            //Arrange
            string institutiontype = "School Time";
            int Terms = 4;
            ServiceOwedService _service = new ServiceOwedService();

            //Act
            var result = _service.CalculateServiceOwedbyTerms(institutiontype, Terms);

            //Assert
            Assert.AreSame("Invalid Institution Academic Schedule", result.ex);
        }

        [TestMethod]
        public void CalculateServiceOwedbyTerms_Should_Fail_Due_To_Invalid_Funding_End_Date_Exeception()
        {
            //Arrange
            string institutiontype = "Trimester";
            int Terms = -1;
            ServiceOwedService _service = new ServiceOwedService();

            //Act
            var result = _service.CalculateServiceOwedbyTerms(institutiontype, Terms);

            //Assert
            Assert.AreSame("Invalid Funding Date Entered, Please enter a Funding Date that is at least one year more than your funding start date", result.ex);
        }

        [TestMethod]
        public void CalculateServiceOwedbyTerms_Should_Fail_Due_To_Service_Owed_Limit_Exceded_Exeception()
        {
            //Arrange
            string institutiontype = "Trimester";
            int Terms = 22;
            ServiceOwedService _service = new ServiceOwedService();

            //Act
            var result = _service.CalculateServiceOwedbyTerms(institutiontype, Terms);

            //Assert
            Assert.AreSame("Maximum threshold for service owed was exceeded", result.ex);
        }

        [TestMethod]
        public void CalculateServiceOwedbyTerms_Should_Succeed_Correct_Academic_Schedule_Correct_Start_and_End_Dates()
        {
            //Arrange
            string institutiontype = "Trimester";
            int Terms = 2;
            ServiceOwedService _service = new ServiceOwedService();

            //Act

            var result = _service.CalculateServiceOwedbyTerms(institutiontype, Terms);

            //Assert
            Assert.IsTrue(result.ServiceTime >= 1 & result.ServiceTime <= 3);
        }
    }
}
