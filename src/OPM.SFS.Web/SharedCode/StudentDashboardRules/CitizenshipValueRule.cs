using OPM.SFS.Data;
using System.Linq;
using System.Threading.Tasks;

namespace OPM.SFS.Web.SharedCode.StudentDashboardRules
{
	public class CitizenshipValueRule 
	{
		private readonly IReferenceDataRepository _refRepo;

		public CitizenshipValueRule(IReferenceDataRepository refRepo)
		{
			_refRepo = refRepo;
		}

		public async Task<bool> CalculateDashboardFieldAsync(string value, Student record)
		{
			if(!string.IsNullOrWhiteSpace(value) && value.Trim() != "N/A")
			{
				var lstValues = await _refRepo.GetCitizenshipAsync();
				record.CitizenshipID = lstValues.Where(m => m.Value == value).Select(m => m.CitizenshipID).FirstOrDefault();
			}
			else
			{
				record.CitizenshipID = null;
			}
			return true;
		}
	}
}
