using OPM.SFS.Data;
using System.Threading.Tasks;
using System;

namespace OPM.SFS.Web.SharedCode.StudentDashboardRules
{
	public class PGVerificationOneCompleteDateValueRule : IStudentDashboardUpdateRule
	{
		public Task<bool> CalculateDashboardFieldAsync(string value, StudentInstitutionFunding record)
		{
			if (!string.IsNullOrWhiteSpace(value) && value.Trim() != "N/A")
			{
				record.PGVerificationOneCompleteDate = Convert.ToDateTime(value);
			}
			else
			{
				record.PGVerificationOneCompleteDate = null;
			}
			return System.Threading.Tasks.Task.FromResult(true);
		}
	}
}
