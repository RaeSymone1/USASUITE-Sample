using OPM.SFS.Data;
using System.Linq;
using System.Threading.Tasks;

namespace OPM.SFS.Web.SharedCode.StudentDashboardRules
{
	public class StatusValueRule 
	{
		private readonly IReferenceDataRepository _refRepo;

		public StatusValueRule(IReferenceDataRepository refRepo)
		{
			_refRepo = refRepo;
		}

		public async Task<bool> CalculateDashboardFieldAsync(string value, StudentInstitutionFunding record)
		{
			if (!string.IsNullOrWhiteSpace(value) && value != "N/A")
			{
				var statusoption = await _refRepo.GetStatusOptionsAsync();
				record.StatusID = statusoption.Where(m => m.Status == value).Select(m => m.StudentStatusId).FirstOrDefault();
			}
			if (value == "N/A")
				record.StatusID = null;
			return true;
		}
	}
}
