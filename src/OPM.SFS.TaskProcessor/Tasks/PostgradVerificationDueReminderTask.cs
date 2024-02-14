using Medallion.Threading.SqlServer;
using Microsoft.EntityFrameworkCore;
using OPM.SFS.Core.Shared;
using OPM.SFS.Data;
using Sgbj.Cron;

namespace OPM.SFS.TaskProcessor.Tasks
{
    public class PostgradVerificationDueReminderTask : BackgroundService
    {
        public readonly ScholarshipForServiceContext _efDB;
        private readonly IEmailerService _emailer;
        private readonly ILogger<PostgradVerificationDueReminderTask> _logger;
        private readonly IConfiguration _appSettings;
        private readonly IUtilitiesService _utilites;

        public PostgradVerificationDueReminderTask(IEmailerService emailer, ILogger<PostgradVerificationDueReminderTask> logger, IConfiguration appSettings, IServiceProvider serviceProvider, IUtilitiesService utilities)
        {
            _efDB = serviceProvider.CreateScope().ServiceProvider.GetRequiredService<ScholarshipForServiceContext>(); 
            _emailer = emailer;
            _logger = logger;
            _appSettings = appSettings;
            _utilites = utilities;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var task = await _efDB.ScheduledTask.Where(m => m.Name == "PostgradVerificationDueReminderTask").FirstOrDefaultAsync();
            if (!task.IsDisabled)
            {
                using var timer = new CronTimer(task.Schedule, TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time"));
                while (await timer.WaitForNextTickAsync())
                {
                    var @lock = new SqlDistributedLock("PostgradVerificationDueReminderTask", _efDB.Database.GetConnectionString());
                    await using (await @lock.AcquireAsync())
                    {
                        try
                        {
                            _logger.LogInformation("ScheduledTask PostgradVerificationDueReminderTask running");
                            await SetTaskStateAsync("RUNNING");
                            await SendPGVerificationDueDateEmailsAsync();
                            await SetTaskStateAsync("COMPLETE");
                            _logger.LogInformation("Scheduled Task PostgradVerificationDueReminderTask completed");
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError($"ScheduledTask PostgradVerificationDueReminderTask failed with error {ex.Message}");
                        }
                    }
                }
            }
        }

        private async Task<bool> SendPGVerificationDueDateEmailsAsync()
        {
            string baseUrl = _appSettings["General:BaseUrl"];
            string evfFormUrl = $"{baseUrl}/docs/EMPLOYMENT%20VERIFICATION%20FORM%20v2.pdf";
            var studentsToEmailPG = await _efDB.StudentInstitutionFundings
                .Where(m => m.PGVerificationOneDueDate.Value.Date == _utilites.ConvertUtcToEastern(DateTime.UtcNow).Date || m.PGVerificationTwoDueDate.Value.Date == _utilites.ConvertUtcToEastern(DateTime.UtcNow).Date)
                .Where(m => m.PostGradVerificationReminderSentDate == null)
                .Select(m => new
                {
                    StudentFundingID = m.StudentInstitutionFundingId,
                    Firstname = m.Student.FirstName,
                    Lastname = m.Student.LastName,
                    Email = m.Student.Email
                })
                .ToListAsync();
            List<int> studentIDList = studentsToEmailPG.Select(m => m.StudentFundingID).ToList();
            foreach (var s in studentsToEmailPG)
            {
                string emailContent = $@"Good day {s.Firstname} {s.Lastname}, <br/><br/>
                                   As a condition of receiving a SFS scholarship, you are required to provide annual verifiable documentation of post-award employment 
                                    and up-to-date contact information <b>no later than {ConvertUtcToEastern(DateTime.UtcNow).AddDays(14).ToShortDateString()}</b>.<br/><br/>
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

                await _emailer.SendEmailDefaultTemplateAsync(s.Email, "Required: Annual SFS Employment Verification", emailContent);
            }
            await UpdatePostgradVerifyReminderSentDate(studentIDList);
            return true;
        }

        private async Task SetTaskStateAsync(string state)
        {
            var task = await _efDB.ScheduledTask.Where(m => m.Name == "PostgradVerificationDueReminderTask").FirstOrDefaultAsync();
            task.LastRunDate = DateTime.UtcNow;
            task.State = state;
            await _efDB.SaveChangesAsync();
        }

        private async Task UpdatePostgradVerifyReminderSentDate(List<int> students)
        {
            foreach(var s in students)
            {
                var toUpdate = await _efDB.StudentInstitutionFundings.Where(m => m.StudentInstitutionFundingId == s).FirstOrDefaultAsync();
                toUpdate.PostGradVerificationReminderSentDate = DateTime.UtcNow;                
            }
            await _efDB.SaveChangesAsync();
        }

        private DateTime ConvertUtcToEastern(DateTime utcDateTime)
        {
            TimeZoneInfo easternTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
            if (easternTimeZone.IsDaylightSavingTime(utcDateTime))
            {
                easternTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Daylight Time");
            }
            DateTime easternTime = TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, easternTimeZone);
            return easternTime;
        }
    }
}
