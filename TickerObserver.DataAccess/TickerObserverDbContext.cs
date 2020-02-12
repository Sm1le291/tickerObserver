using Microsoft.EntityFrameworkCore;
using TickerObserver.DataAccess.Models;

namespace TickerObserver.DataAccess
{
    public class TickerObserverDbContext : DbContext
    {
        public DbSet<TickerTopic> TickerTopics { get; set; }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=tickerObserver.db");
        }
    }
}