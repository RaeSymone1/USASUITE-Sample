using Azure.Core;
using OPM.SFS.Data;
using System;
using System.Threading.Tasks;

namespace OPM.SFS.Web.SharedCode.StudentDashboardRules
{
	public class CommitmentPhaseCompleteValueRule : IStudentDashboardUpdateRule
	{
		public Task<bool> CalculateDashboardFieldAsync(string value, StudentInstitutionFunding record)
		{
			if(!string.IsNullOrWhiteSpace(value) && value.Trim() != "N/A")
			{
				record.CommitmentPhaseComplete = Convert.ToDateTime(value);
			}
			else if (record.PostGradEOD.HasValue && record.ServiceOwed.HasValue)
			{
				record.CommitmentPhaseComplete = CalculateCommitmentPhaseComplete(record.PostGradEOD, record.ServiceOwed.Value);
			}
			else
			{
				record.CommitmentPhaseComplete = null;
			}
			return Task.FromResult(true);
		}

		public DateTime? CalculateCommitmentPhaseComplete(DateTime? commitmentStartDate, double serviceOwed)
		{
			if (commitmentStartDate.HasValue)
			{
				int months = (int)(serviceOwed * 12);
				return commitmentStartDate.Value.AddMonths(months);
			}

			return null;
		}
	}
}
