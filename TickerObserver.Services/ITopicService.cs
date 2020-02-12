using System.Threading.Tasks;

namespace TickerObserver.Services
{
    public interface ITopicService
    {
        Task MarkAsSent(string guid);

        Task<bool> IsSentAlready(string guid);

        Task TrySaveTopic(DomainModels.TickerTopic rssItem, string tickerName, string feedSource);
    }
}