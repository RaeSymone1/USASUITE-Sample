namespace OPM.SFS.Web.Models
{
    public class RegisterPivViewModel
    {
        public string Email { get; set; }
        public string Message { get; set; }
        public string ErrorMessage { get; set; }
    }

    public class RegisterPIVResult
    {
        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; }
        public string Message { get; set; }
    }
}
