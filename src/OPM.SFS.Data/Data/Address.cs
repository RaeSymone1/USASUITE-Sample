using System;
using System.Collections.Generic;

#nullable disable

namespace OPM.SFS.Data
{
    public partial class Address
    {
        public Address()
        {
            StudentCommitments = new HashSet<StudentCommitment>();
            StudentCurrentAddresses = new HashSet<Student>();
            StudentPermanentAddresses = new HashSet<Student>();
        }

        public int AddressId { get; set; }
        public string LineOne { get; set; }
        public string LineTwo { get; set; }
        public string LineThree { get; set; }
        public string City { get; set; }
        public int StateId { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string PhoneNumber { get; set; }
        public string PhoneExtension { get; set; }
        public string Fax { get; set; }

        public virtual State State { get; set; }
        public virtual ICollection<StudentCommitment> StudentCommitments { get; set; }
        public virtual ICollection<Student> StudentCurrentAddresses { get; set; }
        public virtual ICollection<Student> StudentPermanentAddresses { get; set; }
    }
}
