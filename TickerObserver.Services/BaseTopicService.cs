using System.Threading.Tasks;
using TickerObserver.DataAccess.Interfaces;
using TickerObserver.DomainModels;
using TickerObserver.Mappers;

namespace TickerObserver.Services
{
    public class BaseTopicService
    {
        private readonly ITickerTopicRepository _tickerTopicRepository;
        
        private readonly ITickerTopicMapper _tickerTopicMapper;
        public BaseTopicService(ITickerTopicRepository tickerTopicRepository, ITickerTopicMapper tickerTopicMapper)
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
    }
}