using System.Threading.Tasks;
using TickerObserver.DomainModels;

namespace TickerObserver.Services
{
    public interface ITelegramBotService
    {
        Task SendMessage(TickerTopic topic);
    }
}