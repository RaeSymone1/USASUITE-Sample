using System.Collections.Generic;

namespace OPM.SFS.Web.Models
{
    public class CommitmentVerificationViewModel
    {
        public string ServiceOwed { get; set; }
        public string ServiceCompleted { get; set; }
        public string TotalServiceObligation { get; set; }
        public string NextVerificationDueDate { get; set; }
        public List<CommitmentDetails> Commitments { get; set; }


        public class CommitmentDetails
        {
            public int Id { get; set; }
            public string CommitmentType { get; set; }
            public string AgencyName { get; set; }
            public string JobTitle { get; set; }
            public string CommitmentStatus { get; set; }
            public string StartDate { get; set; }
            public string EVFStatus { get; set; }
            public string EVFDateSubmitted { get; set; }
            public bool ShowAddVerification { get; set; }
        }
    }
}
