using System.Net.Mail;
using System.Threading.Tasks;

namespace SecureWebApi.Shared.Services
{
    public interface IMailServiceConfiguration
    {

    }

    public interface IMailService
    {
        Task<bool> Send(MailAddress receiver, MailAddress sender, string topic, string bodyHtml);
    }

    public abstract class MailService : IMailService
    {
        private MailService() { }

        public MailService(IMailServiceConfiguration config) { }

        public abstract Task<bool> Send(MailAddress receiver, MailAddress sender, string topic, string bodyHtml);
    }
}
