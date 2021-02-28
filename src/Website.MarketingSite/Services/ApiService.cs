using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Website.MarketingSite.Configurations;
using Website.MarketingSite.Models.ViewModels;

namespace Website.MarketingSite.Services
{
    public class ApiService
    {
        private readonly ILogger<ApiService> _logger;

        public HttpClient Client { get; }

        public ApiService(HttpClient client, IConfiguration configuration, ILogger<ApiService> logger)
        {
            client.BaseAddress = new Uri(configuration.GetValue<string>("ServiceOrigins:API"));
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            Client = client;

            _logger = logger;
        }

        public async Task<IEnumerable<ProductViewModel>> GetLatestProductsAsync()
        {
            IEnumerable<ProductViewModel> products = null;

            var uri = ApiEndpointConfiguration.ProductsGetLatest;

            if (uri == null)
            {
                _logger.LogError("Get product error: uri is null");
            }

            try
            {
                var response = await Client.GetAsync(uri);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError(string.Format("Error: status code {0}", response.StatusCode));
                }

                var raw = await response.Content.ReadAsStringAsync();

                products = JsonConvert.DeserializeObject<IEnumerable<ProductViewModel>>(raw);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            return products;
        }
    }
}
