using OPM.SFS.Data;
using System.Threading.Tasks;
using System;

namespace OPM.SFS.Web.SharedCode.StudentDashboardRules
{
	public class DateLeftPGEarlyValueRule : IStudentDashboardUpdateRule
	{
		public DateLeftPGEarlyValueRule()
		{

		}
		public Task<bool> CalculateDashboardFieldAsync(string value, StudentInstitutionFunding record)
		{
			if (!string.IsNullOrEmpty(value) && value.Trim() != "N/A")
			{
				record.DateLeftPGEarly = Convert.ToDateTime(value);
			}
			else
			{
				record.DateLeftPGEarly = null;
			}
			return System.Threading.Tasks.Task.FromResult(true);
		}
	}
}
