using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPM.SFS.Core.DTO
{
	public class CommitmentReportedDTO
	{
		public int StudentID { get; set; }
		public int CommitmentID { get; set; }
		public string CommitmentType { get; set; }
		public int? ParentAgencyID { get; set; }
		public string AgencyName { get; set; }
		public string AgencyType { get; set; }		
		public string SubAgencyName { get; set; }		
		public DateTime? StartDate { get; set; }
		public DateTime? DateApproved { get; set; }
		public string Status { get; set; }
		public string StatusDisplay { get;set; }
		public string ServiceOwed { get; set; }
	}
}
