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
    public class SeekingAlphaService : BaseTopicService, ISeekingAlphaService
    {
        private readonly ISeekingAlphaTickerRepository _seekingAlphaTickerRepository;

        private readonly ISeekingAlphaRssParser _seekingAlphaRssParser;
        
        public SeekingAlphaService(
            ISeekingAlphaTickerRepository seekingAlphaTickerRepository,
            ISeekingAlphaRssParser seekingAlphaRssParser,
            ITickerTopicMapper tickerTopicMapper,
            ITickerTopicRepository tickerTopicRepository) : base(tickerTopicRepository, tickerTopicMapper)
        {
            _seekingAlphaTickerRepository = seekingAlphaTickerRepository;
            _seekingAlphaRssParser = seekingAlphaRssParser;
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
                    var topic = GetTickerTopic(rssItem, tickerName);
                    topics.Add(topic);

                    await TrySaveTopic(topic, tickerName, "SeekingAlpha");
                }
            }
            
            return topics;
        }

        private TickerObserver.DomainModels.TickerTopic GetTickerTopic(SeekingAlphaRssItem rssItem, string tickerName)
        {
            return new TickerTopic
            {
                Source = rssItem.Source,
                Title = rssItem.Title,
                Description = rssItem.Description,
                TickerName = tickerName,
                FullUrl = rssItem.FullUrl,
                PublishDate = rssItem.PublishDate,
                Guid = rssItem.GUID
            };
        }
    }
}