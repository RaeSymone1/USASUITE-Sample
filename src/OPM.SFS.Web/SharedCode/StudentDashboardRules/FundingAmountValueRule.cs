using OPM.SFS.Data;
using System.Threading.Tasks;

namespace OPM.SFS.Web.SharedCode.StudentDashboardRules
{
	public class FundingAmountValueRule : IStudentDashboardUpdateRule
	{
		public Task<bool> CalculateDashboardFieldAsync(string value, StudentInstitutionFunding record)
		{
			if (!string.IsNullOrEmpty(value) && value.Trim() != "N/A")
			{
				decimal decValue;
				if (decimal.TryParse(value, out decValue))
					record.FundingAmount = decValue;
			}
			else
			{
				record.FundingAmount = null;
			}
			return System.Threading.Tasks.Task.FromResult(true);
		}

	}
}
