using OPM.SFS.Data;
using System;
using System.Threading.Tasks;

namespace OPM.SFS.Web.SharedCode.StudentDashboardRules
{
    public class LastUpdateReceivedValueRule 
    {
        public Task<bool> CalculateDashboardFieldAsync(string value, Student record)
        {
            if (!string.IsNullOrEmpty(value) && value.Trim() != "N/A")
            {
                record.LastUpdated = Convert.ToDateTime(value);
            }
            else
            {
                record.LastUpdated = null;
            }
            return System.Threading.Tasks.Task.FromResult(true);
        }
    }
}
