using OPM.SFS.Data;
using OPM.SFS.Web.Shared;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text;
using OPM.SFS.Core.Shared;
using FluentEmail.Core;

namespace OPM.SFS.Web.SharedCode
{
    public interface ICommitmentNotificationService
    {
        Task SendEmaislWhenPIReviewsAsync(CommitmentNotificationRequest request);
        Task SendEmailWhenPOApprovesAsync(CommitmentNotificationRequest request);
        Task SendEmailWhenPORequestFinalDocsAsync(CommitmentNotificationRequest request);
        Task SendEmailWhenStudentSubmittedAsync(CommitmentNotificationRequest request);
        Task SendEmailWhenPORejectsAsync(CommitmentNotificationRequest request);
        Task SendEmailWhenStudentUploadEVFDocument(List<CommitmentNotificationRequest> request);
    }

    public class CommitmentNotificationService : ICommitmentNotificationService
    {

        private readonly ScholarshipForServiceContext _efDB;
        private readonly ICacheHelper _cache;
        private readonly IEmailerService _emailer;
        private readonly INotificationList _notificationList;
        private readonly IReferenceDataRepository _refRepo;


        public CommitmentNotificationService(ScholarshipForServiceContext efDB, IReferenceDataRepository refRepo, ICacheHelper cache, IEmailerService emailer, INotificationList notificationList)
        {
            _refRepo = refRepo;
            _efDB = efDB;
            _cache = cache;
            _emailer = emailer;
            _notificationList = notificationList;
        }

