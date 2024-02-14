using Azure.Core;
using Azure.Identity;
using Dapper;
using FluentValidation.Results;
using MediatR;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using OPM.SFS.Core.DTO;
using OPM.SFS.Data;
using OPM.SFS.Web.Models.Student;
using OPM.SFS.Web.Pages.Student;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using static OPM.SFS.Web.Models.AdminStudentSearchViewModel;
using static OPM.SFS.Web.Models.CommitmentModelViewModel;

namespace OPM.SFS.Web.SharedCode
{
    public interface IStudentRepository
    {
        Task<List<StudentDashboardDTO>> GetAllStudentsForDashboardAsync();
        Task<List<CommitmentReportedDTO>> GetInternshipsByStudentID(int studentID);
        Task UpdateInternshipReported(CommitmentReportedDTO data);
        Task UpdateAdditionalInternshipReported(CommitmentReportedDTO data);
        Task<List<CommitmentReportedDTO>> GetPostGradCommitmentsByStudentID(int studentID);
        Task UpdateAdditionalPostGradReported(CommitmentReportedDTO data);
        Task UpdatePostGradReported(CommitmentReportedDTO data, DateTime? verification1, DateTime? verification2);
        Task AppendNotesForCommitmentsReported(int studentID, string noteToAdd);
        Task<string> GetNotesByStudentID(int studentID);
        Task<Student> GetStudentByStudentUID(int studentUID);
        Task<StudentRegistrationDTO> GetStudentByEmail(string email);

		Task<StudentRegistrationDTO> GetStudentBySSN(string ssn);

		Task<int> AddOrUpdateStudent(StudentRegistrationViewModel student);
        Task<int> GetMaxUID();
        Task<RegistrationCode> GetStudentRegistrationCode(string code);
        Task<List<StudentDashboardDTO>> GetAllStudentsForPIDashboardAsync(int institutionID);
        Task<List<StudentDashboardDTO>> GetAllStudentForDashboardWithSP();
        Task<List<StudentDashboardCommitmentsDTO>> GetAllStudentCommitmentsForDashboardWithSP();
		Task<List<StudentDashboardCommitmentsDTO>> GetAllStudentCommitmentsForDashboardByStudentIDWithSP(string studentID);
		Task<List<CommitmentReportedDTO>> GetApprovedCommitmentsByStudentID(int studentID);
		Task<List<CommitmentReportedDTO>> GetAllSubmittedCommitmentsByStudentID(int studentID);
		Task DeleteOldCommitmentData(int studentID);
        Task UpdateStudentDashboardPGReportedField(int studentID, string pgReported);
		Task UpdateStudentDashboardINReportedField(int studentID, string inReported);

        Task<DocumentScanIno> HandleMalwareScanResults(int studentID);
        Task<CommitmentVerificationDTO> GetCommitmentVerificationData(int studentID);
        Task<List<CommitmentVerificationDetailsDTO>> GetCommitmentVerificationDetailsData(int studentID);
    }

	public class StudentRepository : IStudentRepository
    {
        private readonly ScholarshipForServiceContext _efDB;

        public StudentRepository(ScholarshipForServiceContext efDB)
        {
            _efDB = efDB;
        }

