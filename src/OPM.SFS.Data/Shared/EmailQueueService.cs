using Microsoft.Extensions.DependencyInjection;
using OPM.SFS.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPM.SFS.Core.Shared
{
    public interface IEmailQueueService
    {
        Task QueueEmailAsync(EmailQueue emailQueue);
    }

    public class EmailQueueService : IEmailQueueService
    {
        public readonly ScholarshipForServiceContext _efDB;
        public EmailQueueService(IServiceProvider serviceProvider)
        {
            _efDB = serviceProvider.CreateScope().ServiceProvider.GetRequiredService<ScholarshipForServiceContext>(); 
        }

        public async Task QueueEmailAsync(EmailQueue emailQueue)
        {
            _efDB.EmailQueue.Add(emailQueue);
            await _efDB.SaveChangesAsync();
        }
    }
}
