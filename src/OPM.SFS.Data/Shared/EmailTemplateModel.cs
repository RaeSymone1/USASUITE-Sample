using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPM.SFS.Core.Shared
{
    public class EmailTemplateModel
    {
        public string StudentEmail { get; set; }
        public string StudentName { get; set; }
        public string StudentEmailAddress { get; set; }
        public string StudentAlternateEmailAddress { get; set; }
        public string MessageBody { get; set; }
        public string StudentFullName { get; set; }
        public string AgencyName { get; set; }
        public string SubAgency { get; set; }
        public string AgencyType { get; set; }
        public string CommitmentType { get; set; }
        public string JobTitle { get; set; }
        public string Justification { get; set; }
        public string StudentInstitution { get; set; }
        public string StudentEntryOnDuty { get; set; }
        public string StudentGraduationDate { get; set; }
        public string TypeOfMatch { get; set; }
        public string ManagerName { get; set; }
        public string ManagerEmail { get; set; }
        public string ManagerPhone { get; set; }
        public string SubAgencyName { get; set; }
        public string EntryOnDuty { get; set; }
        public string PIRecommendation { get; set; }
        public string RegistrationCode { get; set; }
        public string BaseURL { get; set; }
    }
}
