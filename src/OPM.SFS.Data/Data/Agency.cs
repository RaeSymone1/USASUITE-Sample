using System;
using System.Collections.Generic;

#nullable disable

namespace OPM.SFS.Data
{
    public partial class Agency
    {
        public Agency()
        {
            StudentCommitments = new HashSet<StudentCommitment>();
        }

        public int AgencyId { get; set; }
        public int? AgencyTypeId { get; set; }
        public int? ParentAgencyId { get; set; }
        public string Name { get; set; }
        public int? AddressId { get; set; }
        public int? CommitmentApprovalWorkflowId { get; set; }
        public bool? RequirePayPlanSeriesGrade { get; set; }
        public bool? RequireSmartCardAuth { get; set; }
        public bool? IsDisabled { get; set; }
        public DateTime? DateInserted { get; set; }
        public DateTime? LastModified { get; set; }
        public int? ModifiedBy { get; set; }

        public virtual AgencyType AgencyType { get; set; }
        public virtual ICollection<StudentCommitment> StudentCommitments { get; set; }
        public virtual Address Address { get; set; }
        public virtual CommitmentApprovalWorkflow CommitmentApprovalWorkflow { get; set; }
    }
}
