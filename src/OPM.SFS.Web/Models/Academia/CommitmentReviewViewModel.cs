using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace OPM.SFS.Web.Models.Academia
{
    public class CommitmentReviewViewModel
    {
        public int CommitmentId { get; set; }
        public string FormattedSSN { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public int? AgencyType { get; set; }
        public string AgencyApprovalWorkflow { get; set; }
        public SelectList AgencyTypeList { get; set; }
        public int Agency { get; set; }
        public SelectList AgencyList { get; set; }
        public int SubAgency { get; set; }
        public SelectList SubAgencyList { get; set; }
        public int CommitmentType { get; set; }
        public SelectList CommitmentTypeList { get; set; }
        public string JobTitle { get; set; }
        public int? PayRate { get; set; }
        public SelectList PayRateList { get; set; }
        public decimal? SalaryMin { get; set; }
        public decimal? SalaryMax { get; set; }
        public string PayPlan { get; set; }
        public string Series { get; set; }
        public string Grade { get; set; }
        public bool IsInternational { get; set; }
        public string Country { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public int? State { get; set; }
        public SelectList StateList { get; set; }
        public string SupervisorFirstName { get; set; }
        public string SupervisorLastName { get; set; }
        public string SupervisorEmailAddress { get; set; }
        public string SupervisorPhoneAreaCode { get; set; }
        public string SupervisorPhonePrefix { get; set; }
        public string SupervisorPhoneSuffix { get; set; }
        public string SupervisorPhoneExtension { get; set; }
        public string MentorFirstName { get; set; }
        public string MentorLastName { get; set; }
        public string MentorEmailAddress { get; set; }
        public string MentorPhoneAreaCode { get; set; }
        public string MentorPhonePrefix { get; set; }
        public string MentorPhoneSuffix { get; set; }
        public string MentorPhoneExtension { get; set; }
        public string StartDateMonth { get; set; }
        public string StartDateDay { get; set; }
        public string StartDateYear { get; set; }
        public string EndDateMonth { get; set; }
        public string EndDateDay { get; set; }
        public string EndDateYear { get; set; }
        public int? JobSearchType { get; set; }
        public SelectList JobSearchTypeList { get; set; }
        public string Justification { get; set; }
        public string Status { get; set; }
        public string ShowForm { get; set; }
        public List<SavedDocument> SavedDocuments { get; set; }

        public class SavedDocument
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Type { get; set; }
        }

        public class AgencyListDTO
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string ApprFlow { get; set; }
        }
    }
}
