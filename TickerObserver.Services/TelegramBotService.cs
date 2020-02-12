using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using TickerObserver.DomainModels;

namespace TickerObserver.Services
{
    public class TelegramBotService : ITelegramBotService
    {
        private readonly string _botApiKey;

        private readonly string _channelName;
        
        private readonly ILogger<TelegramBotService> _logger;
        
        public TelegramBotService(IConfiguration configuration, ILogger<TelegramBotService> logger)
        {
            _botApiKey = configuration["TelegramClient:BotApiKey"];
            _channelName = $"@{configuration["TelegramClient:PublicChannelName"]}";
            _logger = logger;
        }
        public async Task SendMessage(TickerTopic topic)
        {
            var message = BuildMessage(topic);
            
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
                        
                        _logger.LogInformation($"Message sent with response: {responseStr}");
                    }
                }
            }
        }

        private string BuildMessage(TickerTopic tickerTopic)
        {
            var message = tickerTopic.FullUrl
                          + Environment.NewLine
                          + tickerTopic.Title
                          + Environment.NewLine
                          + "#"
                          + tickerTopic.TickerName
                          + Environment.NewLine
                          + "#"
                          + tickerTopic.Source.Replace(" ", "");

            return message;
        }
    }
}