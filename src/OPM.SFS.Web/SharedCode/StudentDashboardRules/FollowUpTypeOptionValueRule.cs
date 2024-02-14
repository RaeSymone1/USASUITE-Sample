using Azure.Core;
using OPM.SFS.Data;
using System.Linq;
using System.Threading.Tasks;

namespace OPM.SFS.Web.SharedCode.StudentDashboardRules
{
	public class FollowUpTypeOptionValueRule : IStudentDashboardUpdateRule
	{
		private readonly IReferenceDataRepository _refRepo;

		public FollowUpTypeOptionValueRule(IReferenceDataRepository refRepo)
		{
			_refRepo = refRepo;
		}
		public async Task<bool> CalculateDashboardFieldAsync(string value, StudentInstitutionFunding record)
		{
			if (!string.IsNullOrWhiteSpace(value) && value.Trim() != "TBD" && value.Trim() != "N/A")
			{
				var followUpTypes = await _refRepo.GetFollowUpTypeOptionsAsync();
				int typeID = followUpTypes.Where(m => m.Name == value).Select(m => m.FollowUpTypeOptionID).FirstOrDefault();
				record.FollowUpTypeOptionID = typeID;
			}
			else
			{
				record.FollowUpTypeOptionID = null;
			}
			return true;
		}
	}
}
