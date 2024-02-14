using OPM.SFS.Data;
using System.Threading.Tasks;
using System;

namespace OPM.SFS.Web.SharedCode.StudentDashboardRules
{
	public class PGVerificationOneDueDateValueRule : IStudentDashboardUpdateRule
	{
		public Task<bool> CalculateDashboardFieldAsync(string value, StudentInstitutionFunding record)
		{
			if (!string.IsNullOrWhiteSpace(value) && value.Trim() != "N/A")
				record.PGVerificationOneDueDate = Convert.ToDateTime(value);
			else if (record.PostGradEOD.HasValue && record.ServiceOwed.HasValue)
			{
				var verficationdate = CalculateCommitmentVerificationOne(record.PostGradEOD, record.ServiceOwed.Value);
				record.PGVerificationOneDueDate = verficationdate;

			}
			else
			{
				record.PGVerificationOneDueDate = null;
			}
			return System.Threading.Tasks.Task.FromResult(true);
		}

		private DateTime? CalculateCommitmentVerificationOne(DateTime? commitmentStartDate, double ServiceOwed)
		{

			if (ServiceOwed.Equals("1"))
				return null;
			if (commitmentStartDate.HasValue)
				return commitmentStartDate.Value.AddYears(1);
			return null;
		}

	}
}