        public async Task SendEmailWhenStudentSubmittedAsync(CommitmentNotificationRequest request)
        {
            //Send emails when commitment is submitted by student and when student is submitting additional information
            var agencyList = await _cache.GetAgenciesAsync();
            string agency = request.Commitment.Agency.Name;
            string subAgency = "N/A";
            if (request.Commitment.Agency.ParentAgencyId.HasValue && request.Commitment.Agency.ParentAgencyId.Value > 0)
            {
                agency = agencyList.Where(m => m.AgencyId == request.Commitment.Agency.ParentAgencyId).Select(m => m.Name).FirstOrDefault();
                subAgency = request.Commitment.Agency.Name;
            }
            if (request.Commitment.Agency.CommitmentApprovalWorkflow.Code == CommitmentProcessConst.CommitmentApprovalFinal)
            {
                _ = await _emailer.SendEmailWithTemplateAsync(request.Commitment.Student.Email, "FedExec_ToStudent_Commitment_PO_Review_PD_FOL", new EmailTemplateModel()
                {
                    StudentFullName = $"{request.Commitment.Student.FirstName} {request.Commitment.Student.LastName}",
                    AgencyName = agency,
                    SubAgency = subAgency,
                    CommitmentType = request.Commitment.CommitmentType.Name,
                    AgencyType = request.Commitment.Agency.AgencyType.Name,
                    JobTitle = request.Commitment.JobTitle,
                    StudentEntryOnDuty = request.Commitment.StartDate.HasValue ? request.Commitment.StartDate.Value.ToShortDateString() : ""

                });

                _ = await _emailer.SendEmailWithTemplateAsync(_notificationList.PO_NotificationList(), "FedExec_ToPO_Pending_FOL_PD_Review", new EmailTemplateModel()
                {
                    StudentFullName = $"{request.Commitment.Student.FirstName} {request.Commitment.Student.LastName}",
                    StudentEmailAddress = request.Commitment.Student.Email,
                    StudentAlternateEmailAddress = string.IsNullOrEmpty(request.Commitment.Student.AlternateEmail) ? "N/A" : request.Commitment.Student.AlternateEmail,
                    StudentInstitution = request.Commitment.Student.StudentInstitutionFundings.FirstOrDefault().Institution.Name,
                    StudentGraduationDate = request.Commitment.Student.StudentInstitutionFundings.FirstOrDefault().ExpectedGradDate.HasValue ? $"{request.Commitment.Student.StudentInstitutionFundings.FirstOrDefault().ExpectedGradDate.Value.Month}// {request.Commitment.Student.StudentInstitutionFundings.FirstOrDefault().ExpectedGradDate.Value.Year}" : "",
                    AgencyName = agency,
                    SubAgency = subAgency,
                    CommitmentType = request.Commitment.CommitmentType.Name,
                    AgencyType = request.Commitment.Agency.AgencyType.Name,
                    JobTitle = request.Commitment.JobTitle,
                    StudentEntryOnDuty = request.Commitment.StartDate.HasValue ? request.Commitment.StartDate.Value.ToShortDateString() : ""

                });
            }
            else
            {
                if (request.Commitment.CommitmentStatus.Code == CommitmentProcessConst.RequestFinalDocs || request.Commitment.CommitmentStatus.Code == CommitmentProcessConst.FinalDocsPendingApproval)
                {
                    _ = await _emailer.SendEmailWithTemplateAsync(request.Commitment.Student.Email, "NonExec_ToStudent_Commitment_PO_Review_PD_FOL", new EmailTemplateModel()
                    {
                        StudentFullName = $"{request.Commitment.Student.FirstName} {request.Commitment.Student.LastName}",
                        AgencyName = agency,
                        SubAgency = subAgency,
                        AgencyType = request.Commitment.Agency.AgencyType.Name,
                        CommitmentType = request.Commitment.CommitmentType.Name,
                        JobTitle = request.Commitment.JobTitle,
                        Justification = request.Commitment.Justification
                    });

                    _ = await _emailer.SendEmailWithTemplateAsync(_notificationList.PO_NotificationList(), "NonExec_ToPO_Pending_FOL_PD_Review", new EmailTemplateModel()
                    {
                        StudentFullName = $"{request.Commitment.Student.FirstName} {request.Commitment.Student.LastName}",
                        StudentEmailAddress = request.Commitment.Student.Email,
                        StudentAlternateEmailAddress = string.IsNullOrEmpty(request.Commitment.Student.AlternateEmail) ? "N/A" : request.Commitment.Student.AlternateEmail,
                        StudentInstitution = request.Commitment.Student.StudentInstitutionFundings.FirstOrDefault().Institution.Name,
                        StudentGraduationDate = request.Commitment.Student.StudentInstitutionFundings.FirstOrDefault().ExpectedGradDate.HasValue ? $"{request.Commitment.Student.StudentInstitutionFundings.FirstOrDefault().ExpectedGradDate.Value.Month}// {request.Commitment.Student.StudentInstitutionFundings.FirstOrDefault().ExpectedGradDate.Value.Year}" : "",
                        AgencyName = agency,
                        SubAgency = subAgency,
                        CommitmentType = request.Commitment.CommitmentType.Name,
                        AgencyType = request.Commitment.Agency.AgencyType.Name,
                        JobTitle = request.Commitment.JobTitle,
                        StudentEntryOnDuty = request.Commitment.StartDate.HasValue ? request.Commitment.StartDate.Value.ToShortDateString() : "",
                        Justification = request.Commitment.Justification,
                        PIRecommendation = request.Commitment.PIRecommendation == "PIReject" ? "Reject" : "Approve"
                    });
                }
                else
                {
                    var toPIList = await _notificationList.PI_NotificationListAsync(request.InstitutionID);

                    _ = await _emailer.SendEmailWithTemplateAsync(toPIList, "NonExec_ToPI_Pending_PI_Review", new EmailTemplateModel()
                    {
                        StudentFullName = $"{request.Commitment.Student.FirstName} {request.Commitment.Student.LastName}",
                        StudentEmail = request.Commitment.Student.Email,
                        StudentInstitution = request.Commitment.Student.StudentInstitutionFundings.FirstOrDefault().Institution.Name,
                        AgencyName = agency,
                        SubAgency = subAgency,
                        AgencyType = request.Commitment.Agency.AgencyType.Name,
                        CommitmentType = request.Commitment.CommitmentType.Name,
                        JobTitle = request.Commitment.JobTitle,
                        Justification = request.Commitment.Justification
                    });

                    _ = await _emailer.SendEmailWithTemplateAsync(request.Commitment.Student.Email, "NonExec_ToStudent_Commitment_Pending_PI_Review", new EmailTemplateModel()
                    {
                        StudentFullName = $"{request.Commitment.Student.FirstName} {request.Commitment.Student.LastName}",
                        AgencyName = agency,
                        SubAgency = subAgency,
                        AgencyType = request.Commitment.Agency.AgencyType.Name,
                        CommitmentType = request.Commitment.CommitmentType.Name,
                        JobTitle = request.Commitment.JobTitle,
                        Justification = request.Commitment.Justification,

                    });
                }

            }
        }

