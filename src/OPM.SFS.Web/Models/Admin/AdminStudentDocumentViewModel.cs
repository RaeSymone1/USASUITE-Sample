using System.Collections.Generic;

namespace OPM.SFS.Web.Models
{
    public class AdminStudentDocumentViewModel
    {
        public int StudentID { get; set; }
        public string Studentname { get; set; }
        public string University { get; set; }
        public string Degree { get; set; }
        public string Major { get; set; }
        public string GradDate { get; set; }
        public string MalwareResultDocument { get; set; }
        public List<DocumentItem> Documents { get; set; }
        public List<DocumentTypeViewModel> DocTypes { get; set; }

        public class DocumentItem
        {
            public int DocumentID { get; set; }
            public string DocumentName { get; set; }
            public string DocumentType { get; set; }
            public string DateUploaded { get; set; }
            public bool IsBuilderResume { get; set; }
        }
    }
}
