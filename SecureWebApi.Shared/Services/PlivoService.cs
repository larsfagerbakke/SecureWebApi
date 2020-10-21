using Plivo;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SecureWebApi.Shared.Services
{
    /// <summary>
    /// Plivo configuration object
    /// </summary>
    public class PlivoServiceConfiguration : ISMSServiceConfiguration
    {
        public string authId { get; set; }
        public string authToken { get; set; }
    }

    /// <summary>
    /// Implementation of Plivo, a sms service for sending sms
    /// </summary>
    public class PlivoService : SMSService, ISMSService
    {
        private PlivoServiceConfiguration config;

        public PlivoService(PlivoServiceConfiguration config) : base(config)
        {
            this.config = config;
        }

        /// <summary>
        /// Function for sending sms
        /// </summary>
        /// <param name="receiver">Number of the receiver, must be "+XXZZZZZZZZ"</param>
        /// <param name="sender">Could really be anything as a string, even text</param>
        /// <param name="message">Message to send. Keep it short</param>
        /// <returns>Send sms result</returns>
        public async override Task<bool> Send(string receiver, string sender, string message)
        {
            var api = new PlivoApi(config.authId, config.authToken);

            var response = api.Message.Create(
                src: sender,
                dst: new List<String> { receiver },
                text: message
                );

            if (response.StatusCode != 202) return false;

            return true;
        }
    }
}
