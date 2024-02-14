using OPM.SFS.Data;
using System;
using System.Threading.Tasks;

namespace OPM.SFS.Web.SharedCode.StudentDashboardRules
{
	public class DateReleasedCollectionPackageValueRule : IStudentDashboardUpdateRule
	{
		public Task<bool> CalculateDashboardFieldAsync(string value, StudentInstitutionFunding record)
		{
			if (!string.IsNullOrWhiteSpace(value) && value.Trim() != "TBD" && value.Trim() != "N/A")
			{
				record.DateReleasedCollectionPackage = Convert.ToDateTime(value);
			}
			else
			{
				record.DateReleasedCollectionPackage = null;
			}
			return System.Threading.Tasks.Task.FromResult(true);
		}
	}
}
