using FluentValidation.TestHelper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OPM.SFS.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static OPM.SFS.Web.Pages.Student.ResumeEducationModel;

namespace OPM.SFS.Tests
{
    [TestClass]
    public class EducationTests
    {
        [TestMethod]
        public void EducationModel_Should_Have_Error_When_Invalid_Data_Used()
        {
            EducationValidator validator = new EducationValidator();
            EducationViewModel model = new();
            var result = validator.TestValidate(model);
            result.ShouldHaveAnyValidationError();

        }
    }
}
