using Microsoft.VisualStudio.TestTools.UnitTesting;
using SecureWebApi.Shared.Services;
using System;
using System.Threading.Tasks;

namespace SecureWebApi.IntegrationTests
{
    /// <summary>
    /// Make sure that environment variables PlivoAuthId, PlivoAuthToken and PlivoReceiver is set!
    /// </summary>
    [TestClass]
    public class PlivoTests
    {
        /// <summary>
        /// Test if simply sending SMS works
        /// </summary>
        /// <returns>Test result</returns>
        [TestMethod]
        public async Task Send_SMS_Test()
        {
            var smsService = new PlivoService(new PlivoServiceConfiguration
            {
                authId = Environment.GetEnvironmentVariable("PlivoAuthId"),
                authToken = Environment.GetEnvironmentVariable("PlivoAuthToken")
            });

            var sendSmsResult = await smsService.Send(Environment.GetEnvironmentVariable("PlivoReceiver"), "Test sender", "Test message");

            Assert.AreEqual(sendSmsResult, true);
        }
    }
}
