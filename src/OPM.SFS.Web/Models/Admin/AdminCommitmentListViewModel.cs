using System.Collections.Generic;

namespace OPM.SFS.Web.Models
{
    public class AdminCommitmentListViewModel
    {
        public List<CommitListItem> Commitments { get; set; }

        public class CommitListItem
        {
            public string StudentFullName { get; set; }
            public int StudentID { get; set; }
            public int CommitmentID { get; set; }
            public string Institution { get; set; }
            public string CommitmentType { get; set; }
            public string Agency { get; set; }
            public string AgencyType { get; set; }
            public string JobTitle { get; set; }
            public string StartDate { get; set; }
            public string Status { get; set; }
            public string StatusDisplay { get; set; }
            public string SubmitDate { get; set; }
            public string RejectNote { get; set; }
        }
    }

    
}
