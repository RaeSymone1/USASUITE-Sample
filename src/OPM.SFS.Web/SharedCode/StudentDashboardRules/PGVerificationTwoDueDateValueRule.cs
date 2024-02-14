using OPM.SFS.Data;
using System.Threading.Tasks;
using System;

namespace OPM.SFS.Web.SharedCode.StudentDashboardRules
{
	public class PGVerificationTwoDueDateValueRule : IStudentDashboardUpdateRule
	{
		public Task<bool> CalculateDashboardFieldAsync(string value, StudentInstitutionFunding record)
		{
			if (!string.IsNullOrWhiteSpace(value) && value.Trim() != "N/A")
				record.PGVerificationTwoDueDate = Convert.ToDateTime(value);
			else if (record.PostGradEOD.HasValue && record.ServiceOwed.HasValue)
			{
				var verficationdate = CalculateCommitmentVerificationTwo(record.PostGradEOD, record.ServiceOwed.Value);
				record.PGVerificationTwoDueDate = verficationdate;

			}
			else
			{
				record.PGVerificationTwoDueDate = null;
			}
			return System.Threading.Tasks.Task.FromResult(true);
		}

		public DateTime? CalculateCommitmentVerificationTwo(DateTime? commitmentStartDate, Double? ServiceOwed)
		{

			if (ServiceOwed < 3)
				return null;

			if (commitmentStartDate.HasValue)
				return commitmentStartDate.Value.AddYears(2);
			return null;
		}
	}
}
