using Microsoft.AspNetCore.Http;

namespace OPM.SFS.Web.Shared
{
    public class CommitmentDocumentValidator
    {
        public CommitmentDocumentValidator()
        {

        }

        public ValidationResult IsValidDocumentForUpload(IFormFile document)
        {
            if (document == null) return new ValidationResult() { IsSuccess = false, Message = "Document is required." };
            if(!IsValidDocType(document.ContentType)) return new ValidationResult() { IsSuccess = false, Message = "Files must be one of the following formats: TXT, PDF, Word (DOC or DOCX) or Excel (XLS, XSLX)" };
            if(document.Length > 5242880) return new ValidationResult() { IsSuccess = false, Message = "File size exceeds 5MB." };
            return new ValidationResult() { IsSuccess = true };
        }

        //public ValidationResult IsDocumentCompleteForCommitment()
        //{

        //}

        private bool IsValidDocType(string conentType)
        {
            if (conentType.ToLower() == "application/pdf") //pdf
                return true;
            if (conentType.ToLower() == "application/msword") //word (doc)
                return true;
            if (conentType.ToLower() == "application/vnd.openxmlformats-officedocument.wordprocessingml.document") //word (docx)
                return true;
            if (conentType.ToLower() == "text/plain") //txt
                return true;
            if (conentType.ToLower() == "application/vnd.ms-excel") //Excel (xls)
                return true;
            if (conentType.ToLower() == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet") //excel (xlst)
                return true;
            return false;
        }

        public class ValidationResult
        {
            public bool IsSuccess { get; set; }
            public string Message { get; set; }
        }
    }
}
