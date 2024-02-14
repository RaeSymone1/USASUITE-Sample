using System.Collections.Generic;

namespace OPM.SFS.Web.Models
{
    public class AdminCommitmentListExcelViewModel
    {
        public List<CommitmentListItem> Commitments { get; set; }
        public class CommitmentListItem
        {
            public int StudentID { get; set; }
            public int CommitmentID { get; set; }
            public string StudentFullName { get; set; }
            public string Institution { get; set; }
            public string CommitmentType { get; set; }
            public string Agency { get; set; }
            public string AgencyType { get; set; }
            public string JobTitle { get; set; }
            public string StartDate { get; set; }
            public string Status { get; set; }
        }

    }
}
