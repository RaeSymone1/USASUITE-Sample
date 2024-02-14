using System;
using OPM.SFS.Core.Shared;
using System.Globalization;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using OPM.SFS.Data;
using static OPM.SFS.Web.SharedCode.CommitmentProcessService;
using OPM.SFS.Web.SharedCode;
using System.Text;
using Humanizer;
using OPM.SFS.Core.DTO;
using OPM.SFS.Web.Pages.Student;
using static OPM.SFS.Web.Models.CommitmentModelViewModel;
using Microsoft.CodeAnalysis.CSharp;


namespace OPM.SFS.Web.Shared
{
    public interface IStudentDashboardService
    {
        string CalculateCommitmentVerificationOne(DateTime? commitmentStartDate, string PGVerificationOneDue, string ServiceOwed);
        string CalculateCommitmentVerificationTwo(DateTime? commitmentStartDate, string PGVerificationTwoDue, string ServiceOwed);
        string CalculateCommitmentPhaseComplete(DateTime? commitmentStartDate, decimal serviceOwed);
        string GetServiceOwed(string AcademicSchedule, string TotalAcademicTerms, string EnrolledYear, string EnrolledSession, string FundingEndYear, string FundingEndSession);
        string CalculateTotalTerms(string EnrolledYear, string EnrolledSession, string FundingEndYear, string FundingEndSession, string AcademicSchedule);
        string CalculatePGEmploymentDate(DateTime? expectedGradDate, int GracePeriod, int ExtensionMonths);
        Task<List<string>> SetAgencyInfoAsync(string AgencyName, string AgencyType, int? AgencyParentId, DateTime? INEOD, List<string> CommitmentAgencyInfo, int counter);
        public bool IsValidDate(string date);
        public Task<string> ConsolidateCommitmentNotes(List<INCommitmentDTO> INCommitments, List<PGCommitmentDTO> PGCommitments);
        public Task<string> GenerateSingleCommitmentNote(int commitmentID, int studentID, string commitmentType);
        public Task<bool> UpdateCommitmentReportedForDashboard(int studentID);
        public Task<string> GenerateRegistrationCode(string uuID);
        public Task<string> GetCommimentType(int typeID);
    }

    public class StudentDashboardService : IStudentDashboardService
    {
        private readonly IServiceOwedService _serviceowed;
        private readonly IReferenceDataRepository _refRepo;
        private readonly IStudentRepository _repo;
        private readonly ICryptoHelper _crypto;
        private readonly IUtilitiesService _utilities;

        enum InstitutionTerm { Trimester, Semester, Quarter };
        enum InstitutionTermCount { Trimester = 3, Semester = 2, Quarter = 3 };
        enum QuarterStart { Winter = 0, Spring = -1, Fall = -2 }
        enum QuarterEnd { Winter = -2, Spring = -1, Fall = 0 }
        enum TrimesterStart { Winter = 0, Spring = -1, Fall = -2 }
        enum TrimesterEnd { Winter = -2, Spring = -1, Fall = 0 }
        enum SemesterStart { Spring = 0, Fall = -1 }
        enum SemesterEnd { Spring = -1, Fall = 0 }

        public StudentDashboardService(IServiceOwedService serviceOwed, IReferenceDataRepository refRepo, IStudentRepository repo, ICryptoHelper refcryp, IUtilitiesService utilities)
        {
            _serviceowed = serviceOwed;
            _refRepo = refRepo;
            _repo = repo;
            _crypto = refcryp;
            _utilities = utilities;
        }

