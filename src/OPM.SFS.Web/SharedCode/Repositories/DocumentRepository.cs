using OPM.SFS.Data;
using OPM.SFS.Web.Models.Academia;
using OPM.SFS.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OPM.SFS.Web.SharedCode;
using System.IO;
using OPM.SFS.Core.Shared;
using Microsoft.EntityFrameworkCore;

namespace OPM.SFS.Web.Infrastructure
{

    public interface IDocumentRepository
    {
        Task<FileStreamResult> GetDocumentStreamByDocumentId(int DocumentId);
        Task<string> GetDocumentFileNameByDocumentId(int documentId);
        Task<string> GetDocumentFilePathByDocumentId(int documentId);
        Task<StudentDocument> GetDocumentByDocumentId(int documentId);
    }

    public class DocumentRepository : IDocumentRepository
    {
        private readonly ScholarshipForServiceContext _efDB;


        public DocumentRepository(ScholarshipForServiceContext efDB)
        {
            _efDB = efDB;
        }
        public async Task<FileStreamResult> GetDocumentStreamByDocumentId(int DocumentId)
        {
            var docLookup = await _efDB.StudentDocuments.AsNoTracking().FirstOrDefaultAsync(m => m.StudentDocumentId == DocumentId);
            var extension = System.IO.Path.GetExtension(docLookup.FilePath);
            if (docLookup is not null)
            {
                FileStream fileStream = new FileStream(docLookup.FilePath, FileMode.Open, FileAccess.Read);
                var theFile = new FileStreamResult(fileStream, GetMimeType(extension));
                theFile.FileDownloadName = $"{docLookup.FileName}{Path.GetExtension(docLookup.FilePath)}";
                return theFile;
            }
            return null;
        }

		public async Task<string> GetDocumentFileNameByDocumentId(int documentId)
		{
			var docLookup = await _efDB.StudentDocuments.AsNoTracking().FirstOrDefaultAsync(m => m.StudentDocumentId == documentId);
            return docLookup.FileName;
		}

        public async Task<string> GetDocumentFilePathByDocumentId(int documentId)
        {
            var docLookup = await _efDB.StudentDocuments.AsNoTracking().FirstOrDefaultAsync(m => m.StudentDocumentId == documentId);
            return docLookup.FilePath;
        }
        public async Task<StudentDocument> GetDocumentByDocumentId(int documentId)
        {
            var docLookup = await _efDB.StudentDocuments.AsNoTracking().FirstOrDefaultAsync(m => m.StudentDocumentId == documentId);
            return docLookup;
        }

        public string GetMimeType(string filetype)
        {
            switch (filetype.ToLower().Replace(".", ""))
            {
                case "pdf":
                    return "application/pdf";
                case "doc":
                    return "application/msword";
                case "docx":
                    return "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                case "png":
                    return "image/png";
                case "gif":
                    return "image/gif";
                case "jpg":
                    return "image/jpeg";
                case "pptx":
                    return "application/vnd.openxmlformats-officedocument.presentationml.presentation";
                case "ppt":
                    return "application/vnd.ms-powerpoint";
				case "txt":
					return "text/plain";
				default:
                    break;
            }
            return "";
        }
    }
}
