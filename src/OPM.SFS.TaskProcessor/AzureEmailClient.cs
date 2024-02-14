using Azure;
using Azure.Communication.Email;

namespace OPM.SFS.TaskProcessor
{

	public interface IAzureEmailClient
	{
		Task SendEmailAsync(string recepients, string subject, string content);
	}

	public class AzureEmailClient : IAzureEmailClient
    {
		private readonly IConfiguration _appSettings;

		public AzureEmailClient(IConfiguration appSettings) => _appSettings = appSettings;

		public async Task SendEmailAsync(string recepients, string subject, string content)
		{
			string emailconnection = _appSettings["Azure:EmailEndpoint"];
			EmailClient emailClient = new EmailClient(emailconnection);			

            var sender = _appSettings["Azure:EmailFromAddress"];            
            var emailContent = new EmailContent(subject)
            {
                Html = content, 
            };

            var toRecipients = new List<EmailAddress>()
			{
				new EmailAddress(recepients)
			};
            var emailRecipients = new EmailRecipients(toRecipients);
            var emailMessage = new EmailMessage(sender, emailRecipients, emailContent);
            await emailClient.SendAsync(WaitUntil.Started, emailMessage);
        }

	}
}