        public string CalculateTotalTerms(string EnrolledYear, string EnrolledSession, string FundingEndYear, string FundingEndSession, string AcademicSchedule)
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
                    case "Trimester":
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
        public string GetServiceOwed(string AcademicSchedule, string TotalAcademicTerms, string EnrolledYear, string EnrolledSession, string FundingEndYear, string FundingEndSession)
        {
            if (TotalAcademicTerms != String.Empty && int.TryParse(TotalAcademicTerms, out int result))
            {
                
                ServiceOwed serviceowed = _serviceowed.CalculateServiceOwedbyTerms(AcademicSchedule, int.Parse(TotalAcademicTerms));
                if (serviceowed.ex.Length > 0)
                {
                    return serviceowed.ex;
                }
                else
                {
                    return serviceowed.ServiceTime.ToString();
                }
            }
            else if (EnrolledYear != "N/A" && EnrolledSession != "N/A" && FundingEndYear != "N/A" && FundingEndSession != "N/A" && !string.IsNullOrWhiteSpace(EnrolledYear) && !string.IsNullOrWhiteSpace(EnrolledSession) && !string.IsNullOrWhiteSpace(FundingEndYear) && !string.IsNullOrWhiteSpace(FundingEndSession))
            {
                ServiceOwed serviceowed = _serviceowed.CalculateServiceOwedbySeason(AcademicSchedule, EnrolledSession, Convert.ToInt32(EnrolledYear), FundingEndSession, Convert.ToInt32(FundingEndYear));
                if (serviceowed.ex.Length > 0)
                {
                    return serviceowed.ex;
                }
                else
                {
                    return serviceowed.ServiceTime.ToString();
                }
            }
            else
            {
                return "N/A";
            }
        }

        public string CalculateCommitmentVerificationOne(DateTime? commitmentStartDate, string PGVerificationOneDue, string ServiceOwed)
        {
            if (IsValidDate(PGVerificationOneDue))
               return PGVerificationOneDue;

            if(ServiceOwed.Equals("1"))
                return "N/A";
            if (commitmentStartDate.HasValue)
                return commitmentStartDate.Value.AddYears(1).ToShortDateString();
            return "N/A";
        }

        public string CalculateCommitmentVerificationTwo(DateTime? commitmentStartDate, string PGVerificationTwoDue, string ServiceOwed)
        {
            if (IsValidDate(PGVerificationTwoDue))
                return PGVerificationTwoDue;

            if (ServiceOwed.Equals("1")|| ServiceOwed.Equals("1.5")|| ServiceOwed.Equals("2"))
                return "N/A";

            if (commitmentStartDate.HasValue)
                return commitmentStartDate.Value.AddYears(2).ToShortDateString();
            return "N/A";
        }

        public string CalculateCommitmentPhaseComplete(DateTime? commitmentStartDate, decimal serviceOwed)
        {
            if (commitmentStartDate.HasValue)
            {
                int months = (int)(serviceOwed * 12);
                return commitmentStartDate.Value.AddMonths(months).ToShortDateString();
            }
                
            return "N/A";
        }

        public string CalculatePGEmploymentDate(DateTime? expectedGradDate, int GracePeriod, int ExtensionMonths)
        {
            if (expectedGradDate.HasValue)
            {
                DateTime CalculatedDueDate = expectedGradDate ?? _utilities.ConvertUtcToEastern(DateTime.Now);
                CalculatedDueDate = CalculatedDueDate.AddMonths((GracePeriod + ExtensionMonths));
                return CalculatedDueDate.ToString("MMM yyyy", CultureInfo.GetCultureInfo("en-US"));
            }
            else
                return "N/A";
        }

        public async Task<List<string>> SetAgencyInfoAsync(string AgencyName, string AgencyType, int? AgencyParentId, DateTime? INEOD, List<string> CommitmentAgencyInfo, int counter)
        {
            var Agencies = await _refRepo.GetAgenciesAsync();
            if (counter <= 1)
            {
                if (AgencyParentId != null)
                {
                    CommitmentAgencyInfo.Add(AgencyType);
                    CommitmentAgencyInfo.Add(Agencies.Where(m => m.AgencyId == AgencyParentId).Select(m => m.Name).FirstOrDefault());
                    CommitmentAgencyInfo.Add(AgencyName);
                }
                else
                {
                    CommitmentAgencyInfo.Add(AgencyType);
                    CommitmentAgencyInfo.Add(AgencyName);
                    CommitmentAgencyInfo.Add("N/A");
                }
                if (INEOD.HasValue)
                    CommitmentAgencyInfo.Add(INEOD.Value.ToShortDateString());
                else
                    CommitmentAgencyInfo.Add("N/A");
            }
            return CommitmentAgencyInfo;
            //Add Condition to add any further commitments to notes
        }

