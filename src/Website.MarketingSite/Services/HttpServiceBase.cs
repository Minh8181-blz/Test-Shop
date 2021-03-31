using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Website.MarketingSite.Services
{
    public class HttpServiceBase
    {
        public HttpClient Client { get; }

        public HttpServiceBase(HttpClient client)
        {
            Client = client;
            Client.DefaultRequestHeaders.Add("Accept", "application/json");
        }        

        protected virtual HttpRequestMessage InitRequest(HttpMethod method, string path, Dictionary<string, string> queries = null)
        {
            ValidateRequest(path);

            string requestUri = path;

            if (queries != null)
            {
                var listQuery = queries.Select(s => string.Format("{0}={1}", s.Key, s.Value));
                var queryString = string.Join("&", listQuery);
                requestUri = string.Format("{0}?{1}", requestUri, queryString);
            }

            HttpRequestMessage request = new HttpRequestMessage(method, requestUri);

            return request;
        }

        protected virtual void SetRequestBody(HttpRequestMessage requestMessage, object obj)
        {
            if (obj != null)
            {
                var serialized = JsonConvert.SerializeObject(obj, new JsonSerializerSettings
                {
                    ContractResolver = new DefaultContractResolver
                    {
                        NamingStrategy = new CamelCaseNamingStrategy()
                    }
                });
                requestMessage.Content = new StringContent(serialized, Encoding.UTF8, "application/json");
            }
        }

        protected async Task<HttpResponseMessage> GetAsync(string path, Dictionary<string, string> queries = null)
        {
            var requestMessage = InitRequest(HttpMethod.Get, path, queries);
            return await Client.SendAsync(requestMessage);
        }

        protected async Task<HttpResponseMessage> PostAsync(string path, object obj, Dictionary<string, string> queries = null)
        {
            var requestMessage = InitRequest(HttpMethod.Post, path, queries);
            SetRequestBody(requestMessage, obj);
            return await Client.SendAsync(requestMessage);
        }

        protected async Task<HttpResponseMessage> PutAsync(string path, object obj, Dictionary<string, string> queries = null)
        {
            var requestMessage = InitRequest(HttpMethod.Put, path, queries);
            SetRequestBody(requestMessage, obj);
            return await Client.SendAsync(requestMessage);
        }

        protected async Task<HttpResponseMessage> DeleteAsync(string path, object obj = null, Dictionary<string, string> queries = null)
        {
            var requestMessage = InitRequest(HttpMethod.Delete, path, queries);
            SetRequestBody(requestMessage, obj);
            return await Client.SendAsync(requestMessage);
        }

        protected void ValidateRequest(string path)
        {
            if (path == null)
            {
                throw new Exception("uri is null");
            }
        }
    }
}
