using FluentValidation.TestHelper;
using Microsoft.AspNetCore.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OPM.SFS.Web.SharedCode;
using OPM.SFS.Web.Models;
using OPM.SFS.Web.Shared;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static OPM.SFS.Web.Pages.Student.CommitmentDocumentsModel;

namespace OPM.SFS.Tests
{
    [TestClass]
    public class CommitmentDocumentTests
    {
        [TestMethod]
        public void CommitmentDocumentModel_Should_Have_Error_When_FileType_Invalid()
        {
            CommitmentDocumentValidator validator = new CommitmentDocumentValidator();
            var doc1 = GetFileMock("text/csv", "test content");
            var result = validator.IsValidDocumentForUpload(doc1);
            Assert.IsFalse(result.IsSuccess);
        }

        [TestMethod]
        public void CommitmentDocumentModel_Should_Have_No_Error_When_FileType_Is_Valid()
        {
            CommitmentDocumentValidator validator = new CommitmentDocumentValidator();
            var doc1 = GetFileMock("application/msword", "test content");
            var result = validator.IsValidDocumentForUpload(doc1);
            Assert.IsTrue(result.IsSuccess);
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