        public bool IsValidDate(string date)
        {
            if (string.IsNullOrWhiteSpace(date)) return false;
            if (DateTime.TryParse(date, out _)) return true;
            return false;
        }

        public async Task<string> ConsolidateCommitmentNotes(List<INCommitmentDTO> INCommitments, List<PGCommitmentDTO> PGCommitments)
        {

            StringBuilder allNotes = new StringBuilder();
            var agency = "";
            var subAgency = "";
            var ParentAgency = "";
            var allAgencies = await _refRepo.GetAgenciesWithDisabledAsync();

            if (INCommitments != null && INCommitments.Count > 2)
            {

                //foreach(var internship in INCommitments.Skip(2))
                INCommitments = INCommitments.OrderBy(m => m.CommitmentId).ToList();
                for (int i = 2; i < INCommitments.Count; i++)
                {
                    int ordinal = i + 1;
                    if (INCommitments[i].AgencyParentId != null)
                    {
                        ParentAgency = allAgencies.Where(m => m.AgencyId == INCommitments[i].AgencyParentId).FirstOrDefault() != null
                            ? allAgencies.Where(m => m.AgencyId == INCommitments[i].AgencyParentId).FirstOrDefault().Name : "";

                        if (!string.IsNullOrWhiteSpace(ParentAgency))
                        {                           
                            agency = ParentAgency;
                            subAgency = INCommitments[i].Agency.ToString();
                            allNotes.Append($"{_utilities.ConvertUtcToEastern(DateTime.UtcNow).ToShortDateString()} {ordinal.ToOrdinalWords().Humanize(LetterCasing.Title)} IN with {agency} / {subAgency} with start date of {INCommitments[i].StartDate.Value.ToShortDateString()}; ");
                        }
                        else                        
                        {                            
                            allNotes.Append($"{_utilities.ConvertUtcToEastern(DateTime.UtcNow).ToShortDateString()} {ordinal.ToOrdinalWords().Humanize(LetterCasing.Title)} IN with {INCommitments[i].Agency} with start date of {INCommitments[i].StartDate.Value.ToShortDateString()}; ");
                        }
                    }
                    else
                    {
                        allNotes.Append($"{_utilities.ConvertUtcToEastern(DateTime.UtcNow).ToShortDateString()} {ordinal.ToOrdinalWords().Humanize(LetterCasing.Title)} IN with {INCommitments[i].Agency} with start date of {INCommitments[i].StartDate.Value.ToShortDateString()}; ");

                    }
                }
            }
            if (PGCommitments != null && PGCommitments.Count > 2)
            {
                //foreach(var internship in INCommitments.Skip(2))
                PGCommitments = PGCommitments.OrderBy(m => m.CommitmentId).ToList();
                for (int i = 2; i < PGCommitments.Count; i++)
                {
                    int ordinal = i + 1;
                    if (PGCommitments[i].AgencyParentId != null)
                    {
                        ParentAgency = allAgencies.Where(m => m.AgencyId == PGCommitments[i].AgencyParentId).FirstOrDefault() != null
                            ? allAgencies.Where(m => m.AgencyId == PGCommitments[i].AgencyParentId).FirstOrDefault().Name : "";

                        if (!string.IsNullOrWhiteSpace(ParentAgency))
                        {
                            agency = ParentAgency;
                            subAgency = PGCommitments[i].Agency.ToString();
                            allNotes.Append($"{_utilities.ConvertUtcToEastern(DateTime.UtcNow).ToShortDateString()} {ordinal.ToOrdinalWords().Humanize(LetterCasing.Title)} PG with {agency} / {subAgency} with start date of {PGCommitments[i].StartDate.Value.ToShortDateString()}; ");
                        }
                        else
                        {
                            allNotes.Append($"{_utilities.ConvertUtcToEastern(DateTime.UtcNow).ToShortDateString()} {ordinal.ToOrdinalWords().Humanize(LetterCasing.Title)} PG with {PGCommitments[i].Agency} with start date of {PGCommitments[i].StartDate.Value.ToShortDateString()}; ");
                        }
                    }
                    else
                    {
                        allNotes.Append($"{_utilities.ConvertUtcToEastern(DateTime.UtcNow).ToShortDateString()} {ordinal.ToOrdinalWords().Humanize(LetterCasing.Title)} PG with {PGCommitments[i].Agency} with start date of {PGCommitments[i].StartDate.Value.ToShortDateString()}; ");

                    }
                }
            }
            return allNotes.ToString();
            
        }

       
		public async Task<bool> UpdateCommitmentReportedForDashboard(int studentID)
		{			
            var allCommitments = await _repo.GetAllSubmittedCommitmentsByStudentID(studentID);
			await _repo.DeleteOldCommitmentData(studentID);
            await HandleInternshipCommitmentReportedAsync(allCommitments, studentID);
            await HandlePostGradCommitmentReportedAsync(allCommitments, studentID);
			return true;
		}
		public async Task<string> GenerateRegistrationCode(string uuID)
        {
            string result = null;
            if (uuID != null)
            {
                var GlobalConfigSettings = await _refRepo.GetGlobalConfigurationsAsync();
                result = _crypto.Encrypt(uuID, GlobalConfigSettings);
            }
            return result;
        }

