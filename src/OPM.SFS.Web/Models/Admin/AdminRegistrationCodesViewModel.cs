using System.Collections.Generic;

namespace OPM.SFS.Web.Models
{
    public class AdminRegistrationCodesViewModel
    {

        public List<RegistrationCodeItem> RegistrationCodes { get; set; }

        public class RegistrationCodeItem
        {
            public string Quartername { get; set; }
            public string Code { get; set; }
        }
    }
}
