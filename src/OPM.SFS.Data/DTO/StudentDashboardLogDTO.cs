using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPM.SFS.Core.DTO
{
    public class StudentDashboardLogDTO
    {
        public int StudentID { get; set; }
        public int StudentUID { get; set; }
        public string Contract { get; set; }
        public string TotalTerms { get; set; }
        public string PGExtensionType { get; set; }
        public string PGEmploymentDueDate { get; set; }
        public string DateLeftPGEarly { get; set; }
        public string Status { get; set; }
        public string PgVerificationOneDue { get; set; }
        public string PgVerificationOneComplete { get; set; }
        public string PgVerificationTwoDue { get; set; }
        public string PgVerificationTwoComplete { get; set; }
        public string CommitmentPhaseComplete { get; set; }
        public string Note { get; set; }
        public string LastUpdateReceived { get; set; }
        public string FollowupDate { get; set; }
        public string FollowupActionType { get; set; }
        public string FollowupAction { get; set; }
        public string ReleasePackageDueDate { get; set; }
        public string ReleasePackageSent { get; set; }
        public string Amount { get; set; }
        public string Citizenship { get; set; }
		public string SOCVerificationComplete { get; set; }
	}
}