        public async Task<string> GenerateSingleCommitmentNote(int commitmentID, int studentID, string commitmentType)
        {
            string note = "";
            if (commitmentType == "Postgraduate")
            {
                var currentPostGrads = await _repo.GetPostGradCommitmentsByStudentID(studentID);
                if(currentPostGrads.Count > 2)
                {                   
                    int index = currentPostGrads.FindIndex(a => a.CommitmentID == commitmentID);
                    var commitment = currentPostGrads.Where(m => m.CommitmentID == commitmentID).FirstOrDefault();
					string startDate = commitment.StartDate.HasValue ? commitment.StartDate.Value.ToShortDateString() : "N/A";
					if (commitment.SubAgencyName != "N/A")
                    {
                        note = $"{_utilities.ConvertUtcToEastern(DateTime.UtcNow).ToShortDateString()} {(index + 1).ToOrdinalWords().Humanize(LetterCasing.Title)} PG with {commitment.AgencyName} / {commitment.SubAgencyName} with start date of {startDate}; ";
                    }
                    else
                    {
                        note = $"{_utilities.ConvertUtcToEastern(DateTime.UtcNow).ToShortDateString()} {(index + 1).ToOrdinalWords().Humanize(LetterCasing.Title)} PG with {commitment.AgencyName} with start date of {startDate}; ";
                    }
                   
                }               
            }
            if(commitmentType == "Internship")
            {
                var currentInterships = await _repo.GetInternshipsByStudentID(studentID);
                if (currentInterships.Count > 2)
                {                    
                    int index = currentInterships.FindIndex(a => a.CommitmentID == commitmentID);
                    var commitment = currentInterships.Where(m => m.CommitmentID == commitmentID).FirstOrDefault();
                    string startDate = commitment.StartDate.HasValue ? commitment.StartDate.Value.ToShortDateString() : "N/A";
                    if (commitment.SubAgencyName != "N/A")
                    {
                        note = $"{_utilities.ConvertUtcToEastern(DateTime.UtcNow).ToShortDateString()} {(index + 1).ToOrdinalWords().Humanize(LetterCasing.Title)} IN with {commitment.AgencyName} / {commitment.SubAgencyName} with start date of {startDate}; ";
                    }
                    else
                    {
                        note = $"{_utilities.ConvertUtcToEastern(DateTime.UtcNow).ToShortDateString()} {(index + 1).ToOrdinalWords().Humanize(LetterCasing.Title)} IN with {commitment.AgencyName} with start date of {startDate}; ";
                    }
                }
            }
            return note;
        }

