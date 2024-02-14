using OPM.SFS.Data;
using System;
using System.Threading.Tasks;

namespace OPM.SFS.Web.SharedCode.StudentDashboardRules
{
	public class SOCVerificationCompleteValueRule : IStudentDashboardUpdateRule
	{
		public Task<bool> CalculateDashboardFieldAsync(string value, StudentInstitutionFunding record)
		{
			if (!string.IsNullOrWhiteSpace(value) && value.Trim() != "N/A")
			{
				record.SOCVerificationComplete = Convert.ToDateTime(value);
			}
			else
			{
				record.SOCVerificationComplete = null;
			}
			return System.Threading.Tasks.Task.FromResult(true);
		}
	}
}
