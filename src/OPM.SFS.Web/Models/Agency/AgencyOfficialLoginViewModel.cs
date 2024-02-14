namespace OPM.SFS.Web.Models
{
    public class AgencyOfficialLoginViewModel
    {      
        public bool ShowPIVSuccessMessage { get; set; }
        public bool IsAccountInactive { get; set; }
        public string EncryptedStudentID { get; set; }
        public bool ShowSuccessEmail { get; set; }
        public string ReactivateUrl { get; set; }
    }

    public record AgencyOfficialLoginResult
    {
        public bool IsSuccessful { get; set; }
        public string ErrorMessage { get; set; }
        public string Redirect { get; internal set; }
    }
}