        public async Task<string> GetCommimentType(int typeID)
        {
            var typeData = await _refRepo.GetCommitmentTypeAsync();
            return typeData.Where(m => m.CommitmentTypeId== typeID).Select(m => m.Name).FirstOrDefault();
        }

        private async Task HandlePostGradCommitmentReportedAsync(List<CommitmentReportedDTO> allCommitments, int studentID)
        {
			int postgrad = 0;
			var approvedPG = allCommitments.Where(m => m.CommitmentType == "Postgraduate").Where(m => m.StartDate.HasValue).Where(m => m.Status == "POApproved").ToList();
			foreach (var c in approvedPG)
			{
				postgrad++;
				if (postgrad == 1)
				{
					if (c.ServiceOwed != null)
					{
						var PGVerification1Due = CalculateCommitmentVerificationOne(c.StartDate, "", c.ServiceOwed);
						var PGVerification2Due = CalculateCommitmentVerificationTwo(c.StartDate, "", c.ServiceOwed);
						DateTime? verify1Date = PGVerification1Due != "N/A" ? Convert.ToDateTime(PGVerification1Due) : null;
						DateTime? verify2Date = PGVerification2Due != "N/A" ? Convert.ToDateTime(PGVerification2Due) : null;
						await _repo.UpdatePostGradReported(c, verify1Date, verify2Date);
					}
					else
						await _repo.UpdatePostGradReported(c, null, null);

				}
				if (postgrad == 2)
				{
					await _repo.UpdateAdditionalPostGradReported(c);
				}
				else
				{
					string newNotes = await GenerateSingleCommitmentNote(c.CommitmentID, studentID, "Postgraduate");
					if (!string.IsNullOrEmpty(newNotes)) await _repo.AppendNotesForCommitmentsReported(studentID, newNotes);
				}
			}

			if (approvedPG.Count == 0)
			{
				//check for commitments pending approval
				var pendingPg = allCommitments.Where(m => m.Status != "POApproved").Where(m => m.Status != "Incomplete").Where(m => m.CommitmentType == "Postgraduate").FirstOrDefault();
				if (pendingPg != null)
				{
					string pgReported = $"Y - {pendingPg.StatusDisplay}";
					await _repo.UpdateStudentDashboardPGReportedField(studentID, pgReported);
				}
			}
		}

		private async Task HandleInternshipCommitmentReportedAsync(List<CommitmentReportedDTO> allCommitments, int studentID)
        {
			int internships = 0;
			var approvedIN = allCommitments.Where(m => m.CommitmentType == "Internship").Where(m => m.StartDate.HasValue).Where(m => m.Status == "POApproved").ToList();

			foreach (var c in approvedIN)
			{
				internships++;
				if (internships == 1)
				{
					await _repo.UpdateInternshipReported(c);
				}

				if (internships == 2)
				{
					await _repo.UpdateAdditionalInternshipReported(c);
				}
				else
				{
					string newNotes = await GenerateSingleCommitmentNote(c.CommitmentID, studentID, "Internship");
					if (!string.IsNullOrEmpty(newNotes)) await _repo.AppendNotesForCommitmentsReported(studentID, newNotes);
				}
			}

			if (approvedIN.Count == 0)
			{
				//check for commitments pending approval
				var pendingIN = allCommitments.Where(m => m.Status != "POApproved").Where(m => m.Status != "Incomplete").Where(m => m.CommitmentType == "Internship").FirstOrDefault();
				if (pendingIN != null)
				{
					string inReported = $"Y - {pendingIN.StatusDisplay}";
					await _repo.UpdateStudentDashboardINReportedField(studentID, inReported);
				}
			}


		}

	}

    public class DashboardUpdateResults
    {
        public string NewPgPlacement { get; set; }
        public string NewPhase { get; set; }
    }
}
