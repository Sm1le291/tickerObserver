using System.Threading.Tasks;
using TickerObserver.DataAccess.Interfaces;
using TickerObserver.Mappers;

namespace TickerObserver.Services
{
    public class TopicService : ITopicService
    {
        private readonly ITickerTopicRepository _tickerTopicRepository;
        
        private readonly ITickerTopicMapper _tickerTopicMapper;
        public TopicService(ITickerTopicRepository tickerTopicRepository, ITickerTopicMapper tickerTopicMapper)
        {
            _tickerTopicRepository = tickerTopicRepository;
            _tickerTopicMapper = tickerTopicMapper;
        }

        public async Task TrySaveTopic(DomainModels.TickerTopic rssItem, string tickerName, string feedSource)
        {
            if (!await _tickerTopicRepository.IsExists(rssItem.Guid))
            {
                var item = _tickerTopicMapper.MapFromDomainModel(rssItem, tickerName, feedSource);
                await _tickerTopicRepository.Add(item);
            }
        }

        public async Task MarkAsSent(string guid)
        {
            await _tickerTopicRepository.MarkAsSent(guid);
        }
        
        public async Task<bool> IsSentAlready(string guid)
        {
            return await _tickerTopicRepository.IsSentAlready(guid);
        }
    }
}