        public async Task<List<StudentDashboardDTO>> GetAllStudentsForDashboardAsync()
        {

            var studentData = await _efDB.Students
                   .Select(s => new StudentDashboardDTO()
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
                       ProfileStatus = s.ProfileStatus.Name != null ? s.ProfileStatus.Name.ToString() : "",
                       EmailAddress = s.Email.ToString(),
                       Degree = s.StudentInstitutionFundings.FirstOrDefault().Degree.Name,
                       Major = s.StudentInstitutionFundings.FirstOrDefault().Major.Name,
                       Minor = s.StudentInstitutionFundings.FirstOrDefault().Minor != null ? s.StudentInstitutionFundings.FirstOrDefault().Minor.Name : "",
                       SecondDegreeMajor = s.StudentInstitutionFundings.FirstOrDefault().SecondDegreeMajor != null ? s.StudentInstitutionFundings.FirstOrDefault().SecondDegreeMajor.Name : "",
                       SecondDegreeMinor = s.StudentInstitutionFundings.FirstOrDefault().SecondDegreeMinor != null ? s.StudentInstitutionFundings.FirstOrDefault().SecondDegreeMinor.Name : "",
                       AltEmail = s.AlternateEmail,
                       ExtensionType = s.StudentInstitutionFundings.FirstOrDefault().ExtensionType != null ? s.StudentInstitutionFundings.FirstOrDefault().ExtensionType.Extension : "No Extension",
                       ExtensionMonths = s.StudentInstitutionFundings.FirstOrDefault().ExtensionType != null ? s.StudentInstitutionFundings.FirstOrDefault().ExtensionType.Months : 0,
                       PGEmploymentDueDate = s.StudentInstitutionFundings.FirstOrDefault().PGEmploymentDueDate,
                       PGVerificationOneDueDate = s.StudentInstitutionFundings.FirstOrDefault().PGVerificationOneDueDate != null ? Convert.ToDateTime(s.StudentInstitutionFundings.FirstOrDefault().PGVerificationOneDueDate).ToShortDateString() : "",
                       PGVerificationTwoDueDate = s.StudentInstitutionFundings.FirstOrDefault().PGVerificationTwoDueDate != null ? Convert.ToDateTime(s.StudentInstitutionFundings.FirstOrDefault().PGVerificationTwoDueDate).ToShortDateString() : "",
                       PGVerificationOneCompleteDate = s.StudentInstitutionFundings.FirstOrDefault().PGVerificationOneCompleteDate != null ? s.StudentInstitutionFundings.FirstOrDefault().PGVerificationOneCompleteDate.Value.ToShortDateString() : null,
                       PGVerificationTwoCompleteDate = s.StudentInstitutionFundings.FirstOrDefault().PGVerificationTwoCompleteDate != null ? s.StudentInstitutionFundings.FirstOrDefault().PGVerificationTwoCompleteDate.Value.ToShortDateString() : null,
                       CommitmentPhaseComplete = s.StudentInstitutionFundings.FirstOrDefault().CommitmentPhaseComplete != null ? s.StudentInstitutionFundings.FirstOrDefault().CommitmentPhaseComplete.Value.ToShortDateString() : null,
					   SOCVerificationComplete = s.StudentInstitutionFundings.FirstOrDefault().SOCVerificationComplete != null ? s.StudentInstitutionFundings.FirstOrDefault().SOCVerificationComplete.Value.ToShortDateString() : null,
					   Citizenship = s.Citizenship.Value,
                       LastUpdated = s.LastUpdated,
                       Contract = s.StudentInstitutionFundings.FirstOrDefault().Contract != null ? s.StudentInstitutionFundings.FirstOrDefault().Contract.Name : "",
                       ServiceOwed = s.StudentInstitutionFundings.FirstOrDefault().ServiceOwed,
                       InternshipReported = s.StudentInstitutionFundings.FirstOrDefault().InternshipReported,
                       PostGradReported = s.StudentInstitutionFundings.FirstOrDefault().PostGradReported,
                       InternshipAgencyType = s.StudentInstitutionFundings.FirstOrDefault().InternshipAgencyType,
                       InternshipAgencyName = s.StudentInstitutionFundings.FirstOrDefault().InternshipAgencyName,
                       InternshipSubAgencyName = s.StudentInstitutionFundings.FirstOrDefault().InternshipSubAgencyName,
                       InternshipEOD = s.StudentInstitutionFundings.FirstOrDefault().InternshipEOD,
                       AdditionalInternshipAgencyType = s.StudentInstitutionFundings.FirstOrDefault().AdditionalInternshipAgencyType,
                       AdditionalInternshipAgencyName = s.StudentInstitutionFundings.FirstOrDefault().AdditionalInternshipAgencyName,
                       AdditionalInternshipSubAgencyName = s.StudentInstitutionFundings.FirstOrDefault().AdditionalInternshipSubAgencyName,
                       AdditionalInternshipReportedWebsite = s.StudentInstitutionFundings.FirstOrDefault().AdditionalInternshipReportedWebsite,
                       PostGradAgencyType = s.StudentInstitutionFundings.FirstOrDefault().PostGradAgencyType,
                       PostGradAgencyName = s.StudentInstitutionFundings.FirstOrDefault().PostGradAgencyName,
                       PostGradSubAgencyName = s.StudentInstitutionFundings.FirstOrDefault().PostGradSubAgencyName,
                       PostGradEOD = s.StudentInstitutionFundings.FirstOrDefault().PostGradEOD,
                       AdditionalPostGradAgencyType = s.StudentInstitutionFundings.FirstOrDefault().AdditionalPostGradAgencyType,
                       AdditionalPostGradAgencyName = s.StudentInstitutionFundings.FirstOrDefault().AdditionalPostGradAgencyName,
                       AdditionalPostGradSubAgencyName = s.StudentInstitutionFundings.FirstOrDefault().AdditionalPostGradSubAgencyName,
                       AdditionalPostGradReportedWebsite = s.StudentInstitutionFundings.FirstOrDefault().AdditionalPostGradReportedWebsite,
                       Notes = s.StudentInstitutionFundings.FirstOrDefault().Notes != null ? s.StudentInstitutionFundings.FirstOrDefault().Notes : "",
                       FollowUpDate = s.StudentInstitutionFundings.FirstOrDefault().FollowUpDate != null ? s.StudentInstitutionFundings.FirstOrDefault().FollowUpDate.Value.ToShortDateString() : "",
                       FollowUpAction = s.StudentInstitutionFundings.FirstOrDefault().FollowUpAction,
                       FollowUpType = s.StudentInstitutionFundings.FirstOrDefault().FollowUpTypeOption != null ? s.StudentInstitutionFundings.FirstOrDefault().FollowUpTypeOption.Name : "",
                       DatePendingReleaseCollectionInfo = s.StudentInstitutionFundings.FirstOrDefault().DatePendingReleaseCollectionInfo != null ? s.StudentInstitutionFundings.FirstOrDefault().DatePendingReleaseCollectionInfo.Value.ToShortDateString() : "",
                       DateReleasedCollectionPackage = s.StudentInstitutionFundings.FirstOrDefault().DateReleasedCollectionPackage != null ? s.StudentInstitutionFundings.FirstOrDefault().DateReleasedCollectionPackage.Value.ToShortDateString() : "",
                   }).OrderBy(m => m.StudentUID).ToListAsync();
            return studentData;
        }


