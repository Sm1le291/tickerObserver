using System.Threading.Tasks;
using TickerObserver.DataAccess.Models;

namespace TickerObserver.DataAccess.Interfaces
{
    public interface ITickerTopicRepository
    {
        Task Add(TickerTopic tickerTopic);

        Task<bool> IsExists(string guid);

        Task MarkAsSent(string guid);
        
        Task<bool> IsSentAlready(string guid);
    }
}