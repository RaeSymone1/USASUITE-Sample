using System.Collections.Generic;

namespace OPM.SFS.Web.Models
{
    public class AdminStudentSearchViewModel
    {
        public string SearchOption { get; set; } 
        public string SearchFilter { get; set; }
        public List<StudentRecords> Students { get; set; }
        public string SearchResultsMessage { get; set; }
        public class StudentRecords
        {
            public int StudentID { get; set; }
            public int StudentUID { get; set; }
            public string SSN { get; set; }
            public string LastName { get; set; }
            public string FirstName { get; set; }
            public string Email { get; set; }
            public string Status { get; set; }
            public string Display { get; set; }
            public string BackgroundComplete { get; set; }
            public string ProfileComplete { get; set; }
        }
    }
}
