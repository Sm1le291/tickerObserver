using TickerTopicDataAccess = TickerObserver.DataAccess.Models.TickerTopic;
namespace TickerObserver.Mappers
{
    public interface ITickerTopicMapper
    {
        TickerTopicDataAccess MapFromDomainModel(DomainModels.TickerTopic rssItem, string tickerName, string feedSource);
    }
}