using FluentValidation.TestHelper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OPM.SFS.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static OPM.SFS.Web.Pages.Student.ProfileModel;

namespace OPM.SFS.Tests
{
    [TestClass]
    public class StudentProfileTests
    {
        [TestMethod]
        public void ProfileModel_Should_Have_Error_When_Invalid_Data_Used()
        {
            ProfileValidator validator = new ProfileValidator();
            var model = new StudentProfileViewModel()
            {
                Firstname = "test",
                Lastname = "<script'.?>",
                DateOfBirth = "1/1/2000",
                Email = "test",
                AlternateEmail = "test",
                ExpectedGradDate = "test",
                DateAvailIntern = "test",
                DateAvailPostGrad = "test",
                CurrAddress1 = "test",
                CurrCity = "test",
                CurrStateID = 1,
                CurrPostalCode = "test",
                CurrPhone = "test",
                PermAddress1 = "test",
                PermCity = "test",
                PermStateID = 0,
                PermPostalCode = "test",
                ContactFirstname = "test",
                ContactLastname = "test",
                ContactRelationship = "test",
                ContactPhone = "test"
            };
            var result = validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(request => request.Lastname);
        }

        [TestMethod]
        public void ProfileModel_Should_Have_No_Errors()
        {
            ProfileValidator validator = new ProfileValidator();
            var model = new StudentProfileViewModel()
            {
                Firstname = "test",
                Lastname = "test",
                DateOfBirth = "1/1/2000",
                Email = "test@test.com",
                AlternateEmail = "test@test.com",
                ExpectedGradDate = "test",
                DateAvailIntern = "test",
                DateAvailPostGrad = "test",
                CurrAddress1 = "test",
                CurrCity = "test",
                CurrStateID = 1,
                CurrPostalCode = "test",
                CurrPhone = "test",
                PermAddress1 = "test",
                PermCity = "test",
                PermStateID = 1,
                PermPostalCode = "test",
                PermPhone = "123456789",
                ContactFirstname = "test",
                ContactLastname = "test",
                ContactRelationship = "test",
                ContactPhone = "test",
                ContactEmail = "test@test.com"
            };
            var result = validator.TestValidate(model);
            result.ShouldNotHaveAnyValidationErrors();
        }
    }   
}

