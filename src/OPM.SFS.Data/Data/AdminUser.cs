using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace OPM.SFS.Data
{
    public partial class AdminUser
    {
        public AdminUser()
        {
            AdminUserPasswordHistories = new HashSet<AdminUserPasswordHistory>();
        }
        public int AdminUserId { get; set; }
        public int? AdminUserRoleID { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public string PhoneExt { get; set; }
        public string Fax { get; set; }
        public DateTime? DateInserted { get; set; }
        public DateTime? LastUpdated { get; set; }
        public DateTime? PasswordExpirationDate { get; set; }
        public int? FailedLoginCount { get; set; }
        public DateTime? FailedLoginDate { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public DateTime? LockedOutDate { get; set; }
        public bool EnforcePIV { get; set; }
        public DateTime? ROBExpiration { get; set; }
        public DateTime PIVOverrideExpiration { get; set; }
        public string PasswordCrypto { get; set; }
        public bool? ForcePasswordReset { get; set; }
        public bool IsDisabled { get; set; }
        public byte[] Thumbprint { get; set; }
        public string Issuer { get; set; }
        public string SerialNumber { get; set; }
        public string Subject { get; set; }
        public string SubjectAlternative { get; set; }
        public string LoginGovLinkID { get; set; }
        public DateTime? ValidAfter { get; set; }
        public DateTime? ValidUntil { get; set; }
        public byte[] Certificate { get; set; }
        public DateTime? InactiveAccountReminderSentDate { get; set; }


        public virtual AdminUserRole AdminUserRole { get; set; }
        public virtual ICollection<AdminUserPasswordHistory> AdminUserPasswordHistories { get; set; }
    }
}
