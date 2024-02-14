using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OPM.SFS.Data;
using OPM.SFS.Web.SharedCode;
using OPM.SFS.Web.Infrastructure;
using OPM.SFS.Web.Shared;
using OPM.SFS.Web.Pages.Student;
using FluentValidation.TestHelper;
using static OPM.SFS.Web.Pages.Student.RegistrationDetailsModel;
using OPM.SFS.Web.Models.Student;
using Moq;
using OPM.SFS.Core.Shared;

namespace OPM.SFS.Tests
{
    [TestClass]
    public class StudentRegistrationTests
    {
        [TestMethod]
        public void RegistrationCode_should_error_when_null()
        {
            RegistrationValidator validator = new RegistrationValidator();
            var model = new RegistrationCodeRequest { Code = null };
            var result = validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(request => request.Code);
        }

        [TestMethod]
        public void RegistrationCode_should_error_when_characters_invalid()
        {
            RegistrationValidator validator = new RegistrationValidator();
            var model = new RegistrationCodeRequest { Code = "<>''(script)123456AB" };
            var result = validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(request => request.Code);
        }

        [TestMethod]
        public void RegistrationCode_Is_Valid()
        {
            var utilties = new UtilitiesService();
			StudentRegistrationHelper tester = new StudentRegistrationHelper(utilties);
            var quarterStartDate = tester.GetFirstDayOfQuarter(DateTime.UtcNow);
            var registrationCodeTest = new RegistrationCode()
            {
                Code = "123456",
                QuarterName = "2021-Q4",
                QuarterStartDate = quarterStartDate
            };                        
            var result = tester.ValidateCode(registrationCodeTest);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void RegistrationCode_Is_Invalid_For_Quarter()
        {
			var utiltiesMock = new Mock<IUtilitiesService>();
			StudentRegistrationHelper tester = new StudentRegistrationHelper(utiltiesMock.Object);
            var registrationCodeTest = new RegistrationCode()
            {
                Code = "123456",
                QuarterName = "2021-Q4",
                QuarterStartDate = Convert.ToDateTime("2020-10-01")
            };
            var result = tester.ValidateCode(registrationCodeTest);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void FirstDay_Of_Quarter_Is_Valid()
        {
            string First_Day_Of_Q42021 = "10/1/2021";
			var utiltiesMock = new Mock<IUtilitiesService>();
			StudentRegistrationHelper tester = new StudentRegistrationHelper(utiltiesMock.Object);
            DateTime Date_In_Q42021 = Convert.ToDateTime("12/1/2021");
            var result = tester.GetFirstDayOfQuarter(Date_In_Q42021);
            Assert.AreEqual(result.ToShortDateString(), First_Day_Of_Q42021);
        }

        [TestMethod]
        public void FirstDay_Of_Quarter_Is_InValid()
        {
            string First_Day_Of_Q42021 = "10/1/2021";
			var utiltiesMock = new Mock<IUtilitiesService>();
			StudentRegistrationHelper tester = new StudentRegistrationHelper(utiltiesMock.Object);
            DateTime Date_In_Q12021 = Convert.ToDateTime("1/10/2021");
            var result = tester.GetFirstDayOfQuarter(Date_In_Q12021);
            Assert.AreNotEqual(result.ToShortDateString(), First_Day_Of_Q42021);
        }

        [TestMethod]
        public void LastDay_Of_Quarter_Is_Valid()
        {
            string Last_Day_Of_Q42021 = "12/31/2021";
			var utiltiesMock = new Mock<IUtilitiesService>();
			StudentRegistrationHelper tester = new StudentRegistrationHelper(utiltiesMock.Object);
            DateTime RandomDate_In_Q42021 = Convert.ToDateTime("11/1/2021");
            var result = tester.GetLastDayOfQuarter(RandomDate_In_Q42021);
            Assert.AreEqual(result.ToShortDateString(), Last_Day_Of_Q42021);
        }

        [TestMethod]
        public void LastDay_Of_Quarter_Is_InValid()
        {
            string Last_Day_Of_Q42021 = "12/31/2021";
			var utiltiesMock = new Mock<IUtilitiesService>();
			StudentRegistrationHelper tester = new StudentRegistrationHelper(utiltiesMock.Object);
            DateTime RandomDate_In_Q12021 = Convert.ToDateTime("1/10/2021");
            var result = tester.GetFirstDayOfQuarter(RandomDate_In_Q12021);
            Assert.AreNotEqual(result.ToShortDateString(), Last_Day_Of_Q42021);
        }

        [TestMethod]
        public void RegistrationDetails_should_not_have_validation_errors()
        {
            RegistrationDetailsValidator validator = new RegistrationDetailsValidator();
            var model = new StudentRegistrationViewModel
            {
                Firstname = "Test",
                Lastname = "Test",
                Email = "test@test.com",
                AlternateEmail = "test2@test.com",
                SSN = "1234567",
                ConfirmSSN = "1234567",
                DateOfBirth = "01/01/2000",
                GraduationDate = "12/2024",
                SelectedCollege = 1,
                SelectedDegree = 1,
                SelectedDiscipline = 1,
                InitialFundingDate = "12/2022"
            };
            var result = validator.TestValidate(model);
            result.ShouldNotHaveAnyValidationErrors();
        }

        [TestMethod]
        public void RegistrationDetails_should_have_validation_error_email_matches()
        {
            RegistrationDetailsValidator validator = new RegistrationDetailsValidator();
            var model = new StudentRegistrationViewModel
            {
                Firstname = "Test",
                Lastname = "Test",
                Email = "test@test.com",
                AlternateEmail = "test@test.com",
                SSN = "1234567",
                ConfirmSSN = "1234567",
                DateOfBirth = "1/1/2000",
                GraduationDate = "12/2024",
                SelectedCollege = 1,
                SelectedDegree = 1 ,
                SelectedDiscipline = 1,
                InitialFundingDate = "12/2022"
            };
            var result = validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Email);
        }

    }
}
