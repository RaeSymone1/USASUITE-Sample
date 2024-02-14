using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPM.SFS.Data
{
    [Table("StudentAccount")]
    public class StudentAccount
    {
        public StudentAccount()
        {
            StudentPasswordHistories = new HashSet<StudentAccountPasswordHistory>();
        }
        public int StudentAccountID { get; set; }
        public int StudentID { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool IsDisabled { get; set; }
        public int FailedLoginCount { get; set; }
        public DateTime? FailedLoginDate { get; set; }
        public DateTime? PasswordExpiration { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public DateTime? LockedOutDate { get; set; }
        public string PasswordCrypto { get; set; }
        public bool? ForcePasswordReset { get; set; }
        public virtual ICollection<StudentAccountPasswordHistory> StudentPasswordHistories { get; set; }
    }
}
