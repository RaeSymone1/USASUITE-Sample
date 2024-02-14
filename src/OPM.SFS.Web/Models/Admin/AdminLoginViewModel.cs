namespace OPM.SFS.Web.Models
{
    public class AdminLoginViewModel
    {
        public string UserId { get; set; }
        public string Password { get; set; }
        public bool ShowPIVSuccessMessage { get; set; }
        public bool ShowEnforcePIVMessage { get; set; }

        public bool IsAccountInactive { get; set; }
        public string EncryptedStudentID { get; set; }
        public bool ShowSuccessEmail { get; set; }
        public string ReactivateUrl { get; set; }

        public class LoginUserResult
        {
            public bool IsSuccessful { get; set; }
            public string ErrorMessage { get; set; }
            public string Redirect { get; set; }
        }
    }

    
}
