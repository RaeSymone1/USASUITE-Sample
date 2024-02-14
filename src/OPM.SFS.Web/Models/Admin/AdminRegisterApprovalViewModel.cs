using System.Collections.Generic;

namespace OPM.SFS.Web.Models
{
    public class AdminRegisterApprovalViewModel
    {
        public int StudentPendingApproval { get; set; }
        public int AOPendingApproval { get; set; }
        public int PIPendingApproval { get; set; }
        public string Type { get; set; }
        public bool IsEnabledOnSite { get; set; }
		//public string UpdateStudent { get; set; }
		//public string UpdatePI { get; set; }
		//public string UpdateAO { get; set; }
		//public string Updates { get; set; }
		public List<UserItem> Accounts { get; set; }
        //public List<UserItem> AgencyUsers { get; set; }
        //public List<UserItem> AcademiaUsers { get; set; }        

        public class UserItem
        {
            public int ID { get; set; }
            public int UID { get; set; }
            public string LastName { get; set; }
            public string FirstName { get; set; }
            public string Instituion { get; set; }
            public int ApprovalStatus { get; set; }
            public string RadioGroupName { get; set; }
            public string RadioApprovedValue { get; set; }
            public string RadioRejectValue { get; set; }
            public string SubAgency { get; set; }
            public string Agency { get; set; }
            public string Telephone { get; set; }
            public string Email { get; set; }
            public string Status { get; set; }
            

        }

    }
}
