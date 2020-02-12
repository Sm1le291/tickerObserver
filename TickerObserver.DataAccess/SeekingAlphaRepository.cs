using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using TickerObserver.Core.Interfaces;
using TickerObserver.DataAccess.Interfaces;

namespace TickerObserver.DataAccess
{
    public class SeekingAlphaRepository : ISeekingAlphaTickerRepository
    {
        private readonly HttpClient _httpClient;
        

        public SeekingAlphaRepository(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClient = httpClientFactory.GetInstanceByKey(nameof(SeekingAlphaRepository));
            _httpClient.BaseAddress = new Uri("http://seekingalpha.com/");
        }
        public async Task<string> GetTicker(string tickerName)
        {
            try
            {
                var result = await _httpClient.GetStringAsync($"api/sa/combined/{tickerName}");

                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}