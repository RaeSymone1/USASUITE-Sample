using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OPM.SFS.Web.Infrastructure
{
    public class BackgroundServiceWorker : BackgroundService
    {
        private readonly ILogger<BackgroundServiceWorker> _logger;

        public BackgroundServiceWorker(ILogger<BackgroundServiceWorker> logger)
        {
            //Todo: Add Hangfire for scheduling and new table to store jobs
            //This will be used to run scheduled jobs...
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("SFS is running a task at: {time}", DateTime.UtcNow);
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
