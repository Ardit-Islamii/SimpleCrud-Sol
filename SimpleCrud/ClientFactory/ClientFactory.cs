using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Refit;

namespace OrderService.ClientFactory
{
    public class ClientFactory<T> : IClientFactory<T> where T : class
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ClientFactory(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public Task<T> CreateClient(string uri)
        {
            var newHttpClient = _httpClientFactory.CreateClient();
            newHttpClient.BaseAddress = new Uri(uri);

            var client = RestService.For<T>(newHttpClient, new RefitSettings()
            {
                    ContentSerializer = new SystemTextJsonContentSerializer()
            });

            return Task.FromResult(client);
        }
    }
}
