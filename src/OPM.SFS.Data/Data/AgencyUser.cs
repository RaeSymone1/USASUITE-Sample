using System;
using System.Collections.Generic;

#nullable disable

namespace OPM.SFS.Data
{
    public partial class AgencyUser
    {
        public AgencyUser()
        {
            AgencyUserPasswordHistories = new HashSet<AgencyUserPasswordHistory>();
        }        
        public int AgencyUserId { get; set; }
        public int? AgencyID { get; set; }
        public int? AgencyUserRoleID { get; set; }
        public string UserName { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime? DateInserted { get; set; }
        public DateTime? LastUpdated { get; set; }
        public DateTime? PasswordExpirationDate { get; set; }
        public int? FailedLoginCount { get; set; }
        public DateTime? FailedLoginDate { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public DateTime? LockedOutDate { get; set; }
        public string WebsiteUrl { get; set; }
        public bool? DisplayContactInfo { get; set; }
        public bool? IsDisabled { get; set; }
        public int ProfileStatusID { get; set; }
        public byte[] Thumbprint { get; set; }
        public string Issuer { get; set; }
        public string SerialNumber { get; set; }
        public string Subject { get; set; }
        public string SubjectAlternative { get; set; }
        public DateTime? ValidAfter { get; set; }
        public DateTime? ValidUntil { get; set; }
        public byte[] Certificate { get; set; }
        public bool EnforcePIV { get; set; }
        public DateTime PIVOverrideExpiration { get; set; }
        public string PasswordCrypto { get; set; }
        public bool? ForcePasswordReset { get; set; }
        public string LoginGovLinkID { get; set; }
        public DateTime? InactiveAccountReminderSentDate { get; set; }
        public virtual ICollection<AgencyUserPasswordHistory> AgencyUserPasswordHistories { get; set; }
        public virtual Address Address { get; set; }
        public virtual Agency Agency { get; set; }
        public virtual AgencyUserRole AgencyUserRole { get; set; }
        public virtual ProfileStatus ProfileStatus { get; set; }

    }
}
