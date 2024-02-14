using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace OPM.SFS.Web.Models
{
    public class OtherDocumentViewModel
    {
        public List<DocumentViewModel> DocumentList { get; set; }
        public int ResumeCount { get; set; }
        public IFormFile UploadedResume { get; set; }
        public int MaxDocuments { get; set; } = 15;
        public int MaxResumes { get; set; } = 5;
        public List<DocumentTypeViewModel> DocTypes { get; set; }
        public string MalwareResultDocument { get; set; }
        

        public class DocumentViewModel
        {
            public int DocumentID { get; set; }
            public string Name { get; set; }
            public string Date { get; set; }
            public string Type { get; set; }
            public bool CanDelete { get; set; }
        }

       
    }

    public class UploadDocumentViewModel
    {
        public IFormFile Document { get; set; }
        public string Name { get; set; }
        public int DocumentType { get; set; }
        public int StudentID { get; set; }
    }
    public class DocumentTypeViewModel
    {
        public int DocumentTypeID { get; set; }
        public string DocumentTypeDispley { get; set; }
        public string Code { get; set; }

    }

}
