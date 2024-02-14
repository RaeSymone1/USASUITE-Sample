using System.Collections.Generic;

namespace OPM.SFS.Web.Models
{
    public class ParticipatingInstitutionsVM
    {
        public List<InstitutionLink> C3P { get; set; }
        public List<InstitutionLink> FourYear { get; set; }
        //public List<InstitutionLink> Pathways { get; set; }
        public List<InstitutionDetails> AllInstitions { get; set; }
        public Dictionary<string, List<InstitutionDetails>> AllInstitionsC3P { get; set; }
        public Dictionary<string, List<InstitutionDetails>> AllInstitions4Year { get; set; }
        public Dictionary<string, List<InstitutionDetails>> AllActiveInstitutions { get; set; }

        public class StateVM
        {           
            public string Color { get; set; }
            public string Description { get; set; }
            public string Url { get; set; }
            public string Hover_color { get; set; }
            public string Hide { get; set; }
            public string Name { get; set; }
        }
        public class InstitutionLink
        {
            public string Name { get; set; }
            public string ID { get; set; }
        }

        public class InstitutionDetails
        {
            public int ID { get; set; }
            public string Name { get; set; }
            public string Link { get; set; }
            public string AddressLine { get; set; }
            public string ProgramPage { get; set; }
            public bool IsAcceptingApplications { get; set; }
            public List<Contact> Contacts { get; set; }
            public string Type { get; set; }

            public class Contact
            {
                public string Name { get; set; }
                public string Phone { get; set; }
                public string Role { get; set; }
                public string Email { get; set; }
            }
        }
    }

   
       
    

}
