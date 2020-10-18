using SendGrid;
using SendGrid.Helpers.Mail;
using System.Net.Mail;
using System.Threading.Tasks;

namespace SecureWebApi.Shared.Services
{
    public class SendGridMailServiceConfiguration : IMailServiceConfiguration
    {
        public string ApiKey { get; set; }
    }

    public class SendGridService : MailService, IMailService
    {
        private SendGridMailServiceConfiguration config;

        public SendGridService(SendGridMailServiceConfiguration config) : base(config)
        {
            this.config = config;
        }

        public async override Task<bool> Send(MailAddress receiver, MailAddress sender, string topic, string bodyHtml)
        {
            var client = new SendGridClient(config.ApiKey);
            var from = new EmailAddress(sender.Address, sender.DisplayName);
            var subject = topic;
            var to = new EmailAddress(receiver.Address, receiver.DisplayName);
            var plainTextContent = "";
            var htmlContent = bodyHtml;
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);

            if (response.StatusCode != System.Net.HttpStatusCode.Accepted) return false;

            return true;
        }
    }
}
