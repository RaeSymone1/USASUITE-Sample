using OPM.SFS.Data;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using OPM.SFS.Web.Shared;
using Microsoft.Extensions.Configuration;
using Azure.Communication.Email;
using System.Net.Mail;
using OPM.SFS.Core.Shared;

namespace OPM.SFS.Web.SharedCode
{
    public interface IEmploymentVerificationEmailService
    {
        Task SendAllEVFEmails();
    }

    public class EmploymentVerificationEmailService : IEmploymentVerificationEmailService
    {
        private readonly ScholarshipForServiceContext _db;
        private readonly IEmailerService _emailer;
        public EmploymentVerificationEmailService(ScholarshipForServiceContext db, IEmailerService emailer)
        {
            _db = db;
            _emailer = emailer;
        }
        public async Task SendAllEVFEmails()
        {
            await SendPGVerificationDueDateEmailsAsync();
            await SendSOCVerficationDueDateReminderEmailsAsync();
        }
        private async Task<bool> SendPGVerificationDueDateEmailsAsync()
        {
            string evfFormUrl = "https://sfs.opm.gov/docs/EMPLOYMENT%20VERIFICATION%20FORM%20v2.pdf";
            var studentsToEmailPG = await _db.StudentInstitutionFundings
                .Where(m => m.PGVerificationOneDueDate.Value.Date == DateTime.UtcNow.Date || m.PGVerificationTwoDueDate.Value.Date == DateTime.UtcNow.Date)
                .Select(m => new EVFEmailServiceDTO()
                {
                    StudentId = m.Student.StudentId,
                    Firstname = m.Student.FirstName,
                    Lastname = m.Student.LastName,
                    Email = m.Student.Email
                })
                .ToListAsync();

            studentsToEmailPG = studentsToEmailPG.DistinctBy(m => m.StudentId).ToList();
            foreach (var s in studentsToEmailPG)
            {
                string emailContent = $@"Good day {s.Firstname} {s.Lastname}, <br/><br/>
                                   As a condition of receiving a SFS scholarship, you are required to provide annual verifiable documentation of post-award employment 
                                    and up-to-date contact information <b>no later than {DateTime.UtcNow.AddDays(14).ToShortDateString()}</b>.<br/><br/>
                                   Log in to the SFS system and navigate to your profile to confirm and update the following sections:<br/>
                                    <ul>
                                      <li>Name</li>
                                      <li>Primary email address</li>
                                      <li>Alternate email address</li>
                                      <li>Current mailing address</li>
                                      <li>Permanent mailing address</li>
                                      <li>Emergency contact information</li>
                                    </ul><br/>									
                                    Navigate to the resume/documents section to upload your employment verification documentation.<br/><br/>
                                    Employment verification must include dates of employment. The following are the accepted forms of documentation: <br/>
                                    <ol>
                                      <li>The <a href='{evfFormUrl}'>SFS Employment Verification Form</a> completed by your employing organization.</li>
                                      <li>An employment verification document from your employing organization.</li>
                                      <li>If applicable*, your appointment or first SF-50 AND your most recent SF-50.</li>
                                    </ol>                                   
									<b>NOTE: If you send a SF-50(s), please redact your date of birth and social security number.</b><br/>
                                    <small><i>SF50s are for Federal government employees only.</i></small><br/><br/>
                                    If your employment was/is with an Intelligence agency, we understand the sensitivity; however, you are required to provide the 
                                    SFS Program Office documentation from your agency verifying employment. If you cannot, or are unsure how, please contact us for guidance at 
                                    <a href='mailto:sfs@opm.gov'>sfs@opm.gov</a> <br/><br/>
                                    For questions or assistance, contact the SFS Program Office.";

                await _emailer.SendEmailDefaultTemplateAsync(s.Email, $@"Required: Annual SFS Employment Verification", emailContent);
            }
            return true;
        }

        private async Task<bool> SendSOCVerficationDueDateReminderEmailsAsync()
        {
           var usersToEmail = await _db.Students.Where(m => m.StudentInstitutionFundings.FirstOrDefault().CommitmentPhaseComplete.Value.Date == DateTime.UtcNow.Date)
             .Select(m => new EVFEmailServiceDTO()
             {
                 Email = m.Email,
                 Firstname = m.FirstName,
                 Lastname = m.LastName,
                 SOCVerificationDueDate = m.StudentInstitutionFundings.FirstOrDefault().CommitmentPhaseComplete

             }).ToListAsync();
            foreach (var a in usersToEmail)
            {

                var FinalDueTime = a.SOCVerificationDueDate.Value.AddDays(14);
                var FinalDueDate = FinalDueTime.ToString("MMMM dd, yyyy");
                string emailContent = $@"Hello {a.Firstname} {a.Lastname}, <br/><br/>
								   To process your official completion of the Scholarship for Service (SFS) program, we require verification that you have met your service obligation. 
                                   Verification must include dates of employment and there are three accepted forms of documentation:<br/><br/>
                                    <ul>
                                      <li>The attached Employment Verification Form completed by your employing organization’s Human Resources Department</li>
                                      <li>An Employment Verification Form used by your employing organization</li>
                                      <li>If applicable*, your most recent SF-50, which shows your Service Computation Date and the current date of the SF-50 action, and your starting SF-50. NOTE: IF YOU SEND A SF-50(s), PLEASE REDACT YOUR DATE OF BIRTH AND SOCIAL SECURITY NUMBER</li>
                                    </ul><br/>	
									*SF50’s are for Federal government employees only<br/><br/>
									<b>Submission of the verification document(s) must be uploaded in the resume/documents section of the SFS systems student portal <a href=""https://sfs.opm.gov/Student/Login"">SFS systems student portal</a></b><br/><br/>
                                    If your employment was/is with an Intelligence agency, we understand the sensitivity; however, you are required to provide the SFS Program Office documentation from your agency verifying employment. If you cannot, or are unsure how, please contact us for guidance at sfs@opm.gov.									
                                    If you have any questions, contact your institution’s Scholarship for Service Program Principal Investigator (PI) or the SFS Program Office.<br/><br/>
                                    <b>Please provide your employment verification documentation as soon as possible but no later than: {FinalDueDate}</b><br/><br/>
                                    The SFS Program Office and the institution have an obligation to confirm that students awarded the SFS scholarship completed their employment obligation as required. 
                                    You will continue to receive an email from our office every 3-4 weeks requesting the employment verification documentation until it is submitted.<br/>        
                                    If you have questions or need additional assistance, please contact us at sfs@opm.gov.";
                await _emailer.SendEmailDefaultTemplateAsync(a.Email, $@"Employment Verification required to confirm Service Obligation Completion - Please take action by: {FinalDueDate}", emailContent);
            }

            return true;
        }
        
    }

    public class EVFEmailServiceDTO
    {
        public int StudentId { get; set; }
        public string Email { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public DateTime? SOCVerificationDueDate { get; internal set; }
    }

}
