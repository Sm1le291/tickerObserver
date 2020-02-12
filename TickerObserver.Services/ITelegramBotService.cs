using System.Threading.Tasks;

namespace TickerObserver.Services
{
    public interface ITelegramBotService
    {
        Task SendMessage(string message);
    }
}