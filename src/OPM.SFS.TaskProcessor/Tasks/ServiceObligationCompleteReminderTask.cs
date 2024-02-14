using Medallion.Threading.SqlServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OPM.SFS.Core.Shared;
using OPM.SFS.Data;
using Sgbj.Cron;

namespace OPM.SFS.TaskProcessor.Tasks
{
    public class ServiceObligationCompleteReminderTask : BackgroundService
    {
        public readonly ScholarshipForServiceContext _efDB;
        private readonly IEmailerService _emailer;
        private readonly ILogger<PostgradVerificationDueReminderTask> _logger;
        private readonly IConfiguration _appSettings;
        private readonly IUtilitiesService _utilities;

        public ServiceObligationCompleteReminderTask(IEmailerService emailer, ILogger<PostgradVerificationDueReminderTask> logger, IConfiguration appSettings, IServiceProvider serviceProvider, IUtilitiesService utilities)
        {
            _efDB = serviceProvider.CreateScope().ServiceProvider.GetRequiredService<ScholarshipForServiceContext>(); 
            _emailer = emailer;
            _logger = logger;
            _appSettings = appSettings;
            _utilities = utilities;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var task = await _efDB.ScheduledTask.Where(m => m.Name == "ServiceObligationCompleteReminderTask").FirstOrDefaultAsync();
            if (!task.IsDisabled)
            {
                using var timer = new CronTimer(task.Schedule, TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time"));
                while (await timer.WaitForNextTickAsync())
                {
                    var @lock = new SqlDistributedLock("ServiceObligationCompleteReminderTask", _efDB.Database.GetConnectionString());
                    await using (await @lock.AcquireAsync())
                    {
                        try
                        {
                            _logger.LogInformation("ScheduledTask ServiceObligationCompleteReminderTask running");
                            await SetTaskStateAsync("RUNNING");
                            await SendSOCVerficationDueDateReminderEmailsAsync();
                            await SetTaskStateAsync("COMPLETE");
                            _logger.LogInformation("Scheduled Task ServiceObligationCompleteReminderTask completed");
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError($"ScheduledTask ServiceObligationCompleteReminderTask failed with error {ex.Message}");
                        }
                    }
                }
            }
        }

        private async Task<bool> SendSOCVerficationDueDateReminderEmailsAsync()
        {
            string baseUrl = _appSettings["General:BaseUrl"];
            var usersToEmail = await _efDB.StudentInstitutionFundings.Where(m => m.CommitmentPhaseComplete.Value.Date == _utilities.ConvertUtcToEastern(DateTime.Now).Date)
                .Where(m => m.ServiceObligationCompleteReminderSentDate == null)
              .Select(m => new 
              {
                  StudentFundingID = m.StudentInstitutionFundingId,
                  Email = m.Student.Email,
                  Firstname = m.Student.FirstName,
                  Lastname = m.Student.LastName,
                  SOCVerificationDueDate = m.CommitmentPhaseComplete
              }).ToListAsync();
            List<int> studentIDList = usersToEmail.Select(m => m.StudentFundingID).ToList();
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
									<b>Submission of the verification document(s) must be uploaded in the resume/documents section of the SFS systems student portal <a href=""{baseUrl}/Student/Login"">SFS systems student portal</a></b><br/><br/>
                                    If your employment was/is with an Intelligence agency, we understand the sensitivity; however, you are required to provide the SFS Program Office documentation from your agency verifying employment. If you cannot, or are unsure how, please contact us for guidance at sfs@opm.gov.									
                                    If you have any questions, contact your institution’s Scholarship for Service Program Principal Investigator (PI) or the SFS Program Office.<br/><br/>
                                    <b>Please provide your employment verification documentation as soon as possible but no later than: {FinalDueDate}</b><br/><br/>
                                    The SFS Program Office and the institution have an obligation to confirm that students awarded the SFS scholarship completed their employment obligation as required. 
                                    You will continue to receive an email from our office every 3-4 weeks requesting the employment verification documentation until it is submitted.<br/>        
                                    If you have questions or need additional assistance, please contact us at sfs@opm.gov.";
                await _emailer.SendEmailDefaultTemplateAsync(a.Email, $@"Employment Verification required to confirm Service Obligation Completion - Please take action by: {FinalDueDate}", emailContent);
            }
            await UpdateServiceObligationCompleteReminderSentDate(studentIDList);
            return true;
        }

        private async Task SetTaskStateAsync(string state)
        {
            var task = await _efDB.ScheduledTask.Where(m => m.Name == "ServiceObligationCompleteReminderTask").FirstOrDefaultAsync();
            task.LastRunDate = DateTime.UtcNow;
            task.State = state;
            await _efDB.SaveChangesAsync();
        }

        private async Task UpdateServiceObligationCompleteReminderSentDate(List<int> students)
        {
            foreach (var s in students)
            {
                var toUpdate = await _efDB.StudentInstitutionFundings.Where(m => m.StudentInstitutionFundingId == s).FirstOrDefaultAsync();
                toUpdate.ServiceObligationCompleteReminderSentDate = DateTime.UtcNow;
            }
            await _efDB.SaveChangesAsync();
        }
    }
}
