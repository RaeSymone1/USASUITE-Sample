using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPM.SFS.Core.DTO
{
    public class StudentDashboardCommitmentsDTO
    {
        public int StudentId { get; set; }
        public int StudentCommitmentID { get; set; }
        public string Agency { get; set; }
        public int? ParentAgencyID { get; set; }
        public string ParentAgency { get; set; }
        public string AgencyType { get; set; }
        public DateTime? DateApproved { get; set; }
        public string StartDate { get; set; }
        public string Status { get; set; }
        public string CommitmentType { get; set; }

    }
}
