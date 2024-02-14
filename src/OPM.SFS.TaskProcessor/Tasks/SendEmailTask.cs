using Medallion.Threading.SqlServer;
using Microsoft.EntityFrameworkCore;
using OPM.SFS.Data;
using Sgbj.Cron;

namespace OPM.SFS.TaskProcessor.Tasks
{
	/// <summary>
	/// Checks the email queue and sends emails based on the Azure Communications Services
	/// limits
	/// </summary>
	public class SendEmailTask : BackgroundService
    {
		public readonly ScholarshipForServiceContext _efDB;
		private readonly IAzureEmailClient _azureEmail;
		private readonly ILogger _logger;

		public SendEmailTask(IAzureEmailClient azureEmail, ILogger<SendEmailTask> logger, IServiceProvider serviceProvider)
		{
			
			_azureEmail = azureEmail;
			_logger = logger;
            _efDB = serviceProvider.CreateScope().ServiceProvider.GetRequiredService<ScholarshipForServiceContext>();
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            //runs every 3 minutes
            var task = await _efDB.ScheduledTask.Where(m => m.Name == "SendEmailTask").FirstOrDefaultAsync();
            if (!task.IsDisabled)
            {
                using var timer = new CronTimer(task.Schedule, TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time"));
                while (await timer.WaitForNextTickAsync())
                {
                    var @lock = new SqlDistributedLock("SendEmailLock", _efDB.Database.GetConnectionString());
                    await using (await @lock.AcquireAsync())
                    {
                        try
                        {
                            _logger.LogInformation("QueueTask SendEmailTask - Checking for emails to send");
							await SetTaskStateAsync("RUNNING");
							var emailsToSend = _efDB.EmailQueue.ToList().Take(30).ToList();
                            _logger.LogInformation($"QueueTask SendEmailTask - Found {emailsToSend.Count} emails to send");
                            foreach (var email in emailsToSend)
                            {
                                _logger.LogInformation($"Sending email for {email.Subject} to {email.ToUri} from {email.FromUri}");
                                await _azureEmail.SendEmailAsync(email.ToUri, email.Subject, email.Body);
                                _efDB.EmailQueue.Remove(email);
                            }
                            await _efDB.SaveChangesAsync();
							await SetTaskStateAsync("COMPLETE");
							_logger.LogInformation("QueueTask SendEmailTask completed");
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex.ToString());
                        }
                    }

                }
            }
        }

		private async Task SetTaskStateAsync(string state)
		{
			var task = await _efDB.ScheduledTask.Where(m => m.Name == "SendEmailTask").FirstOrDefaultAsync();
			task.LastRunDate = DateTime.UtcNow;
			task.State = state;
			await _efDB.SaveChangesAsync();
		}
	}
}
