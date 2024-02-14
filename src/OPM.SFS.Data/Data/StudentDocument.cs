using System;
using System.Collections.Generic;

#nullable disable

namespace OPM.SFS.Data
{
    public partial class StudentDocument
    {
        public StudentDocument()
        {
            StudentBuilderResumes = new HashSet<StudentBuilderResume>();
        }
        public int StudentDocumentId { get; set; }
        public int StudentId { get; set; }
        public int? DocumentTypeId { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public DateTime? DateCreated { get; set; }
        public bool? IsDeleted { get; set; }
        public string UserId { get; set; }
        public virtual Student Student { get; set; }
        public virtual DocumentType DocumentType { get; set; }
        public bool IsShareable { get; set; }
        public string LastUpdatedBy { get; set; }
        public virtual ICollection<StudentBuilderResume> StudentBuilderResumes { get; set; }


    }
}
