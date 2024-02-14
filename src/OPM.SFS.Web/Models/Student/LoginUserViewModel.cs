namespace OPM.SFS.Web.Models
{
    public class LoginUserViewModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public bool IsAccountInactive { get; set; }
        public string EncryptedStudentID { get; set; }
        public bool ShowSuccessEmail { get; set; }
        public string ReactivateUrl { get; set; }
    }

    public record LoginResult
    {
        public bool IsSuccessful { get; set; }
        public string ErrorMessage { get; set; }
        public string Redirect { get; internal set; }
    }
}
