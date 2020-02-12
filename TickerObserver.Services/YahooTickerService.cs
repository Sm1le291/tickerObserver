using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Microsoft.Toolkit.Parsers;
using Microsoft.Toolkit.Parsers.Rss;
using TickerObserver.DataAccess.Interfaces;
using TickerObserver.DomainModels;

namespace TickerObserver.Services
{
    public class YahooTickerService : IYahooTickerService
    {
        private readonly IYahooTickerRepository _yahooTickerRepository;
        
        private readonly IParser<RssSchema> _rssParser;
        
        private readonly ITopicService _topicService;
        
        public YahooTickerService(
            IYahooTickerRepository yahooTickerRepository,
            IParser<RssSchema> rssParser,
            ITopicService topicService)
        {
            _yahooTickerRepository = yahooTickerRepository;
            _rssParser = rssParser;
            _topicService = topicService;
        }
        
        public async Task<IEnumerable<TickerTopic>> GetTopicsByTicker(string tickerName)
        {
            Collection<TickerTopic> topics = new Collection<TickerTopic>();
            var feed = await _yahooTickerRepository.GetTicker(tickerName);

            if (feed != null)
            {
                var rss = _rssParser.Parse(feed);

                foreach (var rssItem in rss)
                {
                    var topic = GetTickerTopic(rssItem, tickerName);
                    topics.Add(topic);

                    await _topicService.TrySaveTopic(topic, tickerName, "Yahoo");
                }
            }

            return topics;
        }
        
        private TickerObserver.DomainModels.TickerTopic GetTickerTopic(RssSchema rssItem, string tickerName)
        {
            return new TickerTopic
            {
                Source = rssItem.Author,
                Title = rssItem.Title,
                Description = rssItem.Content,
                TickerName = tickerName,
                FullUrl = rssItem.FeedUrl,
                PublishDate = rssItem.PublishDate,
                Guid = rssItem.InternalID
            };
        }
    }
}