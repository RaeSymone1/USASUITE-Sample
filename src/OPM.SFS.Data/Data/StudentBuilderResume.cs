using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace OPM.SFS.Data
{
    public partial class StudentBuilderResume
    {
        public StudentBuilderResume()
        {
            WorkExperiences = new HashSet<WorkExperience>();
            Educations = new HashSet<Education>();
        }

        public int StudentBuilderResumeId { get; set; }
        public int StudentDocumentID { get; set; }
        public int StudentId { get; set; }
        [Column(TypeName = "varchar(max)")]
        public string Objective { get; set; }
        [Column(TypeName = "varchar(max)")]
        public string OtherQualification { get; set; }
        [Column(TypeName = "varchar(max)")]
        public string JobRelatedSkill { get; set; }
        [Column(TypeName = "varchar(max)")]
        public string Certificate { get; set; }
        [Column(TypeName = "varchar(max)")]
        public string HonorsAwards { get; set; }
        [Column(TypeName = "varchar(max)")]
        public string Supplemental { get; set; }
        public DateTime? DateInserted { get; set; }
        public DateTime? LastModified { get; set; }


        public virtual Student Student { get; set; }
        public virtual StudentDocument StudentDocument { get; set; }
        public virtual ICollection<WorkExperience> WorkExperiences { get; set; }
        public virtual ICollection<Education> Educations { get; set; }
    }
}
