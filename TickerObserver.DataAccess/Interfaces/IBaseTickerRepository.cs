using System.Threading.Tasks;

namespace TickerObserver.DataAccess.Interfaces
{
    public interface IBaseTickerRepository
    {
        Task<string> GetTicker(string tickerName);
    }
}