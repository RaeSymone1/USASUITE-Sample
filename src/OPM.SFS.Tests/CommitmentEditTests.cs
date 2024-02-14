using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OPM.SFS.Data;
using OPM.SFS.Web.Infrastructure;
using OPM.SFS.Web.Shared;
using OPM.SFS.Web.Pages.Student;
using FluentValidation.TestHelper;
using OPM.SFS.Web.Models;
using OPM.SFS.Web.SharedCode;

namespace OPM.SFS.Tests
{
    [TestClass]
    public class CommitmentEditTests
    {
        [TestMethod]
        public void CommitmentModel_Should_Have_Error_When_PreAPPR_Flow_No_Justification()
        {
            CommitmentEditValidator validator = new CommitmentEditValidator();
            var model = new CommitmentModelViewModel { ShowForm = CommitmentProcessConst.CommitmentApprovalTentative };
            var result = validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(request => request.Justification);
        }

        [TestMethod]
        public void CommitmentModel_Should_Have_No_Error_When_PreAPPR_Flow()
        {
            CommitmentEditValidator validator = new CommitmentEditValidator();
            var model = new CommitmentModelViewModel { ShowForm = CommitmentProcessConst.CommitmentApprovalTentative, AgencyType = 1, Agency = 1, Justification = "Sample text", JobTitle = "Tester", CommitmentType = 1 };
            var result = validator.TestValidate(model);
            result.ShouldNotHaveAnyValidationErrors();
        }

        [TestMethod]
        public void CommitmentModel_Should_Have_Error_When_APPR_Flow_No_StartDate()
        {
            CommitmentEditValidator validator = new CommitmentEditValidator();
            var model = new CommitmentModelViewModel { ShowForm = CommitmentProcessConst.CommitmentApprovalFinal };
            var result = validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(request => request.StartDateDay);
        }

        [TestMethod]
        public void CommitmentModel_Should_Have_No_Error_When_APPR_Flow()
        {
            CommitmentEditValidator validator = new CommitmentEditValidator();
            var model = new CommitmentModelViewModel 
            { 
                ShowForm = CommitmentProcessConst.CommitmentApprovalFinal, 
                AgencyType = 1, 
                Agency = 1, 
                JobTitle = "Tester",
                CommitmentType = 1, 
                SalaryMin = 25,
                SalaryMax = 35,
                PayRate = 1,
                PayPlan = "GS",
                Series = "2210",
                Grade = "09",
                City = "Atlanta",
                State = 1,
                PostalCode = "10000",
                SupervisorFirstName = "Test",
                SupervisorLastName = "Test",
                SupervisorEmailAddress = "test",
                SupervisorPhoneAreaCode = "123",
                SupervisorPhonePrefix = "456",
                SupervisorPhoneSuffix = "6789",
                StartDateMonth = "10",
                StartDateDay = "10",
                StartDateYear = "2022",
                JobSearchType = 1
            };
            var result = validator.TestValidate(model);
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
