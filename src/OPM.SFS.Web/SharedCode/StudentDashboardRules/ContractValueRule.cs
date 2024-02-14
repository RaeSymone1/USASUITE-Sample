using System.Linq;
using System.Threading.Tasks;

namespace OPM.SFS.Web.SharedCode.StudentDashboardRules
{
	public class ContractValueRule : IStudentDashboardUpdateRule
	{
		private readonly IReferenceDataRepository _refRepo;

		public ContractValueRule(IReferenceDataRepository refRepo)
		{
			_refRepo = refRepo;
		}

		public async Task<bool> CalculateDashboardFieldAsync(string value, SFS.Data.StudentInstitutionFunding record)
		{

			if (!string.IsNullOrWhiteSpace(value) && value != "N/A")
			{
				var contracts = await _refRepo.GetContractAsync();
				var result = contracts.Where(m => m.Name == value).Select(m => m.ContractId).FirstOrDefault();
				record.ContractId = result;
			}
			if (value == "N/A")
				record.ContractId = null;
			return true;
		}
	}
}
