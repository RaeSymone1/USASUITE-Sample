using Azure.Core;
using OPM.SFS.Data;
using System;
using System.Threading.Tasks;

namespace OPM.SFS.Web.SharedCode.StudentDashboardRules
{
	public class FollowUpDateValueRule : IStudentDashboardUpdateRule
	{
		public Task<bool> CalculateDashboardFieldAsync(string value, StudentInstitutionFunding record)
		{
			if (!string.IsNullOrWhiteSpace(value) && value.Trim() != "TBD" && value.Trim() != "N/A")
			{
				record.FollowUpDate = Convert.ToDateTime(value);
			}
			else
			{
				record.FollowUpDate = null;
			}
			return System.Threading.Tasks.Task.FromResult(true);
		}
	}
}