        public async Task SendEmaislWhenPIReviewsAsync(CommitmentNotificationRequest request)
        {
            var GlobalConfigSettings = await _refRepo.GetGlobalConfigurationsAsync();

            var agencyList = await _cache.GetAgenciesAsync();
            string agency = request.Commitment.Agency.Name;
            string subAgency = "N/A";
            if (request.Commitment.Agency.ParentAgencyId.HasValue && request.Commitment.Agency.ParentAgencyId.Value > 0)
            {
                agency = agencyList.Where(m => m.AgencyId == request.Commitment.Agency.ParentAgencyId).Select(m => m.Name).FirstOrDefault();
                subAgency = request.Commitment.Agency.Name;
            }

            var toPIList = await _notificationList.PI_NotificationListAsync(request.InstitutionID);
            //Send email to PI for confirmation
            _ = await _emailer.SendEmailWithTemplateAsync(toPIList, "NonExec_ToPI_Pending_Commitment_PO_Review", new EmailTemplateModel()
            {
                StudentFullName = $"{request.Commitment.Student.FirstName} {request.Commitment.Student.LastName}",
                StudentEmail = request.Commitment.Student.Email,
                StudentInstitution = request.Commitment.Student.StudentInstitutionFundings.FirstOrDefault().Institution.Name,
                AgencyName = agency,
                SubAgency = subAgency,
                AgencyType = request.Commitment.Agency.AgencyType.Name,
                CommitmentType = request.Commitment.CommitmentType.Name,
                JobTitle = request.Commitment.JobTitle,
                Justification = request.Commitment.Justification,
                PIRecommendation = request.Commitment.PIRecommendation == "Reject" ? "Reject" : "Approve"
            });

            //Send email to the student for status change
            _ = await _emailer.SendEmailWithTemplateAsync(request.Commitment.Student.Email, "NonExec_ToStudent_Commitment_Pending_PO_Review", new EmailTemplateModel()
            {
                StudentFullName = $"{request.Commitment.Student.FirstName} {request.Commitment.Student.LastName}",
                AgencyName = agency,
                SubAgency = subAgency,
                AgencyType = request.Commitment.Agency.AgencyType.Name,
                CommitmentType = request.Commitment.CommitmentType.Name,
                JobTitle = request.Commitment.JobTitle,
                Justification = request.Commitment.Justification
            });

            //Send email to the PO for review
            string toEmail = GlobalConfigSettings.Where(m => m.Key == "ProgramOfficeURI" && m.Type == "EmailSettings").Select(m => m.Value).FirstOrDefault();
            _ = await _emailer.SendEmailWithTemplateAsync(toEmail, "NonExec_ToPO_Pending_Review", new EmailTemplateModel()
            {
                StudentFullName = $"{request.Commitment.Student.FirstName} {request.Commitment.Student.LastName}",
                StudentEmailAddress = request.Commitment.Student.Email,
                StudentAlternateEmailAddress = request.Commitment.Student.AlternateEmail,
                StudentInstitution = request.Commitment.Student.StudentInstitutionFundings.FirstOrDefault().Institution.Name,
                AgencyName = agency,
                SubAgency = subAgency,
                AgencyType = request.Commitment.Agency.AgencyType.Name,
                CommitmentType = request.Commitment.CommitmentType.Name,
                JobTitle = request.Commitment.JobTitle,
                Justification = request.Commitment.Justification,
                PIRecommendation = request.Commitment.PIRecommendation == "Reject" ? "Reject" : "Approve"
            });

        }