        public async Task<List<StudentDashboardDTO>> GetAllStudentsForPIDashboardAsync(int userID)
        {
            var insitutionID = await _efDB.AcademiaUsers.Where(m => m.AcademiaUserId == userID)
                   .Select(m => m.Institution.InstitutionId).FirstOrDefaultAsync();

            var allInstitutionIds = await _efDB.Institutions.Where(m => m.InstitutionId.Equals(insitutionID) || m.ParentInstitutionID.Equals(insitutionID)).Select(m => m.InstitutionId).ToListAsync();

            var studentData = await _efDB.Students
                  .Where(m => allInstitutionIds.Contains(m.StudentInstitutionFundings.FirstOrDefault().InstitutionId.Value))
                  .Select(s => new StudentDashboardDTO()
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
                      ProfileStatus = s.ProfileStatus.Name != null ? s.ProfileStatus.Name.ToString() : "",
                      EmailAddress = s.Email.ToString(),
                      Degree = s.StudentInstitutionFundings.FirstOrDefault().Degree.Name,
                      Major = s.StudentInstitutionFundings.FirstOrDefault().Major.Name,
                      Minor = s.StudentInstitutionFundings.FirstOrDefault().Minor != null ? s.StudentInstitutionFundings.FirstOrDefault().Minor.Name : "",
                      SecondDegreeMajor = s.StudentInstitutionFundings.FirstOrDefault().SecondDegreeMajor != null ? s.StudentInstitutionFundings.FirstOrDefault().SecondDegreeMajor.Name : "",
                      SecondDegreeMinor = s.StudentInstitutionFundings.FirstOrDefault().SecondDegreeMinor != null ? s.StudentInstitutionFundings.FirstOrDefault().SecondDegreeMinor.Name : "",
                      AltEmail = s.AlternateEmail,
                      ExtensionType = s.StudentInstitutionFundings.FirstOrDefault().ExtensionType != null ? s.StudentInstitutionFundings.FirstOrDefault().ExtensionType.Extension : "No Extension",
                      ExtensionMonths = s.StudentInstitutionFundings.FirstOrDefault().ExtensionType != null ? s.StudentInstitutionFundings.FirstOrDefault().ExtensionType.Months : 0,
                      PGEmploymentDueDate = s.StudentInstitutionFundings.FirstOrDefault().PGEmploymentDueDate,
                      PGVerificationOneDueDate = s.StudentInstitutionFundings.FirstOrDefault().PGVerificationOneDueDate != null ? Convert.ToDateTime(s.StudentInstitutionFundings.FirstOrDefault().PGVerificationOneDueDate).ToShortDateString() : "",
                      PGVerificationTwoDueDate = s.StudentInstitutionFundings.FirstOrDefault().PGVerificationTwoDueDate != null ? Convert.ToDateTime(s.StudentInstitutionFundings.FirstOrDefault().PGVerificationTwoDueDate).ToShortDateString() : "",
                      PGVerificationOneCompleteDate = s.StudentInstitutionFundings.FirstOrDefault().PGVerificationOneCompleteDate != null ? s.StudentInstitutionFundings.FirstOrDefault().PGVerificationOneCompleteDate.Value.ToShortDateString() : null,
                      PGVerificationTwoCompleteDate = s.StudentInstitutionFundings.FirstOrDefault().PGVerificationTwoCompleteDate != null ? s.StudentInstitutionFundings.FirstOrDefault().PGVerificationTwoCompleteDate.Value.ToShortDateString() : null,
                      CommitmentPhaseComplete = s.StudentInstitutionFundings.FirstOrDefault().CommitmentPhaseComplete != null ? s.StudentInstitutionFundings.FirstOrDefault().CommitmentPhaseComplete.Value.ToShortDateString() : null,
                      SOCVerificationComplete = s.StudentInstitutionFundings.FirstOrDefault().SOCVerificationComplete != null ? s.StudentInstitutionFundings.FirstOrDefault().SOCVerificationComplete.Value.ToShortDateString() : null,
                      Citizenship = s.Citizenship.Value,
                      LastUpdated = s.LastUpdated,
                      Contract = s.StudentInstitutionFundings.FirstOrDefault().Contract != null ? s.StudentInstitutionFundings.FirstOrDefault().Contract.Name : "",
                      ServiceOwed = s.StudentInstitutionFundings.FirstOrDefault().ServiceOwed,
                      InternshipReported = s.StudentInstitutionFundings.FirstOrDefault().InternshipReported,
                      PostGradReported = s.StudentInstitutionFundings.FirstOrDefault().PostGradReported,
                      InternshipAgencyType = s.StudentInstitutionFundings.FirstOrDefault().InternshipAgencyType,
                      InternshipAgencyName = s.StudentInstitutionFundings.FirstOrDefault().InternshipAgencyName,
                      InternshipSubAgencyName = s.StudentInstitutionFundings.FirstOrDefault().InternshipSubAgencyName,
                      InternshipEOD = s.StudentInstitutionFundings.FirstOrDefault().InternshipEOD,
                      AdditionalInternshipAgencyType = s.StudentInstitutionFundings.FirstOrDefault().AdditionalInternshipAgencyType,
                      AdditionalInternshipAgencyName = s.StudentInstitutionFundings.FirstOrDefault().AdditionalInternshipAgencyName,
                      AdditionalInternshipSubAgencyName = s.StudentInstitutionFundings.FirstOrDefault().AdditionalInternshipSubAgencyName,
                      AdditionalInternshipReportedWebsite = s.StudentInstitutionFundings.FirstOrDefault().AdditionalInternshipReportedWebsite,
                      PostGradAgencyType = s.StudentInstitutionFundings.FirstOrDefault().PostGradAgencyType,
                      PostGradAgencyName = s.StudentInstitutionFundings.FirstOrDefault().PostGradAgencyName,
                      PostGradSubAgencyName = s.StudentInstitutionFundings.FirstOrDefault().PostGradSubAgencyName,
                      PostGradEOD = s.StudentInstitutionFundings.FirstOrDefault().PostGradEOD,
                      AdditionalPostGradAgencyType = s.StudentInstitutionFundings.FirstOrDefault().AdditionalPostGradAgencyType,
                      AdditionalPostGradAgencyName = s.StudentInstitutionFundings.FirstOrDefault().AdditionalPostGradAgencyName,
                      AdditionalPostGradSubAgencyName = s.StudentInstitutionFundings.FirstOrDefault().AdditionalPostGradSubAgencyName,
                      AdditionalPostGradReportedWebsite = s.StudentInstitutionFundings.FirstOrDefault().AdditionalPostGradReportedWebsite,
                      Notes = s.StudentInstitutionFundings.FirstOrDefault().Notes != null ? s.StudentInstitutionFundings.FirstOrDefault().Notes : "",
                      FollowUpDate = s.StudentInstitutionFundings.FirstOrDefault().FollowUpDate != null ? s.StudentInstitutionFundings.FirstOrDefault().FollowUpDate.Value.ToShortDateString() : "",
                      FollowUpAction = s.StudentInstitutionFundings.FirstOrDefault().FollowUpAction,
                      FollowUpType = s.StudentInstitutionFundings.FirstOrDefault().FollowUpTypeOption != null ? s.StudentInstitutionFundings.FirstOrDefault().FollowUpTypeOption.Name : "",
                      DatePendingReleaseCollectionInfo = s.StudentInstitutionFundings.FirstOrDefault().DatePendingReleaseCollectionInfo != null ? s.StudentInstitutionFundings.FirstOrDefault().DatePendingReleaseCollectionInfo.Value.ToShortDateString() : "",
                      DateReleasedCollectionPackage = s.StudentInstitutionFundings.FirstOrDefault().DateReleasedCollectionPackage != null ? s.StudentInstitutionFundings.FirstOrDefault().DateReleasedCollectionPackage.Value.ToShortDateString() : "",
                  }).OrderBy(m => m.StudentUID).ToListAsync();
            return studentData;
        }

        public async Task<List<CommitmentReportedDTO>> GetInternshipsByStudentID(int studentID)
        {
            var agencyList = await _efDB.Agencies.ToListAsync();
            var commitmentData = await _efDB.StudentCommitments.Where(m => m.StudentId == studentID)
				.Where(x => x.CommitmentStatus.Value == "POApproved")
				.Where(x => x.StartDate.HasValue)
				.Where(m => !m.IsDeleted)
                .Where(m => m.CommitmentType.Name == "Internship")
                .Select(s => new CommitmentReportedDTO()
                {
                    StudentID = s.StudentId,
                    CommitmentID = s.StudentCommitmentId,
                    ParentAgencyID = s.Agency.ParentAgencyId,
                    AgencyName = s.Agency.Name,
                    AgencyType = s.Agency.AgencyType.Name,
                    StartDate = s.StartDate,
                    SubAgencyName = s.Agency.ParentAgencyId.HasValue ? s.Agency.Name : "",
                    Status = s.CommitmentStatus.Value
                }).OrderBy(m => m.CommitmentID).ToListAsync();

            foreach (var item in commitmentData)
            {
                var temp = item.AgencyName;
                item.AgencyName = item.ParentAgencyID != null && item.ParentAgencyID > 0 ? agencyList.Where(m => m.AgencyId == item.ParentAgencyID).FirstOrDefault().Name : item.AgencyName;
                item.SubAgencyName = item.ParentAgencyID != null ? temp : "N/A";
            }
            return commitmentData;
		}
		public async Task DeleteOldCommitmentData(int studentID)
		{
			var toUpdate = await _efDB.StudentInstitutionFundings.Where(m => m.StudentId == studentID).FirstOrDefaultAsync();
			toUpdate.InternshipAgencyName = null;
			toUpdate.InternshipAgencyType = null;
			toUpdate.InternshipEOD = null;
			toUpdate.InternshipSubAgencyName = null;
			toUpdate.InternshipReported = null;
			toUpdate.AdditionalInternshipAgencyName = null;
			toUpdate.AdditionalInternshipAgencyType = null;
			toUpdate.AdditionalInternshipSubAgencyName = null;
			toUpdate.AdditionalInternshipReportedWebsite = null;
			toUpdate.PostGradAgencyName = null;
			toUpdate.PostGradAgencyType = null;
			toUpdate.PostGradEOD = null;
			toUpdate.PostGradSubAgencyName = null;
			toUpdate.PostGradReported = null;
			toUpdate.PGVerificationOneDueDate = null;
            toUpdate.PGVerificationTwoDueDate = null;
			toUpdate.AdditionalPostGradAgencyName = null;
			toUpdate.AdditionalPostGradAgencyType = null;
			toUpdate.AdditionalPostGradSubAgencyName = null;
			toUpdate.AdditionalPostGradReportedWebsite = null;
			await _efDB.SaveChangesAsync();
		}
		public async Task UpdateInternshipReported(CommitmentReportedDTO data)
        {
            var toUpdate = await _efDB.StudentInstitutionFundings.Where(m => m.StudentId == data.StudentID).FirstOrDefaultAsync();
            toUpdate.InternshipAgencyName= data.AgencyName;
            toUpdate.InternshipAgencyType= data.AgencyType;
            toUpdate.InternshipEOD = data.StartDate;
            toUpdate.InternshipSubAgencyName = data.SubAgencyName;
            toUpdate.InternshipReported = "Yes";
            await _efDB.SaveChangesAsync();
        }

		public async Task UpdateAdditionalInternshipReported(CommitmentReportedDTO data)
		{
			var toUpdate = await _efDB.StudentInstitutionFundings.Where(m => m.StudentId == data.StudentID).FirstOrDefaultAsync();
			toUpdate.AdditionalInternshipAgencyName = data.AgencyName;
			toUpdate.AdditionalInternshipAgencyType = data.AgencyType;			
			toUpdate.AdditionalInternshipSubAgencyName = data.SubAgencyName;
			toUpdate.AdditionalInternshipReportedWebsite = "Yes";
			await _efDB.SaveChangesAsync();
		}

		public async Task<List<CommitmentReportedDTO>> GetPostGradCommitmentsByStudentID(int studentID)
		{
			var agencyList = await _efDB.Agencies.ToListAsync();
			var commitmentData = await _efDB.StudentCommitments.Where(m => m.StudentId == studentID)
			    .Where(x => x.CommitmentStatus.Value == "POApproved")
				.Where(x => x.StartDate.HasValue)
				.Where(m => m.CommitmentType.Name == "Postgraduate")
                .Where(m => !m.IsDeleted)
				.Select(s => new CommitmentReportedDTO()
				{
					StudentID = s.StudentId,
					CommitmentID = s.StudentCommitmentId,
					AgencyName =  s.Agency.Name,
					AgencyType = s.Agency.AgencyType.Name,
					StartDate = s.StartDate,
                    ParentAgencyID = s.Agency.ParentAgencyId,
                    ServiceOwed = s.Student.StudentInstitutionFundings.FirstOrDefault().ServiceOwed.ToString()
				}).OrderBy(m => m.CommitmentID).ToListAsync();

            foreach (var item in commitmentData)
            {
                var temp = item.AgencyName;
                item.AgencyName = item.ParentAgencyID != null && item.ParentAgencyID > 0 ? agencyList.Where(m => m.AgencyId == item.ParentAgencyID).FirstOrDefault().Name : item.AgencyName;
                item.SubAgencyName = item.ParentAgencyID != null ? temp : "N/A";
            }
            return commitmentData;
		}

		public async Task UpdatePostGradReported(CommitmentReportedDTO data, DateTime? verification1, DateTime? verification2)
		{
			var toUpdate = await _efDB.StudentInstitutionFundings.Where(m => m.StudentId == data.StudentID).FirstOrDefaultAsync();
			toUpdate.PostGradAgencyName = data.AgencyName;
			toUpdate.PostGradAgencyType = data.AgencyType;
			toUpdate.PostGradEOD = data.StartDate;
			toUpdate.PostGradSubAgencyName = data.SubAgencyName;
            toUpdate.PostGradReported = "Yes";
            if (!toUpdate.PGVerificationOneDueDate.HasValue)
            {
                toUpdate.PGVerificationOneDueDate = verification1;
            }
            if (!toUpdate.PGVerificationTwoDueDate.HasValue)
            {
                toUpdate.PGVerificationTwoDueDate = verification2;
            }
            await _efDB.SaveChangesAsync();
		}

		public async Task UpdateAdditionalPostGradReported(CommitmentReportedDTO data)
		{
			var toUpdate = await _efDB.StudentInstitutionFundings.Where(m => m.StudentId == data.StudentID).FirstOrDefaultAsync();
			toUpdate.AdditionalPostGradAgencyName = data.AgencyName;
			toUpdate.AdditionalPostGradAgencyType = data.AgencyType;
			toUpdate.AdditionalPostGradSubAgencyName = data.SubAgencyName;
            toUpdate.AdditionalPostGradReportedWebsite = "Yes";
			await _efDB.SaveChangesAsync();
		}

        public async Task AppendNotesForCommitmentsReported(int studentID, string noteToAdd)
        {
            var toUpdate = await _efDB.StudentInstitutionFundings.Where(m => m.StudentId == studentID).FirstOrDefaultAsync();
            if (!string.IsNullOrWhiteSpace(toUpdate.Notes))
                toUpdate.Notes = $"{toUpdate.Notes} ; {noteToAdd}";
            else
                toUpdate.Notes = noteToAdd;
            await _efDB.SaveChangesAsync();
        }

        public async Task<string> GetNotesByStudentID(int studentID)
        {
            var notes = await _efDB.StudentInstitutionFundings.Where(m => m.StudentId == studentID)
                .Select(s => s.Notes).FirstOrDefaultAsync();
            return notes;

        }

        public async Task<Student> GetStudentByStudentUID(int studentUID)
        {
            return await _efDB.Students.Where(m => m.StudentUID == studentUID).Include(m => m.ProfileStatus).FirstOrDefaultAsync();
        }

        public async Task<StudentRegistrationDTO> GetStudentByEmail(string email)
        {
            return await _efDB.Students.Where(m => m.Email == email)
                .Select(m => new StudentRegistrationDTO()
                {
                    StudentID = m.StudentId,
                    ProfileStatus = m.ProfileStatus.Name,
                    Email = m.Email,
                    SSN = m.Ssn
                })
                .FirstOrDefaultAsync();
        }

		public async Task<StudentRegistrationDTO> GetStudentBySSN(string ssn)
		{
			return await _efDB.Students.Where(m => m.Ssn == ssn)
				.Select(m => new StudentRegistrationDTO()
				{
					StudentID = m.StudentId,
					ProfileStatus = m.ProfileStatus.Name,
					Email = m.Email,
					SSN = m.Ssn
				})
				.FirstOrDefaultAsync();
		}

		public async Task<int> AddOrUpdateStudent(StudentRegistrationViewModel student)
        {
            int studentID = 0;
            if (student.UID == 0)
            {
                var studentRecord = await _efDB.Students.Where(m => m.Email == student.Email)
                    .Include(m => m.StudentAccount)
                    .Include(m => m.StudentInstitutionFundings)
                    .FirstOrDefaultAsync();

                if (studentRecord == null)
                    studentID = await InsertStudentForRegistration(student);
                else
                    studentID = await UpdateStudentForRegistration(studentRecord, student);
            }
            else
            {
                var studentRecord = await _efDB.Students.Where(m => m.StudentUID == student.UID)
                   .Include(m => m.StudentAccount)
                   .Include(m => m.StudentInstitutionFundings)
                   .FirstOrDefaultAsync();
                studentID = await UpdateStudentForRegistration(studentRecord, student);
            }
                
            return studentID; 
        }

        public async Task<int> GetMaxUID()
        {
            return await _efDB.Students.MaxAsync(m => m.StudentUID);
        }

        public async Task<RegistrationCode> GetStudentRegistrationCode(string code)
        {
            return await _efDB.RegistrationCodes.Where(m => m.Code.ToUpper() == code.ToUpper()).FirstOrDefaultAsync();
        }

        public async Task<List<StudentDashboardDTO>> GetAllStudentForDashboardWithSP()
        {
            var strConnection = _efDB.Database.GetConnectionString();
            using (var connection = new SqlConnection(strConnection))
            {
                var storedProcedureName = "GetAllStudentsForDashboard";
                var results = await connection.QueryAsync<StudentDashboardDTO>(storedProcedureName, null, commandType: CommandType.StoredProcedure);
                return results.ToList();
            }

        }

        private async Task<int> InsertStudentForRegistration(StudentRegistrationViewModel student)
        {
            var pendingID = _efDB.ProfileStatus.Where(m => m.Name == "Pending").Select(m => m.ProfileStatusID).FirstOrDefault();
            SFS.Data.Student newStudent = new();
            newStudent.StudentInstitutionFundings = new List<StudentInstitutionFunding>();

            newStudent.FirstName = student.Firstname;
            newStudent.MiddleName = student.Middlename;
            newStudent.LastName = student.Lastname;
            newStudent.Suffix = student.Suffix;
            newStudent.Ssn = student.SSN;
            newStudent.DateOfBirth = Convert.ToDateTime(student.DateOfBirth);
            newStudent.Email = student.Email;
            newStudent.AlternateEmail = student.AlternateEmail;
            newStudent.DateAdded = DateTime.UtcNow;
            newStudent.ProfileStatusID = pendingID;
            newStudent.StudentUID = await GetMaxUID() + 1;
            newStudent.StudentAccount = new StudentAccount()
            {
                UserName = student.Email,
                Password = "",
                IsDisabled = false,
                FailedLoginCount = 0,
                LastLoginDate = null
            };

            newStudent.StudentInstitutionFundings.Add(new StudentInstitutionFunding()
            {
                InstitutionId = Convert.ToInt32(student.SelectedCollege),
                MajorId = student.SelectedDiscipline,
                DegreeId = student.SelectedDegree,
                InitialFundingDate = Convert.ToDateTime(student.InitialFundingDate),
                ExpectedGradDate = Convert.ToDateTime(student.GraduationDate),
                MinorId = student.SelectedMinor > 0 ? student.SelectedMinor : null,
                SecondDegreeMajorId = Convert.ToInt32(student.SelectedSecondDegreeMajor) > 0 ? Convert.ToInt32(student.SelectedSecondDegreeMajor) : null,
                SecondDegreeMinorId = Convert.ToInt32(student.SelectedSecondDegreeMinor) > 0 ? Convert.ToInt32(student.SelectedSecondDegreeMinor) : null
            });

            await _efDB.Students.AddAsync(newStudent);
            await _efDB.SaveChangesAsync();
            return newStudent.StudentId;
        }

        private async Task<int> UpdateStudentForRegistration(Student toUpdate, StudentRegistrationViewModel student)
        {
            var pendingID = _efDB.ProfileStatus.Where(m => m.Name == "Pending").Select(m => m.ProfileStatusID).FirstOrDefault();
            toUpdate.FirstName = student.Firstname;
            toUpdate.MiddleName = student.Middlename;
            toUpdate.LastName = student.Lastname;
            toUpdate.Suffix = student.Suffix;
            toUpdate.Ssn = student.SSN;
            toUpdate.DateOfBirth = Convert.ToDateTime(student.DateOfBirth);
            toUpdate.Email = student.Email;
            toUpdate.AlternateEmail = student.AlternateEmail;
            toUpdate.LastUpdated = DateTime.UtcNow;
            toUpdate.ProfileStatusID = pendingID;
            toUpdate.StudentAccount.UserName = student.Email;
            toUpdate.StudentAccount.Password = "";
            toUpdate.StudentAccount.IsDisabled = false;
            toUpdate.StudentAccount.FailedLoginCount = 0;
            toUpdate.StudentAccount.LastLoginDate = null;
            toUpdate.StudentInstitutionFundings.FirstOrDefault().InstitutionId = Convert.ToInt32(student.SelectedCollege);
            toUpdate.StudentInstitutionFundings.FirstOrDefault().MajorId = Convert.ToInt32(student.SelectedDiscipline);
            toUpdate.StudentInstitutionFundings.FirstOrDefault().DegreeId = Convert.ToInt32(student.SelectedDegree);
            toUpdate.StudentInstitutionFundings.FirstOrDefault().InitialFundingDate = Convert.ToDateTime(student.InitialFundingDate);
            toUpdate.StudentInstitutionFundings.FirstOrDefault().ExpectedGradDate = Convert.ToDateTime(student.GraduationDate);
            toUpdate.StudentInstitutionFundings.FirstOrDefault().MinorId = Convert.ToInt32(student.SelectedMinor) > 0 ? Convert.ToInt32(student.SelectedMinor) : null;
            toUpdate.StudentInstitutionFundings.FirstOrDefault().SecondDegreeMajorId = Convert.ToInt32(student.SelectedSecondDegreeMajor) > 0 ? Convert.ToInt32(student.SelectedSecondDegreeMajor) : null;
            toUpdate.StudentInstitutionFundings.FirstOrDefault().SecondDegreeMinorId = Convert.ToInt32(student.SelectedSecondDegreeMinor) > 0 ? Convert.ToInt32(student.SelectedSecondDegreeMinor) : null;
            await _efDB.SaveChangesAsync();
            return toUpdate.StudentId;

        }

        public async Task<List<StudentDashboardCommitmentsDTO>> GetAllStudentCommitmentsForDashboardWithSP()
        {
            var strConnection = _efDB.Database.GetConnectionString();
            using (var connection = new SqlConnection(strConnection))
            {
                var storedProcedureName = "GetAllStudentsCommitmentsForDashboard";
                var results = await connection.QueryAsync<StudentDashboardCommitmentsDTO>(storedProcedureName, null, commandType: CommandType.StoredProcedure);
                return results.ToList();
            }

        }

		public async Task<List<StudentDashboardCommitmentsDTO>> GetAllStudentCommitmentsForDashboardByStudentIDWithSP(string studentID)
		{
			var strConnection = _efDB.Database.GetConnectionString();
			using (var connection = new SqlConnection(strConnection))
			{
				var storedProcedureName = "GetAllStudentsCommitmentsForDashboard";
				var _params = new { StudentID = studentID };
				var results = await connection.QueryAsync<StudentDashboardCommitmentsDTO>(storedProcedureName, _params, commandType: CommandType.StoredProcedure);
				return results.ToList();
			}

		}

		public async Task<List<CommitmentReportedDTO>> GetApprovedCommitmentsByStudentID(int studentID)
		{
			var agencyList = await _efDB.Agencies.ToListAsync();
			var commitmentData = await _efDB.StudentCommitments.Where(m => m.StudentId == studentID)
				.Where(x => x.CommitmentStatus.Value == "POApproved")
				.Where(x => x.StartDate.HasValue)
				.Where(m => !m.IsDeleted)
				.Select(s => new CommitmentReportedDTO()
				{
					StudentID = s.StudentId,
					CommitmentID = s.StudentCommitmentId,
                    CommitmentType = s.CommitmentType.Name,
					ParentAgencyID = s.Agency.ParentAgencyId,
					AgencyName = s.Agency.Name,
					AgencyType = s.Agency.AgencyType.Name,
					StartDate = s.StartDate,
					SubAgencyName = s.Agency.ParentAgencyId.HasValue ? s.Agency.Name : "",
					Status = s.CommitmentStatus.Value
				}).OrderBy(m => m.StartDate).ToListAsync();

			foreach (var item in commitmentData)
			{
				var temp = item.AgencyName;
				item.AgencyName = item.ParentAgencyID != null ? agencyList.Where(m => m.AgencyId == item.ParentAgencyID).FirstOrDefault().Name : item.AgencyName;
				item.SubAgencyName = item.ParentAgencyID != null ? temp : "N/A";
			}
			return commitmentData;
		}

		public async Task<List<CommitmentReportedDTO>> GetAllSubmittedCommitmentsByStudentID(int studentID)
		{
			var agencyList = await _efDB.Agencies.ToListAsync();
			var commitmentData = await _efDB.StudentCommitments.Where(m => m.StudentId == studentID)
				.Where(x => x.CommitmentStatus.Value != "POReject")
				.Where(x => x.CommitmentStatus.Value != "Incomplete")
				.Where(m => !m.IsDeleted)
				.Select(s => new CommitmentReportedDTO()
				{
					StudentID = s.StudentId,
					CommitmentID = s.StudentCommitmentId,
					CommitmentType = s.CommitmentType.Name,
					ParentAgencyID = s.Agency.ParentAgencyId,
					AgencyName = s.Agency.Name,
					AgencyType = s.Agency.AgencyType.Name,
					StartDate = s.StartDate,
					SubAgencyName = s.Agency.ParentAgencyId.HasValue ? s.Agency.Name : "",
					Status = s.CommitmentStatus.Value,
                    StatusDisplay = s.CommitmentStatus.AdminDisplay
				}).OrderBy(m => m.StartDate).ToListAsync();

			foreach (var item in commitmentData)
			{
				var temp = item.AgencyName;
				item.AgencyName = item.ParentAgencyID != null && item.ParentAgencyID > 0 ? agencyList.Where(m => m.AgencyId == item.ParentAgencyID).FirstOrDefault().Name : item.AgencyName;
				item.SubAgencyName = item.ParentAgencyID != null && item.ParentAgencyID > 0 ? temp : "N/A";
			}
			return commitmentData;
		}

        public async Task UpdateStudentDashboardPGReportedField(int studentID, string pgReported)
        {
            var studentDashboardRow = _efDB.StudentInstitutionFundings.Where(m => m.StudentId == studentID).FirstOrDefault();
            if(studentDashboardRow != null)
            {				
				studentDashboardRow.PostGradReported = pgReported;
				await _efDB.SaveChangesAsync();
			}
            
        }

		public async Task UpdateStudentDashboardINReportedField(int studentID, string inReported)
		{
			var studentDashboardRow = _efDB.StudentInstitutionFundings.Where(m => m.StudentId == studentID).FirstOrDefault();
			if (studentDashboardRow != null)
			{
				studentDashboardRow.InternshipReported = inReported;
				await _efDB.SaveChangesAsync();
			}

		}

        public async Task<DocumentScanIno> HandleMalwareScanResults(int studentID)
        {
            DocumentScanIno result = new();
            var maliciousScanDoc = await _efDB.StudentDocuments.Where(m => m.StudentId == studentID)
                .Where(m => m.LastUpdatedBy == "Malicious Malware Scan Result")
                .Include(m => m.DocumentType)
                .FirstOrDefaultAsync();

            if(maliciousScanDoc != null)
            {
                result.FileName = maliciousScanDoc.FileName;
                result.DocumentType = maliciousScanDoc.DocumentType.Name;
                _efDB.StudentDocuments.Remove(maliciousScanDoc);
                await _efDB.SaveChangesAsync();
            }
            return result;
        }

        public async Task<CommitmentVerificationDTO> GetCommitmentVerificationData(int studentID)
        {
            var mainData = await _efDB.Students.Where(m => m.StudentId == studentID)
                .Select(p => new CommitmentVerificationDTO()
                {
                    ServiceOwed = p.StudentInstitutionFundings.FirstOrDefault().ServiceOwed.ToString(),
                    PGVerificationOneDue = p.StudentInstitutionFundings.FirstOrDefault().PGVerificationOneDueDate.Value.ToShortDateString(),
                    PGVerificationOneComplete = p.StudentInstitutionFundings.FirstOrDefault().PGVerificationOneCompleteDate.Value.ToShortDateString(),
                    PGVerificationTwoDue = p.StudentInstitutionFundings.FirstOrDefault().PGVerificationTwoDueDate.Value.ToShortDateString(),
                    PGVerificationTwoComplete = p.StudentInstitutionFundings.FirstOrDefault().PGVerificationTwoCompleteDate.Value.ToShortDateString(),
                    SOCDueDate = p.StudentInstitutionFundings.FirstOrDefault().CommitmentPhaseComplete.Value.ToShortDateString(),
                    TotalServiceObligation = p.StudentInstitutionFundings.FirstOrDefault().ServiceOwed.ToString()
                })
                .FirstOrDefaultAsync();
            return mainData;
        }

        public async Task<List<CommitmentVerificationDetailsDTO>> GetCommitmentVerificationDetailsData(int studentID)
        {
            var commitmentData = await _efDB.StudentCommitments.Where(m => m.StudentId == studentID)
                .Where(m => !m.IsDeleted)
                .OrderByDescending(m => m.StudentCommitmentId)
                .Select(m => new CommitmentVerificationDetailsDTO()
                {
                    ID = m.StudentCommitmentId,
                    Agency = m.Agency.Name,
                    JobTitle = m.JobTitle,
                    StartDate = m.StartDate,
                    Type = m.CommitmentType.Name,
                    Status = m.CommitmentStatus.StudentDisplay,
                    EVFStatus = m.EmploymentVerification.Status,
                    EVFDateSubmitted = m.EmploymentVerification.DateInserted.ToShortDateString()
                })
                .ToListAsync();
            return commitmentData;
        }

    }

    public class DocumentScanIno
    {
        public string FileName { get; set; }
        public string DocumentType { get; set; }
    }

  
}
