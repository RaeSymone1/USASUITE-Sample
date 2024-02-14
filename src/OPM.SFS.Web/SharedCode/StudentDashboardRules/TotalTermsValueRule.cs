using OPM.SFS.Data;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace OPM.SFS.Web.SharedCode.StudentDashboardRules
{
	public class TotalTermsValueRule : IStudentDashboardUpdateRule
	{
		enum InstitutionTermCount { Trimester = 3, Semester = 2, Quarter = 3 };
		enum QuarterStart { Winter = 0, Spring = -1, Fall = -2 }
		enum QuarterEnd { Winter = -2, Spring = -1, Fall = 0 }
		enum TrimesterStart { Winter = 0, Spring = -1, Fall = -2 }
		enum TrimesterEnd { Winter = -2, Spring = -1, Fall = 0 }
		enum SemesterStart { Spring = 0, Fall = -1 }
		enum SemesterEnd { Spring = -1, Fall = 0 }
		private readonly IReferenceDataRepository _refRepo;

		public TotalTermsValueRule(IReferenceDataRepository refRepo)
		{
			_refRepo = refRepo;
		}

		public async Task<bool> CalculateDashboardFieldAsync(string value, StudentInstitutionFunding record)
		{
			var institutions = await _refRepo.GetInstitutionsAsync();
			string academicSchedule = institutions.Where(m => m.InstitutionId == record.InstitutionId).Select(m => m.AcademicSchedule.Name).FirstOrDefault();
			if (!string.IsNullOrWhiteSpace(value) && value.Trim() != "N/A")
			{
				record.TotalAcademicTerms = value;
			}
			else
			{
				if (record.EnrolledYear.HasValue && !string.IsNullOrWhiteSpace(record.EnrolledSession) && record.FundingEndYear.HasValue && !string.IsNullOrWhiteSpace(record.FundingEndSession)
				&& record.EnrolledYear.HasValue && !string.IsNullOrWhiteSpace(record.EnrolledSession) && record.FundingEndYear.HasValue && !string.IsNullOrWhiteSpace(record.FundingEndSession))
				{
					record.TotalAcademicTerms = CalculateTotalTerms(record.EnrolledYear.ToString(), record.EnrolledSession, record.FundingEndYear.ToString(), record.FundingEndSession, academicSchedule);
				}

			}
			return true;
		}

		private string CalculateTotalTerms(string EnrolledYear, string EnrolledSession, string FundingEndYear, string FundingEndSession, string AcademicSchedule)
		{
			int totalTerm = 0;
			int termCount = 0;
			int startTerm = 0;
			int endTerm = 0;

			if (!string.IsNullOrWhiteSpace(EnrolledYear) & !string.IsNullOrWhiteSpace(EnrolledSession) & !string.IsNullOrWhiteSpace(FundingEndYear) & !string.IsNullOrWhiteSpace(FundingEndSession))
			{
				switch (AcademicSchedule)
				{
					case "Semester":
						termCount = (int)InstitutionTermCount.Semester;
						switch (EnrolledSession)
						{
							case "Spring": startTerm = (int)SemesterStart.Spring; break;
							case "Fall": startTerm = (int)SemesterStart.Fall; break;
						}
						switch (FundingEndSession)
						{
							case "Spring": endTerm = (int)SemesterEnd.Spring; break;
							case "Fall": endTerm = (int)SemesterEnd.Fall; break;
						}
						break;
					case "Trimister":
						termCount = (int)InstitutionTermCount.Trimester;
						switch (EnrolledSession)
						{
							case "Spring": startTerm = (int)TrimesterStart.Spring; break;
							case "Fall": startTerm = (int)TrimesterStart.Fall; break;
							case "Winter": startTerm = (int)TrimesterStart.Fall; break;
						}
						switch (FundingEndSession)
						{
							case "Spring": endTerm = (int)TrimesterEnd.Spring; break;
							case "Fall": endTerm = (int)TrimesterEnd.Fall; break;
							case "Winter": endTerm = (int)TrimesterEnd.Winter; break;
						}
						break;
					case "Quarter":
						termCount = (int)InstitutionTermCount.Quarter;
						switch (EnrolledSession)
						{
							case "Spring": startTerm = (int)QuarterStart.Spring; break;
							case "Fall": startTerm = (int)QuarterStart.Fall; break;
							case "Winter": startTerm = (int)QuarterStart.Winter; break;
						}
						switch (FundingEndSession)
						{
							case "Spring": endTerm = (int)QuarterEnd.Spring; break;
							case "Fall": endTerm = (int)QuarterEnd.Fall; break;
							case "Winter": endTerm = (int)QuarterEnd.Winter; break;
						}
						break;
				}

				int totalYears = Convert.ToInt32(FundingEndYear) - Convert.ToInt32(EnrolledYear) + 1;
				totalTerm = (totalYears * termCount) + startTerm + endTerm;
				return totalTerm.ToString();
			}
			return String.Empty;
		}
	}
}
