using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Leads.Domain.Contracts.v1;
using Leads.Domain.Models.v1;
using Newtonsoft.Json;
using Leads.Library.Helpers.v1;

namespace Leads.Library.Sdks.v1
{
    public class HttpLogSdk : IDisposable
    {
        private readonly ILogger _logger;
        private readonly HttpClient _client;


        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="serviceUrl"></param>
        public HttpLogSdk(
              ILogger logger
            , string serviceUrl)
        {
            _logger = logger;
            _client = HttpClientHelper.Client(serviceUrl);
        }


        /// <summary>
        /// Create new http log.
        /// </summary>
        /// <param name="httpLogModel"></param>
        /// <returns></returns>
        public async Task<HttpLogModel> CreateLog(HttpLogModel httpLogModel)
        {
            try
            {
                var content = new StringContent(JsonConvert.SerializeObject(httpLogModel), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await _client.PostAsync("/v1/httplog", content);
                response.EnsureSuccessStatusCode();
                var httpLogResponse = JsonConvert.DeserializeObject<ApiWrapper<HttpLogModel>>(await response.Content.ReadAsStringAsync());
                return httpLogResponse.Data;
            }
            catch (Exception e)
            {
                _logger.LogError("{e}", e);
                throw;
            }
        }


        /// <summary>
        /// Update http log.
        /// </summary>
        /// <param name="httpLogModel"></param>
        /// <returns></returns>
        public async Task UpdateLog(HttpLogModel httpLogModel)
        {
            try
            {
                var content = new StringContent(JsonConvert.SerializeObject(httpLogModel), Encoding.UTF8, "application/json");
                await _client.PutAsync($"/v1/httplog/{httpLogModel.LogId}", content);
            }
            catch (Exception e)
            {
                _logger.LogError("{e}", e);
                throw;
            }
        }


        /// <summary>
        /// Garbage cleanup.
        /// </summary>
        public void Dispose()
        {
            _client?.Dispose();
        }
    }
}