using OPM.SFS.Data;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace OPM.SFS.Web.SharedCode.StudentDashboardRules
{
	public class ExtensionTypeValueRule : IStudentDashboardUpdateRule
	{
		private readonly IReferenceDataRepository _refRepo;

		public ExtensionTypeValueRule(IReferenceDataRepository refRepo)
		{
			_refRepo = refRepo;
		}

		public async Task<bool> CalculateDashboardFieldAsync(string value, StudentInstitutionFunding record)
		{
			if (!string.IsNullOrWhiteSpace(value) && value != "N/A")
			{
				var extensionType = await _refRepo.GetExtensionTypeAsync();
				record.ExtensionTypeID = String.IsNullOrEmpty(value) ? null : extensionType.Where(x => x.Extension == value).Select(x => x.ExtensionTypeID).FirstOrDefault();
			}
			if (value == "N/A")
				record.ExtensionTypeID = null;

			return true;
		}
	}
}
