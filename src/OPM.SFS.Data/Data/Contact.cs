using System;
using System.Collections.Generic;

#nullable disable

namespace OPM.SFS.Data
{
    public partial class Contact
    {
        public Contact()
        {
            StudentCommitmentMentorContacts = new HashSet<StudentCommitment>();
            StudentCommitmentSupervisorContacts = new HashSet<StudentCommitment>();
            Students = new HashSet<Student>();
        }

        public int ContactId { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string PhoneExt { get; set; }
        public string Relationship { get; set; }

        public virtual ICollection<StudentCommitment> StudentCommitmentMentorContacts { get; set; }
        public virtual ICollection<StudentCommitment> StudentCommitmentSupervisorContacts { get; set; }
        public virtual ICollection<Student> Students { get; set; }
    }
}
