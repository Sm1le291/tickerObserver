using System.Collections.Generic;
using System.Threading.Tasks;
using TickerObserver.DomainModels;

namespace TickerObserver.Services
{
    public interface IYahooTickerService
    {
        Task<IEnumerable<TickerTopic>> GetTopicsByTicker(string tickerName);
    }
}