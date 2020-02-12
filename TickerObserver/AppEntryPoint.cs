using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using TickerObserver.DataAccess.Interfaces;
using TickerObserver.Services;

namespace TickerObserver
{
    public class AppEntryPoint
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<AppEntryPoint> _logger;
        private readonly ISeekingAlphaService _seekingAlphaService;
        private readonly IYahooTickerService _yahooTickerService;
        private readonly ITelegramBotService _telegramBotService; 
        public AppEntryPoint(ISeekingAlphaService seekingAlphaService, IYahooTickerService yahooTickerService, ILoggerFactory loggerFactory, ITelegramBotService telegramBotService)
        {
            _seekingAlphaService = seekingAlphaService;
            _yahooTickerService = yahooTickerService;
            _telegramBotService = telegramBotService;
            _logger = loggerFactory.CreateLogger<AppEntryPoint>();
        }

        public async Task Execute()
        {
            var result = await _seekingAlphaService.GetTopicsByTicker("ABEO");
            var sdas = await _yahooTickerService.GetTopicsByTicker("ABEO");
            
            await _telegramBotService.SendMessage("Hello Igor again");
            _logger.LogDebug("Hello from AppEntryPoint");
            
        }
    }
}