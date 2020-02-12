using System.Net.Http;

namespace TickerObserver.Core.Interfaces
{
    public interface IHttpClientFactory
    {
        HttpClient GetInstanceByKey(string instanceKey, bool newIfNotFound = true);
    }
}