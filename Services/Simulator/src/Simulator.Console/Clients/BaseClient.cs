using Serilog;
using Simulator.Console.Extensions;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Simulator.Console.Client
{
    public abstract class BaseClient
    {
        private readonly HttpClient _httpClient;

        public BaseClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        protected virtual async Task<TResult> GetAsync<TResult>(string baseUrl, Dictionary<string, string> queryCollection = null)
        {
            var urlBuilder = new StringBuilder(baseUrl);
            if (queryCollection != null)
                urlBuilder.Append(queryCollection.ToQueryString());

            var url = urlBuilder.ToString();

            try
            {
                var result = await _httpClient.GetAsync(url);

                result.EnsureSuccessStatusCode();

                var response = await result.Content.ReadAsStringAsync();

                return JsonSerializer.Deserialize<TResult>(response, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            catch (Exception e)
            {
                Log.Error($"Error on consume {_httpClient.BaseAddress.ToString() + url}. Error {e.Message}");
                return default;
            }

        }

    }
}