        public async Task SendEmailWhenPOApprovesAsync(CommitmentNotificationRequest request)
        {
            var GlobalConfigSettings = await _refRepo.GetGlobalConfigurationsAsync();
            var agencyList = await _cache.GetAgenciesAsync();
            string toEmail = GlobalConfigSettings.Where(m => m.Key == "ProgramOfficeURI" && m.Type == "EmailSettings").Select(m => m.Value).FirstOrDefault();
            string agency = request.Commitment.Agency.Name;
            string subAgency = "N/A";
            if (request.Commitment.Agency.ParentAgencyId.HasValue && request.Commitment.Agency.ParentAgencyId.Value > 0)
            {
                agency = agencyList.Where(m => m.AgencyId == request.Commitment.Agency.ParentAgencyId).Select(m => m.Name).FirstOrDefault();
                subAgency = request.Commitment.Agency.Name;
            }


            if (request.Commitment.CommitmentType.Code == CommitmentProcessConst.CommitmentTypeInternship || request.Commitment.CommitmentType.Code == CommitmentProcessConst.CommitmentTypePostGrad)
            {
                //To Student
                _ = await _emailer.SendEmailWithTemplateAsync(request.Commitment.Student.Email, "FedExec_ToStudent_Commitment_PO_Approved", new EmailTemplateModel()
                {
                    StudentFullName = $"{request.Commitment.Student.FirstName} {request.Commitment.Student.LastName}"

                });

                //To PO
                _ = await _emailer.SendEmailWithTemplateAsync(toEmail, "FedExec_ToPO_Approved_MatchReport", new EmailTemplateModel()
                {
                    StudentName = $"{request.Commitment.Student.FirstName} {request.Commitment.Student.LastName}",
                    StudentGraduationDate = request.Commitment.Student.StudentInstitutionFundings.FirstOrDefault().ExpectedGradDate.HasValue ? $"{request.Commitment.Student.StudentInstitutionFundings.FirstOrDefault().ExpectedGradDate.Value.Month}/{request.Commitment.Student.StudentInstitutionFundings.FirstOrDefault().ExpectedGradDate.Value.Year}" : "",
                    StudentInstitution = request.Commitment.Student.StudentInstitutionFundings.FirstOrDefault().Institution.Name,
                    AgencyName = agency,
                    SubAgencyName = subAgency,
                    CommitmentType = request.Commitment.CommitmentType.Name,
                    AgencyType = request.Commitment.Agency.AgencyType.Name,
                    JobTitle = request.Commitment.JobTitle,
                    StudentEntryOnDuty = request.Commitment.StartDate.HasValue ? request.Commitment.StartDate.Value.ToShortDateString() : "",
                    TypeOfMatch = request.Commitment.CommitmentType.Name,
                    ManagerName = $"{request.Commitment.SupervisorContact.FirstName} {request.Commitment.SupervisorContact.LastName}",
                    ManagerEmail = request.Commitment.SupervisorContact.Email,
                    ManagerPhone = request.Commitment.SupervisorContact.Phone
                });
            }
            else
            {
                //To Student
                _ = await _emailer.SendEmailWithTemplateAsync(request.Commitment.Student.Email, "NonExec_ToStudent_Commitment_PO_Approved", new EmailTemplateModel()
                {
                    StudentFullName = $"{request.Commitment.Student.FirstName} {request.Commitment.Student.LastName}"

                });

                //To PO 
                _ = await _emailer.SendEmailWithTemplateAsync(toEmail, "NonExec_ToPO_Approved_MatchReport", new EmailTemplateModel()
                {
                    StudentName = $"{request.Commitment.Student.FirstName} {request.Commitment.Student.LastName}",
                    StudentGraduationDate = request.Commitment.Student.StudentInstitutionFundings.FirstOrDefault().ExpectedGradDate.HasValue ? $"{request.Commitment.Student.StudentInstitutionFundings.FirstOrDefault().ExpectedGradDate.Value.Month}/{request.Commitment.Student.StudentInstitutionFundings.FirstOrDefault().ExpectedGradDate.Value.Year}" : "",
                    StudentInstitution = request.Commitment.Student.StudentInstitutionFundings.FirstOrDefault().Institution.Name,
                    AgencyName = agency,
                    SubAgencyName = subAgency,
                    CommitmentType = request.Commitment.CommitmentType.Name,
                    AgencyType = request.Commitment.Agency.AgencyType.Name,
                    JobTitle = request.Commitment.JobTitle,
                    StudentEntryOnDuty = request.Commitment.StartDate.HasValue ? request.Commitment.StartDate.Value.ToShortDateString() : "",
                    TypeOfMatch = request.Commitment.CommitmentType.Name,
                    ManagerName = $"{request.Commitment.SupervisorContact.FirstName} {request.Commitment.SupervisorContact.LastName}",
                    ManagerEmail = request.Commitment.SupervisorContact.Email,
                    ManagerPhone = request.Commitment.SupervisorContact.Phone

                });

                //To PI
                _ = await _emailer.SendEmailWithTemplateAsync(toEmail, "NonExec_ToPI_PO_Approves", new EmailTemplateModel()
                {
                    StudentName = $"{request.Commitment.Student.FirstName} {request.Commitment.Student.LastName}",
                    StudentGraduationDate = request.Commitment.Student.StudentInstitutionFundings.FirstOrDefault().ExpectedGradDate.HasValue ? $"{request.Commitment.Student.StudentInstitutionFundings.FirstOrDefault().ExpectedGradDate.Value.Month}/{request.Commitment.Student.StudentInstitutionFundings.FirstOrDefault().ExpectedGradDate.Value.Year}" : "",
                    StudentInstitution = request.Commitment.Student.StudentInstitutionFundings.FirstOrDefault().Institution.Name,
                    AgencyName = agency,
                    SubAgencyName = subAgency,
                    CommitmentType = request.Commitment.CommitmentType.Name,
                    AgencyType = request.Commitment.Agency.AgencyType.Name,
                    JobTitle = request.Commitment.JobTitle,
                    StudentEntryOnDuty = request.Commitment.StartDate.HasValue ? request.Commitment.StartDate.Value.ToShortDateString() : "",
                    TypeOfMatch = request.Commitment.CommitmentType.Name,
                    ManagerName = $"{request.Commitment.SupervisorContact.FirstName} {request.Commitment.SupervisorContact.LastName}",
                    ManagerEmail = request.Commitment.SupervisorContact.Email,
                    ManagerPhone = request.Commitment.SupervisorContact.Phone

                });
            }
        }

