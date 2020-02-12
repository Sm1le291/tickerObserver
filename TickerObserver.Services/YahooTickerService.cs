using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Microsoft.Toolkit.Parsers;
using Microsoft.Toolkit.Parsers.Rss;
using TickerObserver.DataAccess.Interfaces;
using TickerObserver.DomainModels;
using TickerObserver.Mappers;

namespace TickerObserver.Services
{
    public class YahooTickerService : BaseTopicService, IYahooTickerService
    {
        private readonly IYahooTickerRepository _yahooTickerRepository;
        
        private readonly IParser<RssSchema> _rssParser;
        
        public YahooTickerService(
            IYahooTickerRepository yahooTickerRepository,
            IParser<RssSchema> rssParser,
            ITickerTopicMapper tickerTopicMapper,
            ITickerTopicRepository tickerTopicRepository) : base(tickerTopicRepository, tickerTopicMapper)
        {
            _yahooTickerRepository = yahooTickerRepository;
            _rssParser = rssParser;
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

                    await TrySaveTopic(topic, tickerName, "Yahoo");
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