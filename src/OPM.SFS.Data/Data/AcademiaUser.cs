using System;
using System.Collections.Generic;

#nullable disable

namespace OPM.SFS.Data
{
    public partial class AcademiaUser
    {
        public AcademiaUser()
        {
            AcademiaUserPasswordHistories = new HashSet<AcademiaUserPasswordHistory>();
        }
        public int AcademiaUserId { get; set; }
        public int InstitutionID { get; set; }
        public int? AcademiaUserRoleID { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Department { get; set; }
        public string Password { get; set; }
        public DateTime? DateInserted { get; set; }
        public DateTime? LastUpdated { get; set; }
        public DateTime? PasswordExpirationDate { get; set; }
        public int? FailedLoginCount { get; set; }
        public DateTime? FailedLoginDate { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public DateTime? LockedOutDate { get; set; }
        public string WebsiteUrl { get; set; }
        public int ProfileStatusID { get; set; }
        public string PasswordCrypto { get; set; }
        public bool? ForcePasswordReset { get; set; }
        public string LoginGovLinkID { get; set; }
        public virtual Institution Institution { get; set; }
        public virtual AcademiaUserRole AcademiaUserRole { get; set; }
        public virtual Address Address { get; set; }
        public virtual ProfileStatus ProfileStatus { get; set; }
        public DateTime? InactiveAccountReminderSentDate { get; set; }
        public virtual ICollection<AcademiaUserPasswordHistory> AcademiaUserPasswordHistories { get; set; }
    }
}
