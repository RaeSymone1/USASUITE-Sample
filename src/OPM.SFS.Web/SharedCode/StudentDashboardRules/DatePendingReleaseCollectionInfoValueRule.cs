using OPM.SFS.Data;
using System.Threading.Tasks;
using System;

namespace OPM.SFS.Web.SharedCode.StudentDashboardRules
{
	public class DatePendingReleaseCollectionInfoValueRule : IStudentDashboardUpdateRule
	{
		public Task<bool> CalculateDashboardFieldAsync(string value, StudentInstitutionFunding record)
		{
			if (!string.IsNullOrWhiteSpace(value) && value.Trim() != "TBD" && value.Trim() != "N/A")
			{
				record.DatePendingReleaseCollectionInfo = Convert.ToDateTime(value);
			}
			else
			{
				record.DatePendingReleaseCollectionInfo = null;
			}
			return System.Threading.Tasks.Task.FromResult(true);
		}
	}
}
