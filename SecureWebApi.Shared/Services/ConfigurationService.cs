using System;
using System.Collections.Generic;
using System.Text;

namespace SecureWebApi.Shared.Services
{
    public class ConfigurationService
    {
        public string jwtKey { get; set; }
        public string jwtIssuer { get; set; }
        public string jwtAudience { get; set; }

        private ConfigurationService() { }

        public ConfigurationService(string jwtKey, string jwtIssuer, string jwtAudience)
        {
            this.jwtKey = jwtKey;
            this.jwtIssuer = jwtIssuer;
            this.jwtAudience = jwtAudience;
        }
    }
}
