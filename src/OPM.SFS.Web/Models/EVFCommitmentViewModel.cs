using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace OPM.SFS.Web.Models
{
    public class EVFCommitmentViewModel
    {
        public string StartDate { get; set; }
        public int CommitmentId { get; set; }
        public int StudentId { get; set; }
        public string JobTitle { get; set; }
        public string Agency { get; set; }
        public string SubAgency { get; set; }
        public int? ParentAgencyId { get; set; }
        public string StatusDisplay { get; set; }
        public string StatusCode { get; set; }
        public string StatusDescription { get; set; }
        public IFormFile UploadedDocument { get; set; }
        public string Training { get; set; }
        public string TakingRemedialTraining { get; set; }
        public string[] AnswerOptions { get; set; }
        public string IsSamePosition { get; set; }
        public string AgencySubAgencyDisplay { get; set; }
        public DateTime? PositionEndDate { get; set; }
        public string HasNewCommitment { get; set; }
        public List<EvfDocumentInfo> Documents { get; set; }       
        
    }

    public class EvfDocumentInfo
    {
        public string EVFDocumentName { get; set; }
        public string EVFDocumentSize { get; set; }
        public int EVFDocumentId { get; set; }
        public int CommitmentId { get; set; }
    }
}
