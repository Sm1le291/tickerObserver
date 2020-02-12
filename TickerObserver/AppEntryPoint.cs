using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using TickerObserver.DomainModels;
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
        private readonly ITopicService _topicService;
        public AppEntryPoint(
            IConfiguration configuration,
            ISeekingAlphaService seekingAlphaService,
            IYahooTickerService yahooTickerService,
            ILoggerFactory loggerFactory,
            ITelegramBotService telegramBotService,
            ITopicService topicService)
        {
            _seekingAlphaService = seekingAlphaService;
            _yahooTickerService = yahooTickerService;
            _telegramBotService = telegramBotService;
            _topicService = topicService;
            _configuration = configuration;
            _logger = loggerFactory.CreateLogger<AppEntryPoint>();
        }

        public async Task Execute()
        {
            

            var pauseBetweenAttempts = int.Parse(_configuration["TickersFeed:RefreshTimeSeconds"]) * 1000;
            var rawTickers = _configuration["TickersFeed:Tickers"];

            string[] tickers;
            if(string.IsNullOrWhiteSpace(rawTickers))
            {
                return;
            }

            tickers = rawTickers.Split(',');


            Console.WriteLine("Press ESC to stop");
            do
            {
                while (!Console.KeyAvailable)
                {
                    foreach (var ticker in tickers)
                    {
                        _logger.LogInformation($"Getting topics from SeekingAlpha for ticker: {ticker}");
                        var seekingAlphaTickers = await _seekingAlphaService.GetTopicsByTicker(ticker);
                        _logger.LogInformation($"Getting topics from Yahoo for ticker: {ticker}");
                        var yahooTickerTopics = await _yahooTickerService.GetTopicsByTicker(ticker);
                    
                        foreach (var seekingAlphaTicker in seekingAlphaTickers)
                        {
                            _logger.LogInformation($"Processing topic from SeekingAlpha with GUID(NOT LINK!): {seekingAlphaTicker.Guid}");
                            await ProcessTopic(seekingAlphaTicker);
                        }

                        foreach (var yahooTickerTopic in yahooTickerTopics)
                        {
                            _logger.LogInformation($"Processing topic from Yahoo with GUID(NOT LINK!): {yahooTickerTopic.Guid}");
                            await ProcessTopic(yahooTickerTopic);
                        }
                    
                        _logger.LogInformation($"Pause before next attempt: {pauseBetweenAttempts} milliseconds");
                    }
                    
                    await Task.Delay(pauseBetweenAttempts);
                }
                
            } while (Console.ReadKey(true).Key != ConsoleKey.Escape);
        }

        private async Task ProcessTopic(TickerTopic tickerTopic)
        {
            var isSent = await _topicService.IsSentAlready(tickerTopic.Guid);

            if (!isSent)
            {
                _logger.LogInformation($"Sending topic with GUID: {tickerTopic.Guid} to chat");
                await _telegramBotService.SendMessage(tickerTopic);
                await _topicService.MarkAsSent(tickerTopic.Guid);
                _logger.LogDebug("Message sent");
            }
            else
            {
                _logger.LogInformation($"Topic with GUID : {tickerTopic.Guid}  has been sent already");
            }
        }
    }
}