using System.Collections.Generic;

namespace OPM.SFS.Web.Models.Academia
{
    public class AcademiaCommitmentsViewModel
    {
        public int UserID { get; set; }
        public int InstitutionID { get; set; }
        public List<CommitmentItem> Commitments { get; set; }
        public string AlertDisplay { get; set; }

        public class CommitmentItem
        {
            public int StudentID { get; set; }
            public int CommitmentID { get; set; }
            public string StudentName { get; set; }
            public string Institution { get; set; }
            public string CommitmentType { get; set; }
            public string AgencyName { get; set; }
            public string AgencyType { get; set; }
            public string JobTitle { get; set; }
            public string StartDate { get; set; }
            public string Status { get; set; }
            public string StatusDescription { get; set; }

        }    
        
    
    }
}
