using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Website.MarketingSite.Configurations
{
    public class ApiEndpointConfiguration
    {
        public readonly string ApiOrigin;
        public readonly string IdentityOrigin;

        public readonly string ProductsGetLatest;

        public readonly string SignUp;
        public readonly string GetIdentityToken;

        public ApiEndpointConfiguration(IConfiguration configuration)
        {
            ApiOrigin = configuration.GetValue<string>("ServiceOrigins:API");
            IdentityOrigin = configuration.GetValue<string>("ServiceOrigins:Identity");

            var apiConfigSection = configuration.GetSection("ServiceEndpoints:API");

            ProductsGetLatest = apiConfigSection.GetValue<string>("Products:GetLatest");

            var identityConfigSection = configuration.GetSection("ServiceEndpoints:Identity");

            SignUp = identityConfigSection.GetValue<string>("Identity:SignUp");
            GetIdentityToken = identityConfigSection.GetValue<string>("Identity:GetToken");
        }
    }
}
