using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OPM.SFS.Core.Shared;
using OPM.SFS.Data;
using OPM.SFS.Web.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OPM.SFS.Web.SharedCode
{
    public interface ILoginGovLinkingHelper
    {
        Task StartLinkingProcessAsync(List<Claim> allEmails, string loginGovLinkID);
    }

    public class LoginGovLinkingHelper : ILoginGovLinkingHelper
    {
        private readonly ScholarshipForServiceContext _efDB;
        private readonly IEmailerService _emailer;
        public readonly IAuditEventLogHelper _auditLogger;
        private readonly IConfiguration _appSettings;
        private readonly IUtilitiesService _utilities;

        public LoginGovLinkingHelper(ScholarshipForServiceContext db, IEmailerService emailer, IAuditEventLogHelper audit, IConfiguration appSettings, IUtilitiesService utilites)
        {
            _efDB = db;
            _emailer = emailer;
            _auditLogger = audit;
            _appSettings = appSettings;
            _utilities = utilites;
        }

        public async Task StartLinkingProcessAsync(List<Claim> allEmails, string loginGovLinkID)
        {
            string baseUrl = _appSettings["General:BaseUrl"];
            bool processedStudent = false, processedPI = false, processedAO = false, processedAdmin = false;


            processedStudent = await StartLinkingForStudentAsync(allEmails, loginGovLinkID, baseUrl);
            if (!processedStudent)
            {
                processedPI = await StartLinkingForAcademiaUsersAsync(allEmails, loginGovLinkID, baseUrl);
                if (!processedPI)
                {
                    processedAO = await StartLinkingForAgencyUsersAsync(allEmails, loginGovLinkID, baseUrl);
                    if (!processedAO)
                    {
                        processedAdmin = await StartLinkingForAdminUsersAsync(allEmails, loginGovLinkID, baseUrl);
                    }
                }
            }
            if (!processedStudent && !processedPI && !processedAO && !processedAdmin)
            {
                string email1 = allEmails.FirstOrDefault().Value;
                string email2 = allEmails.LastOrDefault().Value;
                await _auditLogger.LogAuditEvent($"Login.gov: Linking attempt for emails {email1}, {email2}. Emails not found.");
            }
        }

        private async Task<bool> StartLinkingForStudentAsync(List<Claim> allEmails, string loginGovID, string baseUrl)
        {
            string email1 = allEmails.FirstOrDefault().Value;
            string email2 = allEmails.LastOrDefault().Value;
            var validStudent = await _efDB.Students.Where(m => m.Email == email1 || m.Email == email2 || m.AlternateEmail == email1 || m.AlternateEmail
            == email2)
                            .Where(m => m.ProfileStatus.Name == "Active")
                            .FirstOrDefaultAsync();

            if (validStudent != null)
            {
                var linkExpirationDate = DateTime.UtcNow.AddMinutes(30);
                var linkExpirationDisplayEST = _utilities.ConvertUtcToEastern(linkExpirationDate).ToString("MM/dd/yyyy h:mm tt");
                LoginGovStaging stageAccountLinking = new();
                stageAccountLinking.LoginGovLinkID = loginGovID;
                stageAccountLinking.AccountID = validStudent.StudentId;
                stageAccountLinking.AccountType = "ST";
                stageAccountLinking.DateInserted = DateTime.UtcNow;
                stageAccountLinking.ExpirationDate = linkExpirationDate;
                _efDB.LoginGovStaging.Add(stageAccountLinking);
                await _efDB.SaveChangesAsync();
                var secureLink = $"{baseUrl}/ActivateLogin?s={stageAccountLinking.LoginGovStagingID}";

                //Our records indicate that this is your first time using login.gov with SFS. Please click on the 
                //following link to confirm and proceed with linking your SFS account with Login.gov
                string emailContent = $@"Hello {validStudent.FirstName}, <br /><br /> 
                                            You will need to link your Login.gov account to your SFS profile. Please click on the
                                            following link to confirm and proceed with linking your SFS account with Login.gov {secureLink} <br/>
                                            This link will expire on {linkExpirationDisplayEST} <br /><br />
                                            If you did not request this action or need additional assistance, please send an email to sfs@opm.gov. <br /><br />
                                            Sincerely, <br /><br />
                                            The SFS Help Desk";

                await _emailer.SendEmailDefaultTemplateAsync(email1, "SFS Confirm Login.gov", emailContent);
                //_logger.LogInformation($"User {validStudent.StudentId.ToString()} has started Login.gov / SFS linking process.");
                await _auditLogger.LogAuditEvent($"Login.gov: User {validStudent.StudentId} Role ST generated a one-time url for linking.");
                return true;
            }
            return false;
        }

        private async Task<bool> StartLinkingForAcademiaUsersAsync(List<Claim> allEmails, string loginGovID, string baseUrl)
        {
            string email1 = allEmails.FirstOrDefault().Value;
            string email2 = allEmails.LastOrDefault().Value;
            var validAcademiaUser = await _efDB.AcademiaUsers.Where(m => m.Email == email1 || m.Email == email2)
                            .Where(m => m.ProfileStatus.Name == "Active")
                            .FirstOrDefaultAsync();

            if (validAcademiaUser != null)
            {
				var linkExpirationDate = DateTime.UtcNow.AddMinutes(30);
				var linkExpirationDisplayEST = _utilities.ConvertUtcToEastern(linkExpirationDate).ToString("MM/dd/yyyy h:mm tt");				
                LoginGovStaging stageAccountLinking = new();
                stageAccountLinking.LoginGovLinkID = loginGovID;
                stageAccountLinking.AccountID = validAcademiaUser.AcademiaUserId;
                stageAccountLinking.AccountType = "PI";
                stageAccountLinking.DateInserted = DateTime.UtcNow;
                stageAccountLinking.ExpirationDate = linkExpirationDate;
                _efDB.LoginGovStaging.Add(stageAccountLinking);
                await _efDB.SaveChangesAsync();
                var secureLink = $"{baseUrl}/ActivateLogin?s={stageAccountLinking.LoginGovStagingID}";

                //Our records indicate that this is your first time using login.gov with SFS. Please click on the 
                //following link to confirm and proceed with linking your SFS account with Login.gov
                string emailContent = $@"Hello {validAcademiaUser.Firstname}, <br /><br /> 
                                            Our records indicate that this is your first time using login.gov with SFS. Please click on the
                                            following link to confirm and proceed with linking your SFS account with Login.gov {secureLink} <br/>
                                            This link will expire on {linkExpirationDisplayEST} <br /><br />
                                            If you did not request this action or need additional assistance, please send an email to sfs@opm.gov. <br /><br />
                                            Sincerely, <br /><br />
                                            The SFS Help Desk";

                await _emailer.SendEmailDefaultTemplateAsync(validAcademiaUser.Email, "SFS Confirm Login.gov", emailContent);
                //_logger.LogInformation($"User {validAcademiaUser.AcademiaUserId.ToString()} has started Login.gov / SFS linking process.");
                await _auditLogger.LogAuditEvent($"Login.gov: User {validAcademiaUser.AcademiaUserId} Role PI generated a one-time url for linking.");
                return true;
            }
            return false;
        }

        private async Task<bool> StartLinkingForAgencyUsersAsync(List<Claim> allEmails, string loginGovID, string baseUrl)
        {
            string email1 = allEmails.FirstOrDefault().Value;
            string email2 = allEmails.LastOrDefault().Value;
            var validAgencyUser = await _efDB.AgencyUsers.Where(m => m.Email == email1 || m.Email == email2)
                            .Where(m => m.ProfileStatus.Name == "Active")
                            .FirstOrDefaultAsync();

            if (validAgencyUser != null)
            {
                
				var linkExpirationDate = DateTime.UtcNow.AddMinutes(30);
				var linkExpirationDisplayEST = _utilities.ConvertUtcToEastern(linkExpirationDate).ToString("MM/dd/yyyy h:mm tt");
				LoginGovStaging stageAccountLinking = new();
                stageAccountLinking.LoginGovLinkID = loginGovID;
                stageAccountLinking.AccountID = validAgencyUser.AgencyUserId;
                stageAccountLinking.AccountType = "AO";
                stageAccountLinking.DateInserted = DateTime.Now;
                stageAccountLinking.ExpirationDate = linkExpirationDate;
                _efDB.LoginGovStaging.Add(stageAccountLinking);
                await _efDB.SaveChangesAsync();
                var secureLink = $"{baseUrl}/ActivateLogin?s={stageAccountLinking.LoginGovStagingID}";

                //Our records indicate that this is your first time using login.gov with SFS. Please click on the 
                //following link to confirm and proceed with linking your SFS account with Login.gov
                string emailContent = $@"Hello {validAgencyUser.Firstname}, <br /><br /> 
                                            Our records indicate that this is your first time using login.gov with SFS. Please click on the
                                            following link to confirm and proceed with linking your SFS account with Login.gov {secureLink} <br/>
                                            This link will expire on {linkExpirationDisplayEST} <br /><br />
                                            If you did not request this action or need additional assistance, please send an email to sfs@opm.gov. <br /><br />
                                            Sincerely, <br /><br />
                                            The SFS Help Desk";

                await _emailer.SendEmailDefaultTemplateAsync(validAgencyUser.Email, "SFS Confirm Login.gov", emailContent);
                //_logger.LogInformation($"User {validAgencyUser.AgencyUserId.ToString()} has started Login.gov / SFS linking process.");
                await _auditLogger.LogAuditEvent($"Login.gov: User {validAgencyUser.AgencyUserId} Role AO generated a one-time url for linking.");
                return true;
            }
            return false;
        }

        private async Task<bool> StartLinkingForAdminUsersAsync(List<Claim> allEmails, string loginGovID, string baseUrl)
        {
            string email1 = allEmails.FirstOrDefault().Value;
            string email2 = allEmails.LastOrDefault().Value;
            var validAdminUser = await _efDB.AdminUsers.Where(m => m.Email == email1 || m.Email == email2)
                            .Where(m => m.IsDisabled != true)
                            .FirstOrDefaultAsync();

            if (validAdminUser != null)
            {
                
				var linkExpirationDate = DateTime.UtcNow.AddMinutes(30);
				var linkExpirationDisplayEST = _utilities.ConvertUtcToEastern(linkExpirationDate).ToString("MM/dd/yyyy h:mm tt");
				LoginGovStaging stageAccountLinking = new();
                stageAccountLinking.LoginGovLinkID = loginGovID;
                stageAccountLinking.AccountID = validAdminUser.AdminUserId;
                stageAccountLinking.AccountType = "AD";
                stageAccountLinking.DateInserted = DateTime.Now;
                stageAccountLinking.ExpirationDate = linkExpirationDate;
                _efDB.LoginGovStaging.Add(stageAccountLinking);
                await _efDB.SaveChangesAsync();
                var secureLink = $"{baseUrl}/ActivateLogin?s={stageAccountLinking.LoginGovStagingID}";

                //Our records indicate that this is your first time using login.gov with SFS. Please click on the 
                //following link to confirm and proceed with linking your SFS account with Login.gov
                string emailContent = $@"Hello {validAdminUser.FirstName}, <br /><br /> 
                                            Our records indicate that this is your first time using login.gov with SFS. Please click on the
                                            following link to confirm and proceed with linking your SFS account with Login.gov {secureLink} <br/>
                                            This link will expire on {linkExpirationDisplayEST} <br /><br />
                                            If you did not request this action or need additional assistance, please send an email to sfs@opm.gov. <br /><br />
                                            Sincerely, <br /><br />
                                            The SFS Help Desk";

                await _emailer.SendEmailDefaultTemplateAsync(validAdminUser.Email, "SFS Confirm Login.gov", emailContent);
                //_logger.LogInformation($"User {validAdminUser.AdminUserId.ToString()} has started Login.gov / SFS linking process.");
                await _auditLogger.LogAuditEvent($"Login.gov: User {validAdminUser.AdminUserId} Role AD generated a one-time url for linking.");
                return true;
            }
            return false;
        }
    }
}
