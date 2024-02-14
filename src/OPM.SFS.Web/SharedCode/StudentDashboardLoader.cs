using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NUglify.Helpers;
using OPM.SFS.Core.DTO;
using OPM.SFS.Data;
using OPM.SFS.Web.Models;
using OPM.SFS.Web.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace OPM.SFS.Web.SharedCode
{
    public interface IStudentDashboardLoader
    {
        Task LoadAsync(string batch);
        Task LoadCommitmentsForDashboardByBatchesAsync(string batch, int batchsize);
		Task LoadCommitmentsForDashboardByStudentID(string studentID);
		Task LoadCommitmentsForDashboardByStudentUIDList(string uidList);


	}

    public class StudentDashboardLoader : IStudentDashboardLoader
    {
        //this class will set all the auto-calculated fields in the database to make the 
        //dashboard query simpler to load and manage
        private readonly ScholarshipForServiceContext _efDB;
        private readonly IStudentDashboardService _dashboardService;
        private readonly IReferenceDataRepository _refRepo;
		private readonly IAuditEventLogHelper _auditLogger;
        private readonly IStudentRepository _repo;

        public StudentDashboardLoader(ScholarshipForServiceContext efDB, IStudentDashboardService dashService, IReferenceDataRepository refRepo, IAuditEventLogHelper auditLogger, IStudentRepository repo)
        {
            _efDB = efDB;
            _dashboardService = dashService;
            _refRepo = refRepo;
            _auditLogger = auditLogger;
            _repo = repo;
        }       

        public async Task LoadAsync(string batch)
        {
            
            var commitmenttypelist = await _refRepo.GetCommitmentTypeAsync();            
            var PGCommitmentTypeId = commitmenttypelist.Where(m => m.Name == "Postgraduate").Select(m => m.CommitmentTypeId).FirstOrDefault();
            var INCommitmentTypeId = commitmenttypelist.Where(m => m.Name == "Internship").Select(m => m.CommitmentTypeId).FirstOrDefault();
            var studentData = await _efDB.Students
                    .Select(s => new
                    {
                        StudentUID = s.StudentUID,
                        StudentID = s.StudentId,
                        Firstname = s.FirstName,
                        LastName = s.LastName,
                        Institution = s.StudentInstitutionFundings.FirstOrDefault().Institution.Name.Trim(),
                        InstitutionID = s.StudentInstitutionFundings.FirstOrDefault().Institution.InstitutionId.ToString() != null ? s.StudentInstitutionFundings.FirstOrDefault().Institution.InstitutionId.ToString() : "",
                        SchoolType = s.StudentInstitutionFundings.FirstOrDefault().Institution.InstitutionType.Name,
                        AcademicSchedule = s.StudentInstitutionFundings.FirstOrDefault().Institution.AcademicSchedule.Name,
                        EnrolledSession = !string.IsNullOrWhiteSpace(s.StudentInstitutionFundings.FirstOrDefault().EnrolledSession) ? s.StudentInstitutionFundings.FirstOrDefault().EnrolledSession.ToString() : "",
                        EnrolledYear = s.StudentInstitutionFundings.FirstOrDefault().EnrolledYear.HasValue ? s.StudentInstitutionFundings.FirstOrDefault().EnrolledYear.ToString() : "",
                        FundingEndSession = !string.IsNullOrWhiteSpace(s.StudentInstitutionFundings.FirstOrDefault().FundingEndSession) ? s.StudentInstitutionFundings.FirstOrDefault().FundingEndSession.ToString() : "",
                        FundingEndYear = s.StudentInstitutionFundings.FirstOrDefault().FundingEndYear.HasValue ? s.StudentInstitutionFundings.FirstOrDefault().FundingEndYear.ToString() : "",
                        FundingAmount = s.StudentInstitutionFundings.FirstOrDefault().FundingAmount.HasValue ? s.StudentInstitutionFundings.FirstOrDefault().FundingAmount.ToString() : "",
                        GraduationDate = s.StudentInstitutionFundings.FirstOrDefault().ExpectedGradDate.HasValue ? s.StudentInstitutionFundings.FirstOrDefault().ExpectedGradDate.ToString() : "",
                        TotalAcademicTerms = s.StudentInstitutionFundings.FirstOrDefault().TotalAcademicTerms != null ? s.StudentInstitutionFundings.FirstOrDefault().TotalAcademicTerms : "",
                        DateLeftPGEarly = s.StudentInstitutionFundings.FirstOrDefault().DateLeftPGEarly != null ? s.StudentInstitutionFundings.FirstOrDefault().DateLeftPGEarly.ToString() : "",
                        StatusOption = s.StudentInstitutionFundings.FirstOrDefault().Status.Option != null ? s.StudentInstitutionFundings.FirstOrDefault().Status.Option.ToString() : "",
                        ProgramPhase = s.StudentInstitutionFundings.FirstOrDefault().Status.Phase != null ? s.StudentInstitutionFundings.FirstOrDefault().Status.Phase.ToString() : "",
                        PlacementCategory = s.StudentInstitutionFundings.FirstOrDefault().Status.PostGradPlacementGroup != null ? s.StudentInstitutionFundings.FirstOrDefault().Status.PostGradPlacementGroup.ToString() : "",
                        Status = s.StudentInstitutionFundings.FirstOrDefault().Status.Status != null ? s.StudentInstitutionFundings.FirstOrDefault().Status.Status.ToString() : "",
                        EmailAddress = s.Email.ToString(),
                        Degree = s.StudentInstitutionFundings.FirstOrDefault().Degree.Name,
                        Major = s.StudentInstitutionFundings.FirstOrDefault().Major.Name,
                        PGCommitment = s.StudentCommitments.Where(x => x.CommitmentTypeId == PGCommitmentTypeId)
                        .Where(x => x.CommitmentStatus.Value != "Incomplete")
                        .Where(x => x.CommitmentStatus.Value != "POReject")
                        .Where(x => x.IsDeleted.Equals(false))
                        .OrderBy(x => x.StudentCommitmentId).Select(x => new PGCommitmentDTO()
                        {
                            CommitmentId = x.StudentCommitmentId,
                            Agency = x.Agency.Name,
                            AgencyType = x.Agency.AgencyType.Name,
                            AgencyParentId = x.Agency.ParentAgencyId,
                            PGEOD = x.StartDate,
                            DateApproved = x.DateApproved,
                            StartDate = x.StartDate,
                            CommitmentType = x.CommitmentType.Name
                        }),
                        INCommitment = s.StudentCommitments
                        .Where(x => x.CommitmentTypeId == INCommitmentTypeId)
                        .Where(x => x.CommitmentStatus.Value != "Incomplete")
                        .Where(x => x.CommitmentStatus.Value != "POReject")
                        .Where(x => x.IsDeleted.Equals(false))
                        .OrderBy(x => x.StudentCommitmentId).Select(x => new INCommitmentDTO()
                        {
                            CommitmentId = x.StudentCommitmentId,
                            Agency = x.Agency.Name,
                            AgencyType = x.Agency.AgencyType.Name,
                            AgencyParentId = x.Agency.ParentAgencyId,
                            INEOD = x.StartDate,
                            DateApproved = x.DateApproved,
                            StartDate = x.StartDate,
                            CommitmentType = x.CommitmentType.Name
                        }),
                        Minor = s.StudentInstitutionFundings.FirstOrDefault().Minor != null ? s.StudentInstitutionFundings.FirstOrDefault().Minor.Name : "",
                        SecondDegreeMajor = s.StudentInstitutionFundings.FirstOrDefault().SecondDegreeMajor != null ? s.StudentInstitutionFundings.FirstOrDefault().SecondDegreeMajor.Name : "",
                        SecondDegreeMinor = s.StudentInstitutionFundings.FirstOrDefault().SecondDegreeMinor != null ? s.StudentInstitutionFundings.FirstOrDefault().SecondDegreeMinor.Name : "",
                        AltEmail = s.AlternateEmail,
                        ExtensionType = s.StudentInstitutionFundings.FirstOrDefault().ExtensionType != null ? s.StudentInstitutionFundings.FirstOrDefault().ExtensionType.Extension : "No Extension",
                        ExtensionMonths = s.StudentInstitutionFundings.FirstOrDefault().ExtensionType != null ? s.StudentInstitutionFundings.FirstOrDefault().ExtensionType.Months : 0,
                        PGEmploymentDueDate = s.StudentInstitutionFundings.FirstOrDefault().PGEmploymentDueDate != null ? s.StudentInstitutionFundings.FirstOrDefault().PGEmploymentDueDate.ToString() : null,
                        PGVerificationOneDue = s.StudentInstitutionFundings.FirstOrDefault().PGVerificationOneDueDate != null ? Convert.ToDateTime(s.StudentInstitutionFundings.FirstOrDefault().PGVerificationOneDueDate).ToShortDateString() : "",
                        PGVerificationTwoDue = s.StudentInstitutionFundings.FirstOrDefault().PGVerificationTwoDueDate != null ? Convert.ToDateTime(s.StudentInstitutionFundings.FirstOrDefault().PGVerificationTwoDueDate).ToShortDateString() : "",
                        PGVerificationOneComplete = s.StudentInstitutionFundings.FirstOrDefault().PGVerificationOneCompleteDate != null ? s.StudentInstitutionFundings.FirstOrDefault().PGVerificationOneCompleteDate.Value.ToShortDateString() : null,
                        PGVerificationTwoComplete = s.StudentInstitutionFundings.FirstOrDefault().PGVerificationTwoCompleteDate != null ? s.StudentInstitutionFundings.FirstOrDefault().PGVerificationTwoCompleteDate.Value.ToShortDateString() : null,
                        CommitmentPhaseComplete = s.StudentInstitutionFundings.FirstOrDefault().CommitmentPhaseComplete != null ? s.StudentInstitutionFundings.FirstOrDefault().CommitmentPhaseComplete.Value.ToShortDateString() : null,
                        Citizenship = s.Citizenship.Value,
                        LastUpdated = s.LastUpdated,
                        Contract = s.StudentInstitutionFundings.FirstOrDefault().Contract != null ? s.StudentInstitutionFundings.FirstOrDefault().Contract.Name : "",
                        Notes = s.StudentInstitutionFundings.FirstOrDefault().Notes != null ? s.StudentInstitutionFundings.FirstOrDefault().Notes : ""
                    }).AsSplitQuery().OrderBy(m => m.StudentUID).ToListAsync();


            if (!string.IsNullOrWhiteSpace(batch))
            {
				int skip = (Convert.ToInt32(batch) - 1) * 500;
				var studentDataPage = studentData.Skip(skip).Take(500).ToList();
				foreach (var student in studentDataPage)
				{
					try
					{
						string serviceOwed = "N/A";
						string TotalAcademicTerms = null;
						if (student.EnrolledYear != "N/A" || student.EnrolledSession != "N/A" || student.FundingEndYear != "N/A" || student.FundingEndSession != "N/A")
						{
							if (student.TotalAcademicTerms == String.Empty)
							{

								TotalAcademicTerms = _dashboardService.CalculateTotalTerms(student.EnrolledYear, student.EnrolledSession, student.FundingEndYear, student.FundingEndSession, student.AcademicSchedule);
							}
							else
							{
								TotalAcademicTerms = student.TotalAcademicTerms;
							}
						}
						serviceOwed = _dashboardService.GetServiceOwed(student.AcademicSchedule, student.TotalAcademicTerms, student.EnrolledYear, student.EnrolledSession, student.FundingEndYear, student.FundingEndSession);
						int counter = 0;
						List<string> PGAgencyInfo = new List<string>();
						List<string> INAgencyInfo = new List<string>();
						int totalCommitments = student.PGCommitment.Count() + student.INCommitment.Count();
						totalCommitments = totalCommitments > 0 ? totalCommitments : 0;

						StringBuilder additionalNotes = new StringBuilder();
						additionalNotes.Append(student.Notes != "N/A" ? student.Notes : null);
						additionalNotes.Append(await _dashboardService.ConsolidateCommitmentNotes(student.INCommitment.ToList(), student.PGCommitment.ToList()));

						int noteCommitmentCounter = 1;
						string parentAgencyName = "";
						string newNotes;
						foreach (var commitment in student.INCommitment.OrderBy(m => m.CommitmentId))
						{
							if (commitment != null)
							{
								INAgencyInfo = await _dashboardService.SetAgencyInfoAsync(commitment.Agency, commitment.AgencyType, commitment.AgencyParentId, commitment.INEOD, INAgencyInfo, counter);
								counter++;
							}

						}
						counter = 0;
						foreach (var commitment in student.PGCommitment.OrderBy(m => m.CommitmentId))
						{
							if (commitment != null)
							{
								PGAgencyInfo = await _dashboardService.SetAgencyInfoAsync(commitment.Agency, commitment.AgencyType, commitment.AgencyParentId, commitment.PGEOD, PGAgencyInfo, counter);
								counter++;
							}
						}

						string PGVerification1Due = "N/A", PGVerification2Due = "N/A";
						string commitmentphaseComplete = "N/A";

						if (PGAgencyInfo.Count > 2 && PGAgencyInfo[3] != "N/A")
						{
							DateTime? pgCommitmentEOD = Convert.ToDateTime(PGAgencyInfo[3]);
							double doubleresult;

							PGVerification1Due = _dashboardService.CalculateCommitmentVerificationOne(pgCommitmentEOD, student.PGVerificationOneDue, serviceOwed);
							PGVerification2Due = _dashboardService.CalculateCommitmentVerificationTwo(pgCommitmentEOD, student.PGVerificationTwoDue, serviceOwed);

							if (student.CommitmentPhaseComplete != null && student.CommitmentPhaseComplete != "N/A")
							{
								commitmentphaseComplete = student.CommitmentPhaseComplete;
							}
							else if (!string.IsNullOrWhiteSpace(serviceOwed) && serviceOwed != "N/A" && double.TryParse(serviceOwed, out doubleresult))
							{
								commitmentphaseComplete = _dashboardService.CalculateCommitmentPhaseComplete(pgCommitmentEOD, Convert.ToDecimal(serviceOwed));
							}
						}

						var studentToUpdate = await _efDB.StudentInstitutionFundings.Where(m => m.StudentId == student.StudentID).FirstOrDefaultAsync();
						if (!string.IsNullOrWhiteSpace(serviceOwed) && serviceOwed != "N/A" && double.TryParse(serviceOwed, out double dResult))
						{
							studentToUpdate.ServiceOwed = Convert.ToDouble(serviceOwed);
						}

						if (!string.IsNullOrWhiteSpace(commitmentphaseComplete) && commitmentphaseComplete != "N/A")
						{
							studentToUpdate.CommitmentPhaseComplete = Convert.ToDateTime(commitmentphaseComplete);
						}

						var pgEODDue = !string.IsNullOrWhiteSpace(student.PGEmploymentDueDate) ? _dashboardService.CalculatePGEmploymentDate(DateTime.Parse(student.PGEmploymentDueDate), 0, 0) : _dashboardService.CalculatePGEmploymentDate(DateTime.Parse(student.GraduationDate), 18, student.ExtensionMonths);
						studentToUpdate.TotalAcademicTerms = !string.IsNullOrWhiteSpace(TotalAcademicTerms) && TotalAcademicTerms != "N/A" ? TotalAcademicTerms : null;
						if (!string.IsNullOrWhiteSpace(pgEODDue) && pgEODDue != "N/A")
						{
							studentToUpdate.PGEmploymentDueDate = Convert.ToDateTime(pgEODDue);
						}

						studentToUpdate.Notes = additionalNotes.ToString().IfNullOrWhiteSpace(null);
						studentToUpdate.PGVerificationOneDueDate = PGVerification1Due != "N/A" ? Convert.ToDateTime(PGVerification1Due) : null;
						studentToUpdate.PGVerificationTwoDueDate = PGVerification2Due != "N/A" ? Convert.ToDateTime(PGVerification2Due) : null;
						studentToUpdate.InternshipReported = INAgencyInfo.Count > 0 ? "Yes" : "No";
						studentToUpdate.PostGradReported = PGAgencyInfo.Count > 0 ? "Yes" : "No";
						studentToUpdate.InternshipAgencyType = INAgencyInfo.Count > 0 ? "Yes" : "No";
						studentToUpdate.InternshipAgencyName = INAgencyInfo.Count > 0 ? INAgencyInfo[1] : null;
						studentToUpdate.InternshipSubAgencyName = INAgencyInfo.Count > 0 ? INAgencyInfo[2] : null;
						string InEOD = INAgencyInfo.Count > 0 ? INAgencyInfo[3] : "N/A";
						string PgEOD = PGAgencyInfo.Count > 0 ? PGAgencyInfo[3] : "N/A";
						studentToUpdate.InternshipEOD = InEOD != "N/A" ? Convert.ToDateTime(InEOD) : null;
						studentToUpdate.AdditionalInternshipAgencyType = INAgencyInfo.Count > 4 ? INAgencyInfo[4] : null;
						studentToUpdate.AdditionalInternshipAgencyName = INAgencyInfo.Count > 4 ? INAgencyInfo[5] : null;
						studentToUpdate.AdditionalInternshipSubAgencyName = INAgencyInfo.Count > 4 ? INAgencyInfo[6] : null;
						studentToUpdate.AdditionalInternshipReportedWebsite = INAgencyInfo.Count > 4 ? "Yes" : "No";
						studentToUpdate.PostGradAgencyType = PGAgencyInfo.Count > 0 ? PGAgencyInfo[0] : null;
						studentToUpdate.PostGradAgencyName = PGAgencyInfo.Count > 0 ? PGAgencyInfo[1] : null;
						studentToUpdate.PostGradSubAgencyName = PGAgencyInfo.Count > 0 ? PGAgencyInfo[2] : null;
						studentToUpdate.AdditionalPostGradAgencyType = PGAgencyInfo.Count > 4 ? PGAgencyInfo[4] : null;
						studentToUpdate.AdditionalPostGradAgencyName = PGAgencyInfo.Count > 4 ? PGAgencyInfo[5] : null;
						studentToUpdate.AdditionalInternshipSubAgencyName = PGAgencyInfo.Count > 4 ? PGAgencyInfo[6] : null;
						studentToUpdate.AdditionalPostGradReportedWebsite = PGAgencyInfo.Count > 4 ? PGAgencyInfo[6] : null;
						studentToUpdate.PostGradEOD = PgEOD != "N/A" ? Convert.ToDateTime(PgEOD) : null;
						await _efDB.SaveChangesAsync();
						
					}
					catch (Exception ex)
					{
						await _auditLogger.LogAuditEvent($"Student Dashboard loader - batch {batch} error {ex.Message} for ID {student.StudentID}");
					}

				}
				await _auditLogger.LogAuditEvent($"Student Dashboard loader - batch {batch} complete. {studentDataPage.Count} rows updated.");
			}
			else
			{
				await _auditLogger.LogAuditEvent($"Student Dashboard loader - batch is empty.");
			}
           
        }

        public async Task LoadCommitmentsForDashboardByBatchesAsync(string batch, int batchsize)
        {

			var students = _efDB.Students.Select(m => m.StudentId).ToList();           
			if (!string.IsNullOrWhiteSpace(batch))
            {
				int skip = (Convert.ToInt32(batch) - 1) * batchsize;
				var dataBatch = students.Skip(skip).Take(batchsize).ToList();
				foreach (var studentID in dataBatch)
				{
					await _dashboardService.UpdateCommitmentReportedForDashboard(studentID);
				}
			}
		   
        }

		public async Task LoadCommitmentsForDashboardByStudentUIDList(string uidList)
		{
			List<string> theListToProcess = uidList.Split(",").ToList();
			foreach (string uid in theListToProcess)
			{
				var studentID = await _efDB.Students.Where(m => m.StudentUID == Convert.ToInt32(uid)).Select(m => m.StudentId).FirstOrDefaultAsync();
				await LoadCommitmentsForDashboardByStudentID(studentID.ToString());
			}
		}

		public async Task LoadCommitmentsForDashboardByStudentID(string studentID)
		{
			if(!string.IsNullOrWhiteSpace(studentID))
				await _dashboardService.UpdateCommitmentReportedForDashboard(Convert.ToInt32(studentID));
		}

		private AgencyNameFormat GetAgencyParentAndSubAgencyNames(string parent, string child)
		{
			AgencyNameFormat result = new();
			if(string.IsNullOrWhiteSpace(parent) || parent == "N/A")
			{
				result.Agency = child;
				result.SubAgency = null;
			}
			else
			{
				result.Agency = parent;
				result.SubAgency = child;
			}
			return result;
		}

		private DateTime? FormatDate(string input)
		{
			if(input == null) return null;
			if (input == "N/A") return null;
			return Convert.ToDateTime(input);
		}
    }

	public class AgencyNameFormat
	{
		public string Agency { get; set; }
		public string SubAgency { get; set; }
	}
}
