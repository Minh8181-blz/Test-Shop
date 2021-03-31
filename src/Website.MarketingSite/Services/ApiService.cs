using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Website.MarketingSite.Configurations;
using Website.MarketingSite.Models.ViewModels;

namespace Website.MarketingSite.Services
{
    public class ApiService : HttpServiceBase
    {
        private readonly ILogger<ApiService> _logger;
        private readonly ApiEndpointConfiguration _endpointConfiguration;

        public ApiService(HttpClient client, ApiEndpointConfiguration endpointConfiguration, ILogger<ApiService> logger) : base(client)
        {
            _endpointConfiguration = endpointConfiguration;

            Client.BaseAddress = new Uri(_endpointConfiguration.ApiOrigin);

            _logger = logger;
        }

        public async Task<IEnumerable<ProductViewModel>> GetLatestProductsAsync()
        {
            IEnumerable<ProductViewModel> products = null;

            try
            {
                var response = await GetAsync(_endpointConfiguration.ProductsGetLatest);

                if (response.IsSuccessStatusCode)
                {
                    var raw = await response.Content.ReadAsStringAsync();

                    products = JsonConvert.DeserializeObject<IEnumerable<ProductViewModel>>(raw);
                }
                else
                {
                    _logger.LogError(string.Format("Error: status code {0}", response.StatusCode));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            return products;
        }
    }
}
