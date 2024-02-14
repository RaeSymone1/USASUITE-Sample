using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace OPM.SFS.Web.Models
{
    public class AdminCommitmentDashboardViewModel
    {
        public SelectList ApprovalStatusList { get; set; }
        public int ApprovalStatus { get; set; }
        public string LastApprovedStartdate { get; set; }
        public string LastApprovedEndDate { get; set; }
        public int TotalCommitments { get; set; }
        public List<CommitmentsByType> CommitmentsByAgencyType { get; set; }
        public List<CommitmentsByType> CommitmentsByType { get; set; }
        public string ReportDescription { get; set; }
       

    }

    public class CommitmentsByType
    {
        public int TypeID { get; set; }
        public string TypeName { get; set; }
        public int Total { get; set; }
        public double Percentage { get; set; }
    }

    public class SearchFiltersViewModel
    {
        public int StatusID { get; set; }
        public string Startdate { get; set; }
        public string Enddate { get; set; }
    }


}
