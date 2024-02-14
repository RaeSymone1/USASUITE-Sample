using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace OPM.SFS.Web.Models.Agency
{
   
    public class AgencyOfficialStudentSearchViewModel
    {        
        public int? Degree { get; set; }
        public SelectList DegreeList { get; set; }
        public int? Institition { get; set; }
        public SelectList InstitutionList { get; set; }
        public int? Discipline { get; set; }
        public SelectList DisciplineList { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public bool ShowResults { get; set; }
        public List<StudentResult> SearchResults { get; set; }
        public int? InternQuarter { get; set; }
        public int? InternYear { get; set; }
        public int? PostGradQuarter { get; set; }
        public int? PostGradYear { get; set; }
        public SelectList QuarterList { get; set; }


        public SelectList YearList { get; set; }


        public class StudentResult
        {

            public string StudentName { get; set; }
            public string Degree { get; set; }
            public string Institution { get; set; }
            public string Discipline { get; set; }
            public string InternshipDate { get; set; }
            public string PostGradDate { get; set; }
            public int StudentID { get; set; }

        }

        public class SelectListOption
        {
            public int ID { get; set; }
            public string Value { get; set; }
        }


    }
}
