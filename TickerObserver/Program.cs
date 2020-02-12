using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Toolkit.Parsers;
using Microsoft.Toolkit.Parsers.Rss;
using TickerObserver.Core;
using TickerObserver.Core.Interfaces;
using TickerObserver.DataAccess;
using TickerObserver.DataAccess.Interfaces;
using TickerObserver.Mappers;
using TickerObserver.Parsers;
using TickerObserver.Services;

namespace TickerObserver
{
    class Program
    {
        static readonly HttpClient HttpClient = new HttpClient();
        
        
        static async Task Main(string[] args)
        {
            IConfiguration configuration = GetConfiguration(args);

            var serviceProvider = new ServiceCollection()
                .AddLogging()
                .AddSingleton<IHttpClientFactory>(h => new HttpClientFactory(GetRepositories()))
                .AddScoped<IYahooTickerRepository, YahooTickerRepository>()
                .AddScoped<ITickerTopicMapper, TickerTopicMapper>()
                .AddScoped<ITopicService, TopicService>()
                .AddScoped<ISeekingAlphaTickerRepository, SeekingAlphaRepository>()
                .AddScoped<ISeekingAlphaService, SeekingAlphaService>()
                .AddScoped<ITickerTopicRepository, TickerTopicRepository>()
                .AddScoped<ITelegramBotService, TelegramBotService>()
                .AddScoped<IYahooTickerService, YahooTickerService>()
                .AddScoped<ISeekingAlphaRssParser, SeekingAlphaRssParser>()
                .AddScoped<IParser<RssSchema>, RssParser>()
                .AddSingleton<HttpClient>(i => HttpClient)
                .AddTransient<AppEntryPoint>()
                .AddTransient<IConfiguration>(i => configuration)
                .AddDbContext<TickerObserverDbContext>()
                .BuildServiceProvider();

            serviceProvider
                .GetService<ILoggerFactory>()
                .AddConsole(LogLevel.Debug);
            
            

            var logger = serviceProvider.GetService<ILoggerFactory>()
                .CreateLogger<Program>();
            
            logger.LogDebug("Starting application");
            
            await serviceProvider.GetService<AppEntryPoint>().Execute();
            
            logger.LogDebug("Done!");
        }

        private static IConfiguration GetConfiguration(string[] args)
        {
            return new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddCommandLine(args)
                .Build();
        }

        private static IList<string> GetRepositories()
        {
            return new List<string>
            {
                nameof(YahooTickerRepository),
                nameof(SeekingAlphaRepository)
            };
        }
    }
}