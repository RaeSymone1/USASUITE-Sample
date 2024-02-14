using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPM.SFS.Data
{
    public class CommitmentApprovalWorkflow
    {
        public CommitmentApprovalWorkflow()
        {
            Agencies = new HashSet<Agency>();
        }
        public int CommitmentApprovalWorkflowId { get; set; }
        [MaxLength(50)]
        public string Code { get; set; }
        [MaxLength(100)]
        public string Description { get; set; }
        [MaxLength(250)]
        public string Display { get; set; }
        public virtual ICollection<Agency> Agencies { get; set; }
    }
}
