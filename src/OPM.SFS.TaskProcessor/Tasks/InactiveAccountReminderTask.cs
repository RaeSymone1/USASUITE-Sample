using Medallion.Threading.SqlServer;
using Microsoft.EntityFrameworkCore;
using OPM.SFS.Core.Shared;
using OPM.SFS.Data;
using Sgbj.Cron;
using System.Linq.Expressions;

namespace OPM.SFS.TaskProcessor.Tasks
{
    /// <summary>
    /// Queue email to users who are X days away from account
    /// being set as inactive
    /// </summary>
    public class InactiveAccountReminderTask : BackgroundService
    {
        public readonly ScholarshipForServiceContext _efDB;
        private readonly IEmailerService _emailer;
        private readonly ILogger _logger;
        private readonly IConfiguration _appSettings;        

        public InactiveAccountReminderTask(IEmailerService emailer, ILogger<InactiveAccountReminderTask> logger, IServiceProvider serviceProvider, IConfiguration appSettings)
        {

            _emailer = emailer;
            _logger = logger;
            _efDB = serviceProvider.CreateScope().ServiceProvider.GetRequiredService<ScholarshipForServiceContext>();
            _appSettings = appSettings;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {            
            var task = await _efDB.ScheduledTask.Where(m => m.Name == "InactiveAccountReminderTask").FirstOrDefaultAsync();
            if(!task.IsDisabled)
            {
                using var timer = new CronTimer(task.Schedule, TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time"));
                while (await timer.WaitForNextTickAsync())
                {                   
                    var @lock = new SqlDistributedLock("InactiveAccountReminderLock", _efDB.Database.GetConnectionString());
                    await using (await @lock.AcquireAsync())
                    {
                        try
                        {
                            _logger.LogInformation("ScheduledTask InactiveAccountReminderTask running");
                            await SetTaskStateAsync("RUNNING");
                            await _efDB.SaveChangesAsync();
                            var accounts = await GetAccountsForRemindersAsync();
                            foreach (var account in accounts)
                                await SendReminderEmailAsync(account);
                            await UpdateInactiveReminderSentDate(accounts);
                            await SetTaskStateAsync("COMPLETE");
                            _logger.LogInformation("Scheduled Task InactiveAccountReminderTask completed");
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError($"ScheduledTask InactiveAccountReminderTask failed with error {ex.Message}");
                        }
                    }
                }
            }           
        }
        public async Task<bool> SendReminderEmailAsync(AccountData account)
        {

            string baseUrl = _appSettings["General:BaseUrl"];
            var loginLink = "";
            if (account.AccountType == "ST") loginLink = $"{baseUrl}/Student/Login";
            if (account.AccountType == "PI") loginLink = $"{baseUrl}/Academia/Login";
            if (account.AccountType == "AO") loginLink = $"{baseUrl}/Agency/Login";
            if (account.AccountType == "AD") loginLink = $"{baseUrl}/Admin/Login";
            
            string emailContent = $@"Hello {account.FirstName}, <br/><br/>
                                Its been awhile since you've logged into your SFS account and it will become inactive in 10 days. If you want to keep your SFS account active, sign into your account at 
                                <a href='{loginLink}'>SFS</a>";
            await _emailer.SendEmailDefaultTemplateAsync(account.Email, "SFS Account Inactive in 10 Days", emailContent);           

            return true;
        }

        public async Task<List<AccountData>> GetAccountsForRemindersAsync()
        {
            var GlobalConfigSettings = await _efDB.GlobalConfiguration.ToListAsync();            
            var activeID = _efDB.ProfileStatus.Where(m => m.Name == "Active").Select(m => m.ProfileStatusID).FirstOrDefault();
            int days = Convert.ToInt32(GlobalConfigSettings.Where(m => m.Key == "AccountExpireDays").Select(m => m.Value).FirstOrDefault());
            int reminderDays = Convert.ToInt32(GlobalConfigSettings.Where(m => m.Key == "AccountReminderDays").Select(m => m.Value).FirstOrDefault());
            days = days - reminderDays;
            var loginDateFilter = DateTime.UtcNow.AddDays(days * -1);
            DateTime minDate = new DateTime(loginDateFilter.Date.Year, loginDateFilter.Date.Month, loginDateFilter.Date.Day);
            DateTime maxDate = minDate.AddSeconds(86399);
            List<AccountData> usersToEmail = new();
           
            usersToEmail = await _efDB.AgencyUsers.Where(m => m.LastLoginDate >= minDate && m.LastLoginDate <= maxDate && m.ProfileStatusID == activeID)
                .Where(m => m.InactiveAccountReminderSentDate == null)
                .Select(m => new AccountData()
                {
                    AccountID = m.AgencyUserId,
                    Email = m.Email,
                    FirstName = m.Firstname,
                    AccountType = "AO"
                }).ToListAsync();

            usersToEmail.AddRange(await _efDB.Students.Where(m => m.LastLoginDate >= minDate && m.LastLoginDate <= maxDate && m.ProfileStatusID == activeID)
                .Where(m => m.InactiveAccountReminderSentDate == null)
                .Select(m => new AccountData()
                {
                    AccountID = m.StudentId,
                    Email = m.Email,
                    FirstName = m.FirstName,
                    AccountType = "ST"
                }).ToListAsync());

            usersToEmail.AddRange(await _efDB.AcademiaUsers.Where(m => m.LastLoginDate >= minDate && m.LastLoginDate <= maxDate && m.ProfileStatusID == activeID)
                .Where(m => m.InactiveAccountReminderSentDate == null)
                .Select(m => new AccountData()
                {
                    AccountID = m.AcademiaUserId,
                    Email = m.Email,
                    FirstName = m.Firstname,
                    AccountType = "PI"
                }).ToListAsync());

            usersToEmail.AddRange(await _efDB.AdminUsers.Where(m => !m.IsDisabled && m.LastLoginDate >= minDate && m.LastLoginDate <= maxDate)
                .Where(m => m.InactiveAccountReminderSentDate == null)
                .Select(m => new AccountData()
                {
                    AccountID = m.AdminUserId,
                    Email = m.Email,
                    FirstName = m.FirstName,
                    AccountType = "AD"
                }).ToListAsync());
            
            return usersToEmail;
        }

        private async Task UpdateInactiveReminderSentDate(List<AccountData> accounts)
        {
            var students = accounts.Where(m => m.AccountType == "ST").ToList();
            if(students.Count > 0)
            {
                foreach (var student in students)
                {
                    var _studentToUpdate = await _efDB.Students.Where(m => m.StudentId == student.AccountID).FirstOrDefaultAsync();
                    _studentToUpdate.InactiveAccountReminderSentDate = DateTime.UtcNow;
                }
            }
            var agencyUsers = accounts.Where(m => m.AccountType == "AO").ToList();
            if (agencyUsers.Count > 0)
            {
                foreach (var ao in agencyUsers)
                {
                    var _aoToUpdate = await _efDB.AgencyUsers.Where(m => m.AgencyUserId == ao.AccountID).FirstOrDefaultAsync();
                    _aoToUpdate.InactiveAccountReminderSentDate = DateTime.UtcNow;
                }
            }
            var piUsers = accounts.Where(m => m.AccountType == "PI").ToList();
            if (piUsers.Count > 0)
            {
                foreach (var pi in piUsers)
                {
                    var _piToUpdate = await _efDB.AcademiaUsers.Where(m => m.AcademiaUserId == pi.AccountID).FirstOrDefaultAsync();
                    _piToUpdate.InactiveAccountReminderSentDate = DateTime.UtcNow;
                }
            }
            var adminUsers = accounts.Where(m => m.AccountType == "AD").ToList();
            if (adminUsers.Count > 0)
            {
                foreach (var admin in adminUsers)
                {
                    var _adminToUpdate = await _efDB.AdminUsers.Where(m => m.AdminUserId == admin.AccountID).FirstOrDefaultAsync();
                    _adminToUpdate.InactiveAccountReminderSentDate = DateTime.UtcNow;
                }
            }
            await _efDB.SaveChangesAsync();
        }

        private async Task SetTaskStateAsync(string state)
        {
            var task = await _efDB.ScheduledTask.Where(m => m.Name == "InactiveAccountReminderTask").FirstOrDefaultAsync();
            task.LastRunDate = DateTime.UtcNow;
            task.State = state;
            await _efDB.SaveChangesAsync();
        }

    }
    public class AccountData
    {
        public int AccountID { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string AccountType { get; set; }
    }
}
