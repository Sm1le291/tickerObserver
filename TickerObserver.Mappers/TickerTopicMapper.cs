using TickerTopicDataAccess = TickerObserver.DataAccess.Models.TickerTopic;

namespace TickerObserver.Mappers
{
    public class TickerTopicMapper : ITickerTopicMapper
    {
        public TickerTopicDataAccess MapFromDomainModel(DomainModels.TickerTopic rssItem, string tickerName, string guid, string feedSource)
        {
            return new TickerTopicDataAccess()
            {
                Source = rssItem.Source,
                Title = rssItem.Title,
                FullUrl = rssItem.FullUrl,
                Guid = guid,
                TickerName = tickerName,
                RssFeedSource = feedSource,
                PublishDate = rssItem.PublishDate,
                Description = rssItem.Description
            };
        }
    }
}