using System;
using System.Collections.Generic;

#nullable disable

namespace OPM.SFS.Data
{
    public partial class StudentJobActivity
    {
        public int StudentJobActivityId { get; set; }
        public int StudentId { get; set; }
        public int? JobActivityStatusID { get; set; }
        public int? AgencyTypeId { get; set; }
        public string Agency { get; set; }
        public string ContactFirstName { get; set; }
        public string ContactLastName { get; set; }
        public string ContactPhone { get; set; }
        public string ContactEmail { get; set; }
        public string Description { get; set; }
        public DateTime? DateApplied { get; set; }
        public DateTime? LastUpdated { get; set; }
        public DateTime? DateInserted { get; set; }
        public string PositionTitle { get; set; }
        public string UsajobscontrolNumber { get; set; }
        public string DutyLocation { get; set; }
        public string StatusOther { get; set; }
        public virtual AgencyType AgencyType { get; set; }
        public virtual JobActivityStatus JobActivityStatus { get; set; }
        public virtual Student Student { get; set; }


    }
}
