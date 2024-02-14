using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OPM.SFS.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPM.SFS.Core.Shared
{
    public interface IEmailerService
    {
        Task<bool> SendEmailDefaultTemplateAsync(string to, string subject, string messageBody);
        Task<bool> SendEmailNoTemplateAsync(string to, string subject, string messageBody);
        Task<bool> SendEmailWithTemplateAsync(string to, string templateCode, EmailTemplateModel emailData);
    }

    public class EmailerService : IEmailerService
    {

        private readonly ILogger<EmailerService> _logger;
        private readonly IConfiguration _appSettings;
        private readonly IEmailQueueService _emailQueue;
        private readonly ScholarshipForServiceContext _efDB;


        public EmailerService(ILogger<EmailerService> logger, IConfiguration appSettings, IEmailQueueService emailQueue, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _appSettings = appSettings;
            _emailQueue = emailQueue;
            _efDB = serviceProvider.CreateScope().ServiceProvider.GetRequiredService<ScholarshipForServiceContext>();
        }


        public async Task<bool> SendEmailNoTemplateAsync(string to, string subject, string messageBody)
        {
            if (_appSettings["EmailSettings:DisableEmail"].ToString().Equals("true"))
                return false;
            
            try
            {

                if (to.Contains(";"))
                {
                    var emailList = to.Split(";").ToList();
                    string fromAddress = _appSettings["Azure:EmailFromAddress"];
                    foreach (string uri in emailList)
                        await _emailQueue.QueueEmailAsync(new EmailQueue() { QueueDate = DateTime.UtcNow, ToUri = uri, Body = messageBody, Subject = subject, FromUri = fromAddress });
                }
                else
                {

                    string fromAddress = _appSettings["Azure:EmailFromAddress"];
                    await _emailQueue.QueueEmailAsync(new EmailQueue() { QueueDate = DateTime.UtcNow, ToUri = to, Body = messageBody, Subject = subject, FromUri = fromAddress });

                }
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured when sending email. \n{Errors}", ex.Message);
            }
            return false;
        }

        public async Task<bool> SendEmailDefaultTemplateAsync(string to, string subject, string messageBody)
        {
            if (_appSettings["EmailSettings:DisableEmail"].ToString().Equals("true"))
                return false;

            var defaultTemplate = await _efDB.EmailTemplates.Where(m => m.Code == "Default_Template").Select(m => m.Template).FirstOrDefaultAsync();
            var messageWithTemplate = defaultTemplate.BindObjectProperties(new { EmailContent = messageBody });

            if (to.Contains(";"))
            {

                var emailList = to.Split(";").ToList();
                string fromAddress = _appSettings["Azure:EmailFromAddress"];
                foreach (string uri in emailList)
                    await _emailQueue.QueueEmailAsync(new EmailQueue() { QueueDate = DateTime.UtcNow, ToUri = uri, Body = messageWithTemplate, Subject = subject, FromUri = fromAddress });
            }
            else
            {
                string fromAddress = _appSettings["Azure:EmailFromAddress"];
                await _emailQueue.QueueEmailAsync(new EmailQueue() { QueueDate = DateTime.UtcNow, ToUri = to, Body = messageWithTemplate, Subject = subject, FromUri = fromAddress });
            }
            return true;
        }

        public async Task<bool> SendEmailWithTemplateAsync(string to, string templateCode, EmailTemplateModel emailData)
        {
            if (_appSettings["EmailSettings:DisableEmail"].ToString().Equals("true"))
                return false;

            var templateData = await _efDB.EmailTemplates.Where(m => m.Code == templateCode).FirstOrDefaultAsync();
            string message = templateData.Template.BindObjectProperties(emailData);
            string subject = templateData.Subject.BindObjectProperties(emailData);

            if (to.Contains(";"))
            {

                var emailList = to.Split(";").ToList();
                string fromAddress = _appSettings["Azure:EmailFromAddress"];
                foreach (string uri in emailList)
                    await _emailQueue.QueueEmailAsync(new EmailQueue() { QueueDate = DateTime.UtcNow, ToUri = uri, Body = message, Subject = subject, FromUri = fromAddress });

            }
            else
            {

                string fromAddress = _appSettings["Azure:EmailFromAddress"];
                await _emailQueue.QueueEmailAsync(new EmailQueue() { QueueDate = DateTime.UtcNow, ToUri = to, Body = message, Subject = subject, FromUri = fromAddress });

            }
            return true;
        }
    }
}
