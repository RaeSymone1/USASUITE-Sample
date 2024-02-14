using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPM.SFS.Core.DTO
{
    public class CommitmentVerificationDetailsDTO
    {
        public int ID { get; set; }
        public string Agency { get; set; }
        public string JobTitle { get; set; }
        public DateTime? StartDate { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public string EVFStatus { get; set; }
        public string EVFDateSubmitted { get; set; }

    }
}
