using OPM.SFS.Data;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace OPM.SFS.Web.SharedCode.StudentDashboardRules
{
	public class ServiceOwedValueRule : IStudentDashboardUpdateRule
	{
		private readonly IReferenceDataRepository _refRepo;
		private readonly IServiceOwedService _serviceowed;

		public ServiceOwedValueRule(IReferenceDataRepository refRepo, IServiceOwedService serviceowed)
		{
			_refRepo = refRepo;
			_serviceowed = serviceowed;
		}

		public async Task<bool> CalculateDashboardFieldAsync(string value, StudentInstitutionFunding record)
		{
			var institutions = await _refRepo.GetInstitutionsAsync();
			string academicSchedule = institutions.Where(m => m.InstitutionId == record.InstitutionId).Select(m => m.AcademicSchedule.Name).FirstOrDefault();
			if (record.TotalAcademicTerms != String.Empty && int.TryParse(record.TotalAcademicTerms, out int terms))
			{
				ServiceOwed serviceowed = _serviceowed.CalculateServiceOwedbyTerms(academicSchedule, terms);
				if (serviceowed.ex.Length > 0)
				{
					return false;
				}
				else
				{
					record.ServiceOwed = serviceowed.ServiceTime;
				}
			}
			else if (record.EnrolledYear.HasValue && !string.IsNullOrWhiteSpace(record.EnrolledSession) && record.FundingEndYear.HasValue && !string.IsNullOrWhiteSpace(record.FundingEndSession)
				&& record.EnrolledYear.HasValue && !string.IsNullOrWhiteSpace(record.EnrolledSession) && record.FundingEndYear.HasValue && !string.IsNullOrWhiteSpace(record.FundingEndSession))
			{
				ServiceOwed serviceowed = _serviceowed.CalculateServiceOwedbySeason(academicSchedule, record.EnrolledSession, record.EnrolledYear.Value, record.FundingEndSession, record.FundingEndYear.Value);
				if (serviceowed.ex.Length > 0)
				{
					return false;
				}
				else
				{
					record.ServiceOwed = serviceowed.ServiceTime;
				}
			}
			return true;
		}


	}
}
