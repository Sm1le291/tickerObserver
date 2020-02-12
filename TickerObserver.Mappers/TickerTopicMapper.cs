using TickerTopicDataAccess = TickerObserver.DataAccess.Models.TickerTopic;

namespace TickerObserver.Mappers
{
    public class TickerTopicMapper : ITickerTopicMapper
    {
        public TickerTopicDataAccess MapFromDomainModel(DomainModels.TickerTopic rssItem, string tickerName, string feedSource)
        {
            return new TickerTopicDataAccess()
            {
                Source = rssItem.Source,
                Title = rssItem.Title,
                FullUrl = rssItem.FullUrl,
                Guid = rssItem.Guid,
                TickerName = tickerName,
                RssFeedSource = feedSource,
                PublishDate = rssItem.PublishDate,
                Description = rssItem.Description
            };
        }
    }
}