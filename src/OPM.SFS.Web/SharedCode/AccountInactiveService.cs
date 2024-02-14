using OPM.SFS.Data;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using OPM.SFS.Web.Shared;
using Microsoft.Extensions.Configuration;
using OPM.SFS.Core.Shared;

namespace OPM.SFS.Web.SharedCode
{

    //TODO: Add Task API to call this service
    //Add Audit type table and fix auditing records
    //Add DB migration for moving last login date to Student table
    //Add API key to the Task API
    //Test scheduling with Octopus Runbooks
    //Add data migration to move last login date to the Student table. 
    public interface IAccountInactiveService
    {
        Task<bool> SetInactiveAcademiaUsers();
        Task<bool> SetInactiveAdmins();
        Task<bool> SetInactiveAgencyUsers();
        Task<bool> SetInactiveStudentsAsync();
        Task<bool> SetInactiveAllAccount();
        Task<bool> SendReminderEmailAsync(string accountType);
    }

    public class AccountInactiveService : IAccountInactiveService
    {
        private readonly ScholarshipForServiceContext _db;
        private readonly ICacheHelper _cache;
        public readonly IAuditEventLogHelper _auditLogger;
        private readonly IEmailerService _emailer;
        private readonly IConfiguration _appSettings;

        public AccountInactiveService(ScholarshipForServiceContext db, ICacheHelper cache, IAuditEventLogHelper audit, IEmailerService emailer, IConfiguration appSettings)
        {
            _db = db;
            _cache = cache;
            _auditLogger = audit;
            _emailer = emailer;
            _appSettings = appSettings;
        }

        public async Task<bool> SetInactiveAllAccount()
        {
            var GlobalConfigSettings = await _cache.GetGlobalConfigurationsAsync();
            var allStatus = await _cache.GetProfileStatusAsync();
            var inactiveID = allStatus.Where(m => m.Name == "Inactive").Select(m => m.ProfileStatusID).FirstOrDefault();
            int days = Convert.ToInt32(GlobalConfigSettings.Where(m => m.Key == "AccountExpireDays").Select(m => m.Value).FirstOrDefault());
            var loginDateFilter = DateTime.UtcNow.AddDays(days * -1);

            //agency users
            var agencyUsers = _db.AgencyUsers.Where(m => m.LastLoginDate < loginDateFilter && m.ProfileStatusID != inactiveID).ToList();
            agencyUsers.ForEach(m => { m.ProfileStatusID = inactiveID; m.LastUpdated = DateTime.UtcNow; });
            List<int> aoIDs = agencyUsers.Select(m => m.AgencyUserId).ToList();

            //students 
            var students = _db.Students.Where(m => m.LastLoginDate < loginDateFilter && m.ProfileStatusID != inactiveID).ToList();
            students.ForEach(m => { m.ProfileStatusID = inactiveID; m.LastUpdated = DateTime.UtcNow; });
            List<int> stIDs = students.Select(m => m.StudentId).ToList();

            //PIs
            var academiaUsers = _db.AcademiaUsers.Where(m => m.LastLoginDate < loginDateFilter && m.ProfileStatusID != inactiveID).ToList();
            academiaUsers.ForEach(m => { m.ProfileStatusID = inactiveID; m.LastUpdated = DateTime.UtcNow; });
            List<int> piIDs = academiaUsers.Select(m => m.AcademiaUserId).ToList();

            //admins
            var admins = _db.AdminUsers.Where(m => m.LastLoginDate < loginDateFilter && m.IsDisabled != false).ToList();
            admins.ForEach(m => { m.IsDisabled = true; m.LastUpdated = DateTime.UtcNow; });
            List<int> adminsIDs = admins.Select(m => m.AdminUserId).ToList();

            _db.SaveChanges();
            await _auditLogger.LogAuditEvent($"Task: InactiveAcconts - {aoIDs.Count} AO accounts for disabled due to inactivity.", additionalInfo: $"AO IDs: {string.Join(", ", aoIDs)}");
            await _auditLogger.LogAuditEvent($"Task: InactiveAcconts - {piIDs.Count} PI accounts for disabled due to inactivity.", additionalInfo: $"PI IDs: {string.Join(", ", piIDs)}");
            await _auditLogger.LogAuditEvent($"Task: InactiveAcconts - {stIDs.Count} ST accounts for disabled due to inactivity.", additionalInfo: $"ST IDs: {string.Join(", ", stIDs)}");
            await _auditLogger.LogAuditEvent($"Task: InactiveAcconts - {adminsIDs.Count} AD accounts for disabled due to inactivity.", additionalInfo: $"AD IDs: {string.Join(", ", adminsIDs)}");

            return true;
        }

