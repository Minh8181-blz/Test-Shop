using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Service.CommunicationStandard.Const;
using Service.CommunicationStandard.Models;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Website.MarketingSite.Configurations;
using Website.MarketingSite.Models.Dtos;
using Website.MarketingSite.Models.ViewModels;

namespace Website.MarketingSite.Services
{
    public class AuthService : HttpServiceBase
    {
        private readonly ILogger<AuthService> _logger;
        private readonly ApiEndpointConfiguration _endpointConfiguration;

        public AuthService(HttpClient client, ApiEndpointConfiguration endpointConfiguration, ILogger<AuthService> logger) : base(client)
        {
            _endpointConfiguration = endpointConfiguration;

            Client.BaseAddress = new Uri(_endpointConfiguration.IdentityOrigin);

            _logger = logger;
        }

        public async Task<SignUpResultDto> SignUp(SignUpViewModel model)
        {
            var result = new SignUpResultDto();

            try
            {
                var response = await PostAsync(_endpointConfiguration.GetIdentityToken, model);

                if (response.StatusCode != HttpStatusCode.InternalServerError)
                {
                    var raw = await response.Content.ReadAsStringAsync();
                    var apiResult = JsonConvert.DeserializeObject<ActionResultModel>(raw);

                    if (apiResult.Code == ActionCode.Created)
                    {
                        result.Succeeded = true;
                    }
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

            return result;
        }
    }
}
