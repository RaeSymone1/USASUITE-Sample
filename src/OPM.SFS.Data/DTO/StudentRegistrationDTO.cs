using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPM.SFS.Core.DTO
{
    public class StudentRegistrationDTO
    {
        public int StudentID { get; set; }
        public string ProfileStatus { get; set; }
        public string Email { get; set; }
        public string SSN { get; set; }
    }
}
