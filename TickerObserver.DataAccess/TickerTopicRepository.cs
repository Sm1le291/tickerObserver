using System;
using System.Linq;
using System.Threading.Tasks;
using TickerObserver.DataAccess.Interfaces;
using TickerObserver.DataAccess.Models;

namespace TickerObserver.DataAccess
{
    public class TickerTopicRepository : ITickerTopicRepository
    {
        private readonly TickerObserverDbContext _context;
        public TickerTopicRepository(TickerObserverDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task Add(TickerTopic tickerTopic)
        {
            _context.Add(tickerTopic);
            await _context.SaveChangesAsync();
        }
        
        public async Task MarkAsSent(string guid)
        {
            var item = _context.TickerTopics.FirstOrDefault(x => x.Guid == guid);

            if (item != null)
            {
                item.IsSentAlready = true;
                _context.TickerTopics.Update(item);
                await _context.SaveChangesAsync();
            }
        }
    }
}