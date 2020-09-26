using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Hosting;
using Microsoft.Extensions.DependencyInjection;
using SecureWebApi;
using SecureWebApi.Shared.Services;
using System;

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
            // Add services
            builder.Services.AddSingleton<IUserService>((s) =>
            {
                return new MemoryUserService();
            });

            // Add custom bindings
            builder.AddCustomBindings();
        }
    }
}