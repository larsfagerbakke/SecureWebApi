using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SecureWebApi;
using SecureWebApi.Shared.Services;
using System;
using System.IO;

[assembly: WebJobsStartup(typeof(Startup))]
namespace SecureWebApi
{
    public static class ApplicationExtensions
    {
        public static IWebJobsBuilder AddCustomBindings(this IWebJobsBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.AddExtension<AccessTokenBindingExtension>();

            return builder;
        }
    }

    public class Startup : IWebJobsStartup
    {
        public void Configure(IWebJobsBuilder builder)
        {
            // Read configuration from file
            var appConfig = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("local.settings.json", optional: true)
                .AddEnvironmentVariables()
                .Build();

            // Add services
            var config = new ConfigurationService(appConfig["jwt:jwtKey"], appConfig["jwt:jwtIssuer"], appConfig["jwt:jwtAudience"]);

            builder.Services.AddSingleton<ConfigurationService>((s) =>
            {
                return config;
            });

            builder.Services.AddSingleton<IUserService>((s) =>
            {
                return new MemoryUserService(config);
            });

            // Add custom bindings
            builder.AddCustomBindings();
        }
    }
}