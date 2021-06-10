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
    public class QueueSdk
    {
        private readonly ILogger _logger;
        private readonly HttpClient _client;


        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="serviceUrl"></param>
        public QueueSdk(
              ILogger logger
            , string serviceUrl)
        {
            _logger = logger;
            _client = HttpClientHelper.Client(serviceUrl);
        }


        /// <summary>
        /// Queue lead for procesing.
        /// </summary>
        /// <param name="queueModel"></param>
        /// <returns></returns>
        public async Task<string> CreateLog(QueueModel queueModel)
        {
            try
            {
                var content = new StringContent(JsonConvert.SerializeObject(queueModel), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await _client.PostAsync("/v1/queue", content);
                response.EnsureSuccessStatusCode();
                var httpLogResponse = JsonConvert.DeserializeObject<ApiWrapper<string>>(await response.Content.ReadAsStringAsync());
                return httpLogResponse.Data;
            }
            catch (Exception e)
            {
                _logger.LogError("{e}", e);
                throw;
            }
        }
    }
}