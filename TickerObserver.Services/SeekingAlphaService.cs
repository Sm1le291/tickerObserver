using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.Toolkit.Parsers;
using Microsoft.Toolkit.Parsers.Rss;
using TickerObserver.DataAccess.Interfaces;
using TickerObserver.DomainModels;
using TickerObserver.Mappers;
using TickerObserver.Parsers;

namespace TickerObserver.Services
{
    public class SeekingAlphaService : ISeekingAlphaService
    {
        private readonly ISeekingAlphaTickerRepository _seekingAlphaTickerRepository;

        private readonly ISeekingAlphaRssParser _seekingAlphaRssParser;

        private readonly IParser<RssSchema> _rssParser;
        
        private readonly ITickerTopicMapper _tickerTopicMapper;
        
        public SeekingAlphaService(ISeekingAlphaTickerRepository seekingAlphaTickerRepository, IParser<RssSchema> rssParser, ISeekingAlphaRssParser seekingAlphaRssParser, ITickerTopicMapper tickerTopicMapper)
        {
            _seekingAlphaTickerRepository = seekingAlphaTickerRepository;
            _rssParser = rssParser;
            _seekingAlphaRssParser = seekingAlphaRssParser;
            _tickerTopicMapper = tickerTopicMapper;
        }


        public async Task<IEnumerable<TickerTopic>> GetTopicsByTicker(string tickerName)
        {
            Collection<TickerTopic> topics = new Collection<TickerTopic>();
            
            var feed = await _seekingAlphaTickerRepository.GetTicker(tickerName);
            
            if (feed != null)
            {
                var rss = _seekingAlphaRssParser.Parse(feed);

                foreach (var rssItem in rss)
                {
                    topics.Add(new TickerTopic
                    {
                        Source = rssItem.Source,
                        Title = rssItem.Title,
                        Description = rssItem.Description,
                        TickerName = tickerName,
                        FullUrl = rssItem.FullUrl,
                        PublishDate = rssItem.PublishDate
                    });
                }
            }
            
            return topics;
        }
    }
}