        public async Task<bool> SetInactiveStudentsAsync()
        {

            var GlobalConfigSettings = await _cache.GetGlobalConfigurationsAsync();
            var allStatus = await _cache.GetProfileStatusAsync();
            var inactiveID = allStatus.Where(m => m.Name == "Inactive").Select(m => m.ProfileStatusID).FirstOrDefault();
            int days = Convert.ToInt32(GlobalConfigSettings.Where(m => m.Key == "AccountExpireDays").Select(m => m.Value).FirstOrDefault());
            var loginDateFilter = DateTime.UtcNow.AddDays(days * -1);
            var students = _db.Students.Where(m => m.LastLoginDate < loginDateFilter && m.ProfileStatusID != inactiveID).ToList();
            if (students != null && students.Count > 0)
            {
                students.ForEach(m => { m.ProfileStatusID = inactiveID; m.LastUpdated = DateTime.Now; });
                List<int> stIDs = students.Select(m => m.StudentId).ToList();
                _db.SaveChanges();
                await _auditLogger.LogAuditEvent($"Task: InactiveAcconts - {stIDs.Count} ST accounts for disabled due to inactivity.", additionalInfo: $"ST IDs: {string.Join(", ", stIDs)}");
            }
            return true;
        }

        public async Task<bool> SetInactiveAcademiaUsers()
        {
            var GlobalConfigSettings = await _cache.GetGlobalConfigurationsAsync();
            var allStatus = await _cache.GetProfileStatusAsync();
            var inactiveID = allStatus.Where(m => m.Name == "Inactive").Select(m => m.ProfileStatusID).FirstOrDefault();
            int days = Convert.ToInt32(GlobalConfigSettings.Where(m => m.Key == "AccountExpireDays").Select(m => m.Value).FirstOrDefault());
            var loginDateFilter = DateTime.UtcNow.AddDays(days * -1);
            var academiaUsers = _db.AcademiaUsers.Where(m => m.LastLoginDate < loginDateFilter && m.ProfileStatusID != inactiveID).ToList();
            if (academiaUsers != null && academiaUsers.Count > 0)
            {
                List<int> piIDs = academiaUsers.Select(m => m.AcademiaUserId).ToList();
                academiaUsers.ForEach(m => { m.ProfileStatusID = inactiveID; m.LastUpdated = DateTime.Now; });
                _db.SaveChanges();
                await _auditLogger.LogAuditEvent($"Task: InactiveAcconts - {piIDs.Count} PI accounts for disabled due to inactivity.", additionalInfo: $"PI IDs: {string.Join(", ", piIDs)}");
            }
            return true;
        }

        public async Task<bool> SetInactiveAgencyUsers()
        {
            var GlobalConfigSettings = await _cache.GetGlobalConfigurationsAsync();
            var allStatus = await _cache.GetProfileStatusAsync();
            var inactiveID = allStatus.Where(m => m.Name == "Inactive").Select(m => m.ProfileStatusID).FirstOrDefault();
            int days = Convert.ToInt32(GlobalConfigSettings.Where(m => m.Key == "AccountExpireDays").Select(m => m.Value).FirstOrDefault());
            var loginDateFilter = DateTime.UtcNow.AddDays(days * -1);
            var agencyUsers = _db.AgencyUsers.Where(m => m.LastLoginDate < loginDateFilter && m.ProfileStatusID != inactiveID).ToList();
            if (agencyUsers != null && agencyUsers.Count > 0)
            {
                agencyUsers.ForEach(m => { m.ProfileStatusID = inactiveID; m.LastUpdated = DateTime.Now; });
                List<int> aoIDs = agencyUsers.Select(m => m.AgencyUserId).ToList();
                _db.SaveChanges();
                await _auditLogger.LogAuditEvent($"Task: InactiveAcconts - {aoIDs.Count} AO accounts for disabled due to inactivity.", additionalInfo: $"AO IDs: {string.Join(", ", aoIDs)}");
            }
            return true;
        }