        public async Task SendEmailWhenPORejectsAsync(CommitmentNotificationRequest request)
        {
            var GlobalConfigSettings = await _refRepo.GetGlobalConfigurationsAsync();
            var agencyList = await _cache.GetAgenciesAsync();
            string agency = request.Commitment.Agency.Name;
            string subAgency = "N/A";
            if (request.Commitment.Agency.ParentAgencyId.HasValue && request.Commitment.Agency.ParentAgencyId.Value > 0)
            {
                agency = agencyList.Where(m => m.AgencyId == request.Commitment.Agency.ParentAgencyId).Select(m => m.Name).FirstOrDefault();
                subAgency = request.Commitment.Agency.Name;
            }

            if (request.Commitment.CommitmentType.Code == CommitmentProcessConst.CommitmentTypeInternship || request.Commitment.CommitmentType.Code == CommitmentProcessConst.CommitmentTypePostGrad)
            {
                //To Student
                _ = await _emailer.SendEmailWithTemplateAsync(request.Commitment.Student.Email, "FedExec_ToStudent_Commitment_PO_Rejected", new EmailTemplateModel()
                {
                    StudentFullName = $"{request.Commitment.Student.FirstName} {request.Commitment.Student.LastName}",
                    AgencyName = agency,
                    SubAgency = subAgency,
                    AgencyType = request.Commitment.Agency.AgencyType.Name,
                    CommitmentType = request.Commitment.CommitmentType.Name,
                    JobTitle = request.Commitment.JobTitle,
                    EntryOnDuty = request.Commitment.StartDate.HasValue ? request.Commitment.StartDate.Value.ToShortDateString() : "",
                });

                //To PO
                string toEmail = GlobalConfigSettings.Where(m => m.Key == "ProgramOfficeURI" && m.Type == "EmailSettings").Select(m => m.Value).FirstOrDefault();
                _ = await _emailer.SendEmailWithTemplateAsync(toEmail, "FedExec_ToPO_Rejected", new EmailTemplateModel()
                {
                    StudentFullName = $"{request.Commitment.Student.FirstName} {request.Commitment.Student.LastName}",
                    StudentGraduationDate = request.Commitment.Student.StudentInstitutionFundings.FirstOrDefault().ExpectedGradDate.HasValue ? $"{request.Commitment.Student.StudentInstitutionFundings.FirstOrDefault().ExpectedGradDate.Value.Month}/{request.Commitment.Student.StudentInstitutionFundings.FirstOrDefault().ExpectedGradDate.Value.Year}" : "",
                    StudentInstitution = request.Commitment.Student.StudentInstitutionFundings.FirstOrDefault().Institution.Name,
                    AgencyName = agency,
                    SubAgency = subAgency,
                    CommitmentType = request.Commitment.CommitmentType.Name,
                    AgencyType = request.Commitment.Agency.AgencyType.Name,
                    JobTitle = request.Commitment.JobTitle,
                    StudentEntryOnDuty = request.Commitment.StartDate.HasValue ? request.Commitment.StartDate.Value.ToShortDateString() : "",
                    TypeOfMatch = request.Commitment.CommitmentType.Name,
                    ManagerName = $"{request.Commitment.SupervisorContact.FirstName} {request.Commitment.SupervisorContact.LastName}",
                    ManagerEmail = request.Commitment.SupervisorContact.Email,
                    ManagerPhone = request.Commitment.SupervisorContact.Phone

                });
            }
            else
            {
                //To Student
                _ = await _emailer.SendEmailWithTemplateAsync(request.Commitment.Student.Email, "NonExec_ToStudent_Commitment_PO_Rejected", new EmailTemplateModel()
                {
                    StudentFullName = $"{request.Commitment.Student.FirstName} {request.Commitment.Student.LastName}",
                    AgencyName = agency,
                    SubAgency = subAgency,
                    AgencyType = request.Commitment.Agency.AgencyType.Name,
                    CommitmentType = request.Commitment.CommitmentType.Name,
                    JobTitle = request.Commitment.JobTitle,
                    Justification = request.Commitment.Justification
                });

                //To PO
                string toEmail = GlobalConfigSettings.Where(m => m.Key == "ProgramOfficeURI" && m.Type == "EmailSettings").Select(m => m.Value).FirstOrDefault();
                _ = await _emailer.SendEmailWithTemplateAsync(toEmail, "NonExec_ToPO_Rejected", new EmailTemplateModel()
                {
                    StudentFullName = $"{request.Commitment.Student.FirstName} {request.Commitment.Student.LastName}",
                    StudentGraduationDate = request.Commitment.Student.StudentInstitutionFundings.FirstOrDefault().ExpectedGradDate.HasValue ? $"{request.Commitment.Student.StudentInstitutionFundings.FirstOrDefault().ExpectedGradDate.Value.Month}/{request.Commitment.Student.StudentInstitutionFundings.FirstOrDefault().ExpectedGradDate.Value.Year}" : "",
                    StudentInstitution = request.Commitment.Student.StudentInstitutionFundings.FirstOrDefault().Institution.Name,
                    AgencyName = agency,
                    SubAgency = subAgency,
                    CommitmentType = request.Commitment.CommitmentType.Name,
                    AgencyType = request.Commitment.Agency.AgencyType.Name,
                    JobTitle = request.Commitment.JobTitle,
                    StudentEntryOnDuty = request.Commitment.StartDate.HasValue ? request.Commitment.StartDate.Value.ToShortDateString() : "",
                    Justification = request.Commitment.Justification,
                    PIRecommendation = request.Commitment.PIRecommendation == "PIReject" ? "Reject" : "Approve"
                });
            }
        }

