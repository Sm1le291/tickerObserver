using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
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

        public async Task< bool> IsExists(string guid)
        {
            if (await _context.TickerTopics.AnyAsync(x => x.Guid == guid))
            {
                return true;
            }

            return false;
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

        public async Task<bool> IsSentAlready(string guid)
        {
            var item = await _context.TickerTopics.FirstOrDefaultAsync(x => x.Guid == guid);

            if (item != null && item.IsSentAlready)
            {
                return true;
            }

            return false;
        }
    }
}