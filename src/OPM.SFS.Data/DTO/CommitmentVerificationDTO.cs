using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPM.SFS.Core.DTO
{
    public class CommitmentVerificationDTO
    {
        public string ServiceOwed { get; set; }
        public string PGVerificationOneDue { get; set; }
        public string PGVerificationOneComplete { get; set; }
        public string PGVerificationTwoDue { get; set; }
        public string PGVerificationTwoComplete { get; set; }
        public string SOCDueDate { get; set; }
        public string TotalServiceObligation { get; set; }
    }
}