        public async Task SendEmailWhenPORequestFinalDocsAsync(CommitmentNotificationRequest request)
        {
            //To Student
            var agencyList = await _cache.GetAgenciesAsync();
            string agency = request.Commitment.Agency.Name;
            string subAgency = "N/A";
            if (request.Commitment.Agency.ParentAgencyId.HasValue && request.Commitment.Agency.ParentAgencyId.Value > 0)
            {
                agency = agencyList.Where(m => m.AgencyId == request.Commitment.Agency.ParentAgencyId).Select(m => m.Name).FirstOrDefault();
                subAgency = request.Commitment.Agency.Name;
            }
            _ = await _emailer.SendEmailWithTemplateAsync(request.Commitment.Student.Email, "NonExec_ToStudent_Commitment_PO_Request_PD_FOL", new EmailTemplateModel()
            {
                StudentFullName = $"{request.Commitment.Student.FirstName} {request.Commitment.Student.LastName}",
                StudentEmail = request.Commitment.Student.Email,
                StudentInstitution = request.Commitment.Student.StudentInstitutionFundings.FirstOrDefault().Institution.Name,
                AgencyName = agency,
                SubAgency = subAgency,
                AgencyType = request.Commitment.Agency.AgencyType.Name,
                CommitmentType = request.Commitment.CommitmentType.Name,
                JobTitle = request.Commitment.JobTitle,
                Justification = request.Commitment.Justification
            });
        }

