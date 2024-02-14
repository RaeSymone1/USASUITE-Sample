using Azure.Core;
using OPM.SFS.Data;
using System.Threading.Tasks;

namespace OPM.SFS.Web.SharedCode.StudentDashboardRules
{
	public class FollowUpActionValueRule : IStudentDashboardUpdateRule
	{
		public Task<bool> CalculateDashboardFieldAsync(string value, StudentInstitutionFunding record)
		{
			if (!string.IsNullOrWhiteSpace(value) && value.Trim() != "TBD" && value.Trim() != "N/A")
			{
				record.FollowUpAction = value;
			}
			else
			{
				record.FollowUpAction = null;
			}
			return System.Threading.Tasks.Task.FromResult(true);
		}
	}
}
