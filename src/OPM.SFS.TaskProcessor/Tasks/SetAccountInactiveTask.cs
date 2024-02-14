using Medallion.Threading.SqlServer;
using Microsoft.EntityFrameworkCore;
using OPM.SFS.Core.Shared;
using OPM.SFS.Data;
using Sgbj.Cron;

namespace OPM.SFS.TaskProcessor.Tasks
{
	/// <summary>
	/// Sets accounts inactive after X days of inactivity
	/// </summary>
	public class SetAccountInactiveTask : BackgroundService
    {
        public readonly ScholarshipForServiceContext _efDB;
        private readonly IAzureEmailClient _azureEmail;
        private readonly ILogger _logger;
        private readonly IUtilitiesService _utilties;

        public SetAccountInactiveTask(IAzureEmailClient azureEmail, ILogger<SetAccountInactiveTask> logger, IServiceProvider serviceProvider, IUtilitiesService utilities)
		{            
            _azureEmail = azureEmail;
            _logger = logger;
            _efDB = serviceProvider.CreateScope().ServiceProvider.GetRequiredService<ScholarshipForServiceContext>();
            _utilties = utilities;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            //runs everyday at 2AM EST
            var task = await _efDB.ScheduledTask.Where(m => m.Name == "SetAccountInactiveTask").FirstOrDefaultAsync();
            if (!task.IsDisabled)
            {
                using var timer = new CronTimer(task.Schedule, TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time"));
                while (await timer.WaitForNextTickAsync())
                {
                    var @lock = new SqlDistributedLock("SetAccountInactiveLock", _efDB.Database.GetConnectionString());
                    await using (await @lock.AcquireAsync())
                    {
                        _logger.LogInformation("ScheduledTask SetAccountInactiveTask running");
						await SetTaskStateAsync("RUNNING");
						await SetStudentAccountInactive();
                        await SetAdminAccountInactive();
                        await SetAgencyAccountInactive();
                        await SetAcademiaAccountInactive();
						await SetTaskStateAsync("COMPLETE");
						_logger.LogInformation("ScheduledTask SetAccountInactiveTask completed");
                    }
                }
            }
           
        }
		private async Task SetTaskStateAsync(string state)
		{
			var task = await _efDB.ScheduledTask.Where(m => m.Name == "SetAccountInactiveTask").FirstOrDefaultAsync();
			task.LastRunDate = DateTime.UtcNow;
			task.State = state;
			await _efDB.SaveChangesAsync();
		}

		private async Task SetStudentAccountInactive()
        {
            int inactiveID = _efDB.ProfileStatus.Where(m => m.Name == "Inactive").Select(m => m.ProfileStatusID).FirstOrDefault();
            int expireDays = Convert.ToInt32(_efDB.GlobalConfiguration.Where(m => m.Key == "AccountExpireDays").Select(m => m.Value).FirstOrDefault());
            var loginDateFilter = DateTime.UtcNow.AddDays(expireDays * -1);
            var studentsToMakeInactive = _efDB.Students.Where(m => m.LastLoginDate < loginDateFilter && m.ProfileStatusID != inactiveID).ToList();
            studentsToMakeInactive.ForEach(m => { m.ProfileStatusID = inactiveID; m.LastUpdated = DateTime.Now; m.UserId = "SetAccountInactiveTask"; });            
            await _efDB.SaveChangesAsync();
            _logger.LogInformation($"Scheduled Task SetAccountInactiveTask set {studentsToMakeInactive.Count} inactive");
        }

        private async Task SetAdminAccountInactive()
        {
            int expireDays = Convert.ToInt32(_efDB.GlobalConfiguration.Where(m => m.Key == "AccountExpireDays").Select(m => m.Value).FirstOrDefault());
            var loginDateFilter = DateTime.UtcNow.AddDays(expireDays * -1);
            var adminsToMakeInactive = _efDB.AdminUsers.Where(m => m.LastLoginDate < loginDateFilter && !m.IsDisabled).ToList();
            adminsToMakeInactive.ForEach(m => { m.IsDisabled = true; m.LastUpdated = DateTime.Now; });
            await _efDB.SaveChangesAsync();
            _logger.LogInformation($"Scheduled Task SetAccountInactiveTask set {adminsToMakeInactive.Count} inactive");
        }
        private async Task SetAcademiaAccountInactive()
        {
            int inactiveID = _efDB.ProfileStatus.Where(m => m.Name == "Inactive").Select(m => m.ProfileStatusID).FirstOrDefault();
            int expireDays = Convert.ToInt32(_efDB.GlobalConfiguration.Where(m => m.Key == "AccountExpireDays").Select(m => m.Value).FirstOrDefault());
            var loginDateFilter = DateTime.UtcNow.AddDays(expireDays * -1);
            var academiaUsersToMakeInactive = _efDB.AcademiaUsers.Where(m => m.LastLoginDate < loginDateFilter && m.ProfileStatusID != inactiveID).ToList();
            academiaUsersToMakeInactive.ForEach(m => { m.ProfileStatusID = inactiveID; m.LastUpdated = DateTime.Now; });
            await _efDB.SaveChangesAsync();
            _logger.LogInformation($"Scheduled Task SetAccountInactiveTask set {academiaUsersToMakeInactive.Count} inactive");
        }

        private async Task SetAgencyAccountInactive()
        {
            int inactiveID = _efDB.ProfileStatus.Where(m => m.Name == "Inactive").Select(m => m.ProfileStatusID).FirstOrDefault();
            int expireDays = Convert.ToInt32(_efDB.GlobalConfiguration.Where(m => m.Key == "AccountExpireDays").Select(m => m.Value).FirstOrDefault());
            var loginDateFilter = DateTime.UtcNow.AddDays(expireDays * -1);
            var agencyUsersToMakeInactive = _efDB.AgencyUsers.Where(m => m.LastLoginDate < loginDateFilter && m.ProfileStatusID != inactiveID).ToList();
            agencyUsersToMakeInactive.ForEach(m => { m.ProfileStatusID = inactiveID; m.LastUpdated = DateTime.Now;  });
            await _efDB.SaveChangesAsync();
            _logger.LogInformation($"Scheduled Task SetAccountInactiveTask set {agencyUsersToMakeInactive.Count} inactive");
        }
    }

}

