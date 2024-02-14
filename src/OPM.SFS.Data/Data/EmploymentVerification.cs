using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPM.SFS.Data
{
	public partial class EmploymentVerification
	{
		public int EmploymentVerificationID { get; set; }
		public int StudentId { get; set; }
		public bool? IsSameEmployer { get; set; }
		public bool? IsSamePosition { get; set; }
		public bool? ReceivedLadderPromotion { get; set; }
		public bool? TakingRemedialTraining { get; set; }
		public string Training { get; set; }
		public bool? HasNewCommitment { get; set; }
		public int? EVFDocumentId { get; set; }
		public DateTime DateInserted { get; set; }
        public DateTime? LastUpdated { get; set; }
		public DateTime? PositionEndDate { get; set; }
		public int? StudentCommitmentId { get; set; }
		public string Status { get; set; }        


    }
}
