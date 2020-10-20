using System.Threading.Tasks;

namespace SecureWebApi.Shared.Services
{
    public interface ISMSServiceConfiguration
    {

    }

    public interface ISMSService
    {
        Task<bool> Send(string receiver, string sender, string message);
    }

    public abstract class SMSService : ISMSService
    {
        private SMSService() { }

        public SMSService(ISMSServiceConfiguration config) { }

        public abstract Task<bool> Send(string receiver, string sender, string message);
    }
}
