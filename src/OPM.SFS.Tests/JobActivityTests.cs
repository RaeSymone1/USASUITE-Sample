using FluentValidation.TestHelper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OPM.SFS.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static OPM.SFS.Web.Pages.Student.JobActivityEditModel;

namespace OPM.SFS.Tests
{
    [TestClass]
    public class JobActivityTests
    {
        [TestMethod]
        public void JobActivity_Should_Have_Error_When_Invalid_Data_Used()
        {
            JobActivityValidator validator = new JobActivityValidator();
            JobActivityEditViewModel model = new();
            var result = validator.TestValidate(model);
            result.ShouldHaveAnyValidationError();

        }
    }
}