        public async Task<bool> SetInactiveAdmins()
        {
            var GlobalConfigSettings = await _cache.GetGlobalConfigurationsAsync();
            var allStatus = await _cache.GetProfileStatusAsync();
            var inactiveID = allStatus.Where(m => m.Name == "Inactive").Select(m => m.ProfileStatusID).FirstOrDefault();
            int days = Convert.ToInt32(GlobalConfigSettings.Where(m => m.Key == "AccountExpireDays").Select(m => m.Value).FirstOrDefault());
            var loginDateFilter = DateTime.UtcNow.AddDays(days * -1);
            var admins = _db.AdminUsers.Where(m => m.LastLoginDate < loginDateFilter && m.IsDisabled != false).ToList();
            if (admins != null && admins.Count > 0)
            {
                admins.ForEach(m => { m.IsDisabled = true; m.LastUpdated = DateTime.Now; });
                List<int> adminsIDs = admins.Select(m => m.AdminUserId).ToList();
                _db.SaveChanges();
                await _auditLogger.LogAuditEvent($"Task: InactiveAcconts - {adminsIDs.Count} AD accounts for disabled due to inactivity.", additionalInfo: $"AD IDs: {string.Join(", ", adminsIDs)}");
            }

            return true;
        }

        public async Task<bool> SendReminderEmailAsync(string accountType)
        {

            string baseUrl = _appSettings["General:BaseUrl"];

            var accounts = await GetAccountsForRemindersAsync(accountType);
            var loginLink = "";
            //var loginLink = $"{baseUrl}/Student/Login";
            if (accountType == "ST") loginLink = $"{baseUrl}/Student/Login";
            if (accountType == "PI") loginLink = $"{baseUrl}/Academia/Login";
            if (accountType == "AO") loginLink = $"{baseUrl}/Agency/Login";
            if (accountType == "AD") loginLink = $"{baseUrl}/Admin/Login";
            foreach (var a in accounts)
            {
                string emailContent = $@"Hello {a.FirstName}, <br/><br/>
                                    Its been awhile since you've logged into your SFS account and it will become inactive in 10 days. If you want to keep your SFS account active, sign into your account at 
                                    <a href='{loginLink}'>SFS</a>";
                await _emailer.SendEmailDefaultTemplateAsync(a.Email, "SFS Account Inactive in 10 Days", emailContent);
            }

            return true;
        }

        public async Task<List<AccountData>> GetAccountsForRemindersAsync(string type)
        {
            var GlobalConfigSettings = await _cache.GetGlobalConfigurationsAsync();
            var allStatus = await _cache.GetProfileStatusAsync();
            var inactiveID = allStatus.Where(m => m.Name == "Inactive").Select(m => m.ProfileStatusID).FirstOrDefault();
            int days = Convert.ToInt32(GlobalConfigSettings.Where(m => m.Key == "AccountExpireDays").Select(m => m.Value).FirstOrDefault());
            int reminderDays = Convert.ToInt32(GlobalConfigSettings.Where(m => m.Key == "AccountReminderDays").Select(m => m.Value).FirstOrDefault());
            days = days - reminderDays;
            var loginDateFilter = DateTime.UtcNow.AddDays(days * -1);
            DateTime minDate = new DateTime(loginDateFilter.Date.Year, loginDateFilter.Date.Month, loginDateFilter.Date.Day);
            DateTime maxDate = minDate.AddSeconds(86399);
            List<AccountData> usersToEmail = new();
            if (type == "AO")
            {
                usersToEmail = await _db.AgencyUsers.Where(m => m.LastLoginDate >= minDate && m.LastLoginDate <= maxDate && m.ProfileStatusID != inactiveID)
                    .Select(m => new AccountData()
                    {
                        Email = m.Email,
                        FirstName = m.Firstname
                    }).ToListAsync();
            }
            if (type == "ST")
            {
                usersToEmail = await _db.Students.Where(m => m.LastLoginDate >= minDate && m.LastLoginDate <= maxDate && m.ProfileStatusID != inactiveID)
                    .Select(m => new AccountData()
                    {
                        Email = m.Email,
                        FirstName = m.FirstName
                    }).ToListAsync();
            }
            if (type == "PI")
            {
                usersToEmail = await _db.AcademiaUsers.Where(m => m.LastLoginDate >= minDate && m.LastLoginDate <= maxDate && m.ProfileStatusID != inactiveID)
                    .Select(m => new AccountData()
                    {
                        Email = m.Email,
                        FirstName = m.Firstname
                    }).ToListAsync();
            }
            if (type == "AD")
            {
                usersToEmail = await _db.AdminUsers.Where(m => m.IsDisabled == false && m.LastLoginDate >= minDate && m.LastLoginDate <= maxDate)
                    .Select(m => new AccountData()
                    {
                        Email = m.Email,
                        FirstName = m.FirstName
                    }).ToListAsync();
            }
            return usersToEmail;
        }


    }

    public class AccountData
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
    }
}
