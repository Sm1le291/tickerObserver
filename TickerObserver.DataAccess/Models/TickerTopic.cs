using System;
using System.ComponentModel.DataAnnotations;

namespace TickerObserver.DataAccess.Models
{
    public class TickerTopic
    {
        [Key]
        public int TickerTopicId { get; set; }
        
        public bool IsSentAlready { get; set; }
        
        public string Guid { get; set; }
        
        public string RssFeedSource { get; set; }
        
        public string Source { get; set; }
        
        public string Title { get; set; }
        
        public string TickerName { get; set; }
        
        public string FullUrl { get; set; }
        
        public DateTime PublishDate { get; set; } 
        
        public string Description { get; set; }
    }
}