        public async Task SendEmailWhenStudentUploadEVFDocument(List<CommitmentNotificationRequest> request)
        {
            var GlobalConfigSettings = await _refRepo.GetGlobalConfigurationsAsync();
            string toEmail = GlobalConfigSettings.Where(m => m.Key == "ProgramOfficeURI" && m.Type == "EmailSettings").Select(m => m.Value).FirstOrDefault();
            var agencyList = await _cache.GetAgenciesAsync();
            string agency = "";
            string subAgency = "N/A";
            StringBuilder emailContent = new StringBuilder();

            emailContent.Append($@"The following student has submitted an employment verification for the commitment below.  Please log in to the admin site to review.<br/><br/> 
                                    <b>Student Information:</b> <br/><br/>
                                    Name:  {request.FirstOrDefault().Commitment.Student.FirstName} {request.FirstOrDefault().Commitment.Student.LastName} <br/><br/>
                                    School: {request.FirstOrDefault().Commitment.Student.StudentInstitutionFundings.FirstOrDefault().Institution.Name} <br/><br/>
                                    Graduation Date: {request.FirstOrDefault().Commitment.Student.StudentInstitutionFundings.FirstOrDefault().ExpectedGradDate.Value.ToShortDateString()} <br/><br/>
                                    Type of Match: {request.FirstOrDefault().Commitment.CommitmentType.Name} <br/><br/> 
                                    <b>Agency Information:</b>");

            foreach (var pg in request)
            {
                agency = pg.Commitment.Agency.Name.ToString();
                subAgency = "N/A";
                if (pg.Commitment.Agency.ParentAgencyId.HasValue && pg.Commitment.Agency.ParentAgencyId.Value > 0)
                {
                    agency = agencyList.Where(m => m.AgencyId == pg.Commitment.Agency.ParentAgencyId).Select(m => m.Name).FirstOrDefault();
                    subAgency = pg.Commitment.Agency.Name.ToString();
                }
                emailContent.Append($@"<br/><br/> Agency: {agency} <br/><br/>
                                     Sub Agency: {subAgency} <br/><br/>
                                     Agency Type: {pg.Commitment.Agency.AgencyType.Name} <br/><br/>
                                     Entry On Duty: {pg.Commitment.StartDate.Value.ToShortDateString()}");



            }
            emailContent.Append(@"<br/><br/><br/>***This notification is system generated. Please do not reply***");
            _ = await _emailer.SendEmailDefaultTemplateAsync(toEmail, "Employment Verification", emailContent.ToString());

        }
    }

    public class CommitmentNotificationRequest
    {
        public StudentCommitment Commitment { get; set; }
        public int InstitutionID { get; set; }
        public string Status { get; set; }
    }
}
