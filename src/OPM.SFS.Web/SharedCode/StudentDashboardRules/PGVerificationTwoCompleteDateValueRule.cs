using OPM.SFS.Data;
using System.Threading.Tasks;
using System;

namespace OPM.SFS.Web.SharedCode.StudentDashboardRules
{
	public class PGVerificationTwoCompleteDateValueRule : IStudentDashboardUpdateRule
	{
		public Task<bool> CalculateDashboardFieldAsync(string value, StudentInstitutionFunding record)
		{
			if (!string.IsNullOrWhiteSpace(value) && value.Trim() != "N/A")
			{
				record.PGVerificationTwoCompleteDate = Convert.ToDateTime(value);
			}
			else
			{
				record.PGVerificationTwoCompleteDate = null;
			}
			return System.Threading.Tasks.Task.FromResult(true);
		}
	}
}
