using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace OPM.SFS.Web.Models
{
    public class AdminEditUserViewModel
    {
        public string AccountType { get; set; }
        public int AgencyID { get; set; }
        public int Agency { get; set; }
        public int? SubAgency { get; set; }
        public int Institution { get; set; }
        public SelectList InstitutionList { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Phone { get; set; }
        public string PhoneExt { get; set; }
        public string Fax { get; set; }
        public int ProfileStatusID { get; set; }
        public SelectList ProfileStatusList { get; set; }
        public int ID { get; set; }
        public SelectList AgencyTypeList { get; set; }
        public SelectList AgencyList { get; set; }
        public SelectList SubAgencyList { get; set; }
        public SelectList StateList { get; set; }
        public bool IsProfileDisabled { get; set; }
        public int? StateFilter { get; set; }
        public int? AgencyType { get; set; }

    }
}
