using System;
using System.Collections.Generic;
using System.Net.Http;
using TickerObserver.Core.Interfaces;

namespace TickerObserver.Core
{
    /// <summary>
    /// Данный класс нужен для минимизации количества используемых HttpClient
    /// и избежания исчерпания количества доступных сокетов
    /// подробнее здесь https://docs.microsoft.com/en-us/dotnet/api/system.net.http.httpclient?view=netframework-4.8
    /// Так как у нас предполагаются запросы к разным доменам, то для каждого домена будет свой HttpClient
    /// </summary>
    public class HttpClientFactory : IHttpClientFactory
    {
        private readonly Dictionary<string, HttpClient> _httpClients;

        public HttpClientFactory(IList<string> instances)
        {
            _httpClients = new Dictionary<string, HttpClient>();
            
            foreach (var instance in instances)
            {
                _httpClients.Add(instance, new HttpClient());
            }
        }

        public HttpClient GetInstanceByKey(string instanceKey, bool newIfNotFound = true)
        {
            var instance = _httpClients[instanceKey];

            if (instance != null)
            {
                return instance;
            }

            if (newIfNotFound == true)
            {
                return new HttpClient();
            }
            
            throw new KeyNotFoundException();
        }
    }
}