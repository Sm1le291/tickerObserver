using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace TickerObserver.Services
{
    public class TelegramBotService : ITelegramBotService
    {
        private readonly string _botApiKey;

        private readonly string _channelName;
        
        public TelegramBotService(IConfiguration configuration)
        {
            _botApiKey = configuration["TelegramClient:BotApiKey"];
            _channelName = $"@{configuration["TelegramClient:PublicChannelName"]}";
        }
        public async Task SendMessage(string message)
        {
            string urlString = $"https://api.telegram.org/bot{_botApiKey}/sendMessage?chat_id={_channelName}&text={message}";
            
            var webRequest = WebRequest.Create(urlString);

            using (var response = await webRequest.GetResponseAsync())
            {
                using (var responseStream = response.GetResponseStream())
                {
                    using (var sr = new StreamReader(responseStream))
                    {
                        string line = "";
                        StringBuilder sb = new StringBuilder();
                        while (line != null)
                        {
                            line = await sr.ReadLineAsync();
                            if (line != null)
                                sb.Append(line);
                        }
                        
                        string responseStr = sb.ToString();
                    }
                }
            }
        }
    }
}