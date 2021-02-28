using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Website.MarketingSite.Configurations
{
    public static class ApiEndpointConfiguration
    {
        public static void GetConfiguration(IConfiguration configuration)
        {
            var configSection = configuration.GetSection("ServiceEndpoints:API");

            ProductsGetLatest = configSection.GetValue<string>("Products:GetLatest");
        }

        public static string ProductsGetLatest;
    }
}
