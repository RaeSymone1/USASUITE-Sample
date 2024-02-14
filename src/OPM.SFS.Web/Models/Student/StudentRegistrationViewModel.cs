using Microsoft.AspNetCore.Mvc.Rendering;

namespace OPM.SFS.Web.Models.Student
{
    public class StudentRegistrationViewModel
    {
        public long UID { get; set; }
        public string Firstname { get; set; }
        public string Middlename { get; set; }
        public string Lastname { get; set; }
        public string Suffix { get; set; }
        public string DateOfBirth { get; set; }
        public string SSN { get; set; }
        public string ConfirmSSN { get; set; }
        public int? SelectedCollege { get; set; }
        public int? SelectedDiscipline { get; set; }
        public int? SelectedDegree { get; set; }
        public int? SelectedMinor { get; set; }
        public int? SelectedSecondDegreeMajor { get; set; }
        public int? SelectedSecondDegreeMinor { get; set; }
        public string InitialFundingDate { get; set; }
        public string GraduationDate { get; set; }
        public string Email { get; set; }
        public string AlternateEmail { get; set; }
        public SelectList Institutions { get; set; }
        public SelectList Disciplines { get; set; }
        public SelectList Degrees { get; set; }
        public SelectList DegreeMinors { get; set; }
        public bool IsValidRegistrationAttempt { get; set; }
    }
}
