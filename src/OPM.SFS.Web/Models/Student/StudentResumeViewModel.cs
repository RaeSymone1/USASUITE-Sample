using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace OPM.SFS.Web.Models
{
    public class StudentResumeViewModel
    {
        public List<DocumentViewModel> ResumeList { get; set; }
        public int OtherDocumentCount { get; set; }
        public IFormFile UploadedResume { get; set; }
        public int MaxDocuments { get; set; } = 15;
        public int MaxResumes { get; set; } = 5;
        public string MalwareResultDocument { get; set; }

        public class DocumentViewModel
        {
            public int DocumentID { get; set; }
            public string Name { get; set; }
            public string Date { get; set; }
            public string Type { get; set; }
            public bool CanDelete { get; set; }
            public bool IsSharable { get; set; }
        }
    }

    public class UploadResumeViewModel
    {
        public IFormFile Resume { get; set; }
        public string Name { get; set; }
    }
}
