using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using TickerObserver.Core.Interfaces;
using TickerObserver.DataAccess.Interfaces;

namespace TickerObserver.DataAccess
{
    public class YahooTickerRepository : IYahooTickerRepository
    {
        private readonly HttpClient _httpClient;
        

        public YahooTickerRepository(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClient = httpClientFactory.GetInstanceByKey(nameof(YahooTickerRepository));
            _httpClient.BaseAddress = new Uri("https://www.nasdaq.com/");
        }

        public async Task<string> GetTicker(string tickerName)
        {
            try
            {
                var result = await _httpClient.GetStringAsync($"feed/rssoutbound?symbol={tickerName}");

                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}