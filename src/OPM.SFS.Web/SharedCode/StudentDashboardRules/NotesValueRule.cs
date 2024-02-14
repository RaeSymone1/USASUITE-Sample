using OPM.SFS.Data;
using System.Threading.Tasks;

namespace OPM.SFS.Web.SharedCode.StudentDashboardRules
{
    public class NotesValueRule : IStudentDashboardUpdateRule
    {
        public Task<bool> CalculateDashboardFieldAsync(string value, StudentInstitutionFunding record)
        {
            if(!string.IsNullOrWhiteSpace(value) && value.Trim() != "N/A")
            {
                record.Notes = value;
            }
            else
            {
                record.Notes = null;
            }
            return System.Threading.Tasks.Task.FromResult(true);
        }
        
    }
}
