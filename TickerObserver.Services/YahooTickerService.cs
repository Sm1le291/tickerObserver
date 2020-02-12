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
    public class YahooTickerService : IYahooTickerService
    {
        private readonly IYahooTickerRepository _yahooTickerRepository;
        
        private readonly IParser<RssSchema> _rssParser;

        private readonly ITickerTopicMapper _tickerTopicMapper;
        
        public YahooTickerService(IYahooTickerRepository yahooTickerRepository, IParser<RssSchema> rssParser, ITickerTopicMapper tickerTopicMapper)
        {
            _yahooTickerRepository = yahooTickerRepository;
            _rssParser = rssParser;
            _tickerTopicMapper = tickerTopicMapper;
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
                    topics.Add(new TickerTopic
                    {
                        Source = rssItem.Author,
                        Title = rssItem.Title,
                        Description = rssItem.Content,
                        TickerName = tickerName,
                        FullUrl = rssItem.FeedUrl,
                        PublishDate = rssItem.PublishDate,
                        Guid = rssItem.InternalID
                    });
                }
            }

            return topics;
        }
    }
}