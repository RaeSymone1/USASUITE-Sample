using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace OPM.SFS.Web.Models
{
    public class CommitmentDocumentViewModel
    {
        public int CommitmentID { get; set; }
        public string ApprovalFlow { get; set; }
        public string Status { get; set; }
        public List<SavedDocument> SavedDocuments { get; set; }
        public IFormFile PostionDescription { get; set; }
        public IFormFile FinalJobOffer { get; set; }
        public IFormFile TenativeJobOffer { get; set; }
        public int SavedDocumentCount { get; set; }
        public int StudentID { get; set; }
        public bool CanSubmit { get; set; }
        public bool HideUploadForFinalJobLetter { get; set; }
        public bool HideUploadForPositionDescription { get; set; }
        public bool HideUploadForTenative { get; set; }


        public class SavedDocument
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Type { get; set; }
        }
    }
}
