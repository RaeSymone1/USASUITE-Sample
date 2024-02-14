using OPM.SFS.Data;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System;
using OPM.SFS.Core.Shared;

namespace OPM.SFS.Web.SharedCode.StudentDashboardRules
{
	public class PGEmploymentDueDateValueRule : IStudentDashboardUpdateRule
	{
		private readonly IReferenceDataRepository _refRepo;
		private readonly IUtilitiesService _utilities;

		public PGEmploymentDueDateValueRule(IReferenceDataRepository refRepo, IUtilitiesService utilities )
		{
			_refRepo = refRepo;
			_utilities = utilities;

		}

		public async Task<bool> CalculateDashboardFieldAsync(string value, StudentInstitutionFunding record)
		{
			if (string.IsNullOrWhiteSpace(value) || value == "N/A" || (record.PGEmploymentDueDate.HasValue && record.PGEmploymentDueDate.Value == Convert.ToDateTime(value)))
			{
				if (!record.ExtensionTypeID.HasValue)
				{
					record.PGEmploymentDueDate = null;
				}
				else
				{
					var extensionType = await _refRepo.GetExtensionTypeAsync();
					int extensionMonths = extensionType.Where(m => m.ExtensionTypeID == record.ExtensionTypeID).Select(m => m.Months).FirstOrDefault();
					var gradDate = record.ExpectedGradDate;
					string newDueDate = CalculatePGEmploymentDate(gradDate, 18, extensionMonths);
					if (newDueDate != "N/A")
						record.PGEmploymentDueDate = Convert.ToDateTime(newDueDate);
				}

			}
			else
			{
				record.PGEmploymentDueDate = Convert.ToDateTime(value);
			}
			return true;
		}

		private string CalculatePGEmploymentDate(DateTime? expectedGradDate, int GracePeriod, int ExtensionMonths)
		{
			if (expectedGradDate.HasValue)
			{
				DateTime CalculatedDueDate = expectedGradDate ?? _utilities.ConvertUtcToEastern(DateTime.UtcNow);
				CalculatedDueDate = CalculatedDueDate.AddMonths((GracePeriod + ExtensionMonths));
				return CalculatedDueDate.ToString("MMM yyyy", CultureInfo.GetCultureInfo("en-US"));
			}
			else
				return "N/A";
		}
	}
}
