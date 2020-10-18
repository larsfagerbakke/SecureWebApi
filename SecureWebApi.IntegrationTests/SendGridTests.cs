using Microsoft.VisualStudio.TestTools.UnitTesting;
using SecureWebApi.Shared.Services;
using System;
using System.Threading.Tasks;

namespace SecureWebApi.IntegrationTests
{
    /// <summary>
    /// Make sure that environment variables SendGridApiKey and SendGridIntegrationTestEmail is set!
    /// </summary>
    [TestClass]
    public class SendGridTests
    {
        /// <summary>
        /// Test if simply sending mail works
        /// </summary>
        /// <returns>Test result</returns>
        [TestMethod]
        public async Task Send_Mail_Test()
        {
            var mail = new SendGridService(new SendGridMailServiceConfiguration()
            {
                ApiKey = Environment.GetEnvironmentVariable("SendGridApiKey")
            });

            var mailResponse = await mail.Send(
                new System.Net.Mail.MailAddress(Environment.GetEnvironmentVariable("SendGridIntegrationTestEmail"), "Receiver display name"),
                new System.Net.Mail.MailAddress("integration@test.mail", "Sender display name"),
                "Integration test mail",
                "<b>Integration test mail");

            Assert.AreEqual(mailResponse, true);
        }
    }
}
