using System.Collections.Generic;
using System.Threading.Tasks;
using TickerObserver.DomainModels;

namespace TickerObserver.Services
{
    public interface ISeekingAlphaService
    {
        Task<IEnumerable<TickerTopic>> GetTopicsByTicker(string tickerName);
    }
}