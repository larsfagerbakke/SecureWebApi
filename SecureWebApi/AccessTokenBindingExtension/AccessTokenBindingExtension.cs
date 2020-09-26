using Microsoft.Azure.WebJobs.Description;
using Microsoft.Azure.WebJobs.Host.Bindings;
using Microsoft.Azure.WebJobs.Host.Config;
using Microsoft.Azure.WebJobs.Host.Protocols;
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
        public void Initialize(ExtensionConfigContext context)
        {
            context.AddBindingRule<AccessTokenBindingAttribute>().Bind(new AccessTokenBindingProvider());
        }
    }

    public class AccessTokenBindingProvider : IBindingProvider
    {
        public Task<IBinding> TryCreateAsync(BindingProviderContext context)
        {
            IBinding binding = new AccessTokenBinding();
            return Task.FromResult(binding);
        }
    }

    public class AccessTokenBinding : IBinding
    {
        public bool FromAttribute => false;
        public ParameterDescriptor ToParameterDescriptor() => new ParameterDescriptor();
        public Task<IValueProvider> BindAsync(object value, ValueBindingContext context) => null;

        public Task<IValueProvider> BindAsync(BindingContext context)
        {
            var headers = context.BindingData["Headers"] as IDictionary<string, string>;

            if (!headers.ContainsKey("Authorization"))
                return Task.FromResult<IValueProvider>(new AccessTokenProvider());

            return Task.FromResult<IValueProvider>(new AccessTokenProvider(headers["Authorization"].Split(' ')[1]));
        }
    }

    internal class AccessTokenProvider : IValueProvider
    {
        private string accessToken;

        public AccessTokenProvider() { }

        public AccessTokenProvider(string accessToken)
        {
            this.accessToken = accessToken;
        }

        public Type Type => null;
        public string ToInvokeString() => "";

        public async Task<object> GetValueAsync()
        {
            try
            {
                var decryptedToken = SecureWebApi.Shared.Helpers.Authentication.TokenHelper.ReadToken(accessToken, "20ddfb841b924343b60affc56b2267c5", "jwtIssuer", "jwtAudience"); // TODO: Fetch from configuration

                return new AccessTokenModel { TokenState = AccessTokenModel.State.Valid, Roles = new List<AccessTokenModel.Role> { AccessTokenModel.Role.User }, UserId = decryptedToken.Id };
            }
            catch
            {
                return new AccessTokenModel { TokenState = AccessTokenModel.State.Invalid };
            }
        }
    }
}
