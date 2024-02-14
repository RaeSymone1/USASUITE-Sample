using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace OPM.SFS.Web.Models
{
    public class AdminAgencyEditViewModel
    {
        public int AgencyID { get; set; }
        public int AgencyType { get; set; }
        public SelectList AgencyTypeList { get; set; }
        public bool HasPayPlanGradeSeries { get; set; }
        public string AgencyName { get; set; }
        public SelectList ParentAgencyList { get; set; }
        public int? ParentAgency { get; set; }
        public string City { get; set; }
        public string County { get; set; }
        public SelectList StateList { get; set; }
        public int State { get; set; }
        public string Country { get; set; }
        public SelectList ApprovalProcessList { get; set; }
        public int CommitmentApprovalWorkflow { get; set; }
    }
}
