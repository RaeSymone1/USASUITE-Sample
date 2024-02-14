using FluentValidation.TestHelper;
using Microsoft.AspNetCore.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OPM.SFS.Web.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static OPM.SFS.Web.Pages.Student.DocumentListModel;
using static OPM.SFS.Web.Pages.Student.ResumeListModel;

namespace OPM.SFS.Tests
{
    [TestClass]
    public class StudentDocumentRepositoryTests
    {
        [TestMethod]
        public void StudentResume_Should_Have_Error_When_Uploading_CSV_File()
        {
            ResumeUploadValidator validator = new ResumeUploadValidator();
            var model = new UploadResumeViewModel { Name = "Test Resume", Resume = GetFileMock("text/csv", "some text") };
            var result = validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(request => request.Resume.ContentType);
        }

        [TestMethod]
        public void StudentResume_Should_Have_Error_When_Name_Invalid()
        {
            ResumeUploadValidator validator = new ResumeUploadValidator();
            var model = new UploadResumeViewModel { Name = "Test Resume<script>", Resume = GetFileMock("application/pdf", "some text") };
            var result = validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(request => request.Name);
        }

        [TestMethod]
        public void StudentResume_Should_Have_No_Errors()
        {
            ResumeUploadValidator validator = new ResumeUploadValidator();
            var model = new UploadResumeViewModel { Name = "Test Resume", Resume = GetFileMock("application/pdf", "some text") };
            var result = validator.TestValidate(model);
            result.ShouldNotHaveAnyValidationErrors();
        }


        [TestMethod]
        public void StudentOtherDocument_Should_Have_Error_When_Uploading_CSV_File()
        {
            DocumentUploadValidator validator = new DocumentUploadValidator();
            var model = new UploadDocumentViewModel { Name = "Test Resume", Document = GetFileMock("text/csv", "some text") };
            var result = validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(request => request.Document.ContentType);
        }

        [TestMethod]
        public void StudentOtherDocument_Should_Have_Error_When_Name_Invalid()
        {
            DocumentUploadValidator validator = new DocumentUploadValidator();
            var model = new UploadDocumentViewModel { Name = "Test Resume<script>", Document = GetFileMock("application/pdf", "some text") };
            var result = validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(request => request.Name);
        }

        [TestMethod]
        public void StudentOtherDocument_Should_Have_No_Errors()
        {
            DocumentUploadValidator validator = new DocumentUploadValidator();
            var model = new UploadDocumentViewModel { Name = "Test Resume", Document = GetFileMock("application/pdf", "some text") };
            var result = validator.TestValidate(model);
            result.ShouldNotHaveAnyValidationErrors();
        }

        private IFormFile GetFileMock(string contentType, string content)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(content);

            var file = new FormFile(
                baseStream: new MemoryStream(bytes),
                baseStreamOffset: 0,
                length: bytes.Length,
                name: "Data",
                fileName: "dummy.csv"
            )
            {
                Headers = new HeaderDictionary(),
                ContentType = contentType
            };

            return file;
        }
    }

   
}
