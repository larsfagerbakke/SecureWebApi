using Microsoft.Azure.WebJobs.Description;
using Microsoft.Azure.WebJobs.Host.Bindings;
using Microsoft.Azure.WebJobs.Host.Config;
using Microsoft.Azure.WebJobs.Host.Protocols;
using SecureWebApi.Shared.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SecureWebApi
{
    [Binding]
    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.ReturnValue)]
    public class AccessTokenBindingAttribute : Attribute { }

    [Extension("TokenBinding")]
    public class AccessTokenBindingExtension : IExtensionConfigProvider
    {
        private readonly ConfigurationService config;

        public AccessTokenBindingExtension(ConfigurationService config)
        {
            this.config = config;
        }

        public void Initialize(ExtensionConfigContext context)
        {
            context.AddBindingRule<AccessTokenBindingAttribute>().Bind(new AccessTokenBindingProvider(config));
        }
    }

    public class AccessTokenBindingProvider : IBindingProvider
    {
        private readonly ConfigurationService config;

        public AccessTokenBindingProvider(ConfigurationService config)
        {
            this.config = config;
        }

        public Task<IBinding> TryCreateAsync(BindingProviderContext context)
        {
            IBinding binding = new AccessTokenBinding(config);
            return Task.FromResult(binding);
        }
    }

    public class AccessTokenBinding : IBinding
    {
        private readonly ConfigurationService config;

        public AccessTokenBinding(ConfigurationService config)
        {
            this.config = config;
        }

        public bool FromAttribute => false;
        public ParameterDescriptor ToParameterDescriptor() => new ParameterDescriptor();
        public Task<IValueProvider> BindAsync(object value, ValueBindingContext context) => null;

        public Task<IValueProvider> BindAsync(BindingContext context)
        {
            var headers = context.BindingData["Headers"] as IDictionary<string, string>;

            if (!headers.ContainsKey("Authorization"))
                return Task.FromResult<IValueProvider>(new AccessTokenProvider());

            return Task.FromResult<IValueProvider>(new AccessTokenProvider(headers["Authorization"].Split(' ')[1], config));
        }
    }

    internal class AccessTokenProvider : IValueProvider
    {
        private readonly ConfigurationService config; 

        private string accessToken;

        public AccessTokenProvider() { }

        public AccessTokenProvider(string accessToken, ConfigurationService config)
        {
            this.config = config;
            this.accessToken = accessToken;
        }

        public Type Type => null;
        public string ToInvokeString() => "";

        public async Task<object> GetValueAsync()
        {
            try
            {
                var decryptedToken = SecureWebApi.Shared.Helpers.Authentication.TokenHelper.ReadToken(accessToken, config.jwtKey, config.jwtIssuer, config.jwtAudience);

                return new AccessTokenModel { TokenState = AccessTokenModel.State.Valid, Roles = new List<AccessTokenModel.Role> { AccessTokenModel.Role.User }, UserId = decryptedToken.Id };
            }
            catch
            {
                return new AccessTokenModel { TokenState = AccessTokenModel.State.Invalid };
            }
        }
    }
}
