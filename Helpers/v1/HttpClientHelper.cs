using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Leads.Library.Helpers.v1
{
    internal static class HttpClientHelper
    {
        private static HttpClient client;

        internal static HttpClient Client(string serviceUrl)
        {
            if (client is null)
            {
                client = new HttpClient();
                client.BaseAddress = new Uri(serviceUrl.TrimEnd('/'));
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            }

            return client;
        }
    }
}