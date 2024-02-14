using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace OPM.SFS.Web.Models
{
    public class BackgroundInfoViewModel
    {
        public int Gender { get; set; }
        public SelectList GenderList { get; set; }
        public int Ethnicity { get; set; }
        public SelectList EthnicityList { get; set; }
        public List<int> SelectedRace { get; set; }
        public List<RaceOption> RaceOptions { get; set; }
        public int HighSchoolState { get; set; }
        public SelectList HighSchoolStateList { get; set; }
        public int HighSchoolType { get; set; }
        public SelectList HighSchoolTypeList { get; set; }
        public string IsArmedForces { get; set; }
        public SelectList IsArmedForcesList { get; set; }
        public int YearsInSecurityPosition { get; set; }
        public bool ShowSuccessMessage { get; set; }
        public bool ShowIncompleteWarning { get; set; }

        public class RaceOption
        {
            public string Name { get; set; }
            public int ID { get; set; }
            public bool Selected { get; set; }
        }

        public class ArmedForceOptions
        {
            public string Name { get; set; }
            public string ID { get; set; }
        }
    }
}
