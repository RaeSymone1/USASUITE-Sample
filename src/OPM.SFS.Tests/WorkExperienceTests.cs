using FluentValidation.TestHelper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OPM.SFS.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static OPM.SFS.Web.Pages.Student.WorkExperienceModel;

namespace OPM.SFS.Tests
{
    [TestClass]
    public class WorkExperienceTests
    {
        [TestMethod]
        public void WorkExperienceModel_Should_Have_Error_When_Invalid_Data_Used()
        {
            WorkExperienceValidator validator = new WorkExperienceValidator();
            WorkExperienceViewModel model = new();
            var result = validator.TestValidate(model);
            result.ShouldHaveAnyValidationError();

        }
    }
}
