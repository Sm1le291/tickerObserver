using System;

namespace TickerObserver.DomainModels
{
    public class SeekingAlphaRssItem
    {
        public string Source { get; set; }
        public string Title { get; set; }
        
        public string Description { get; set; }
        
        public string FullUrl { get; set; }
        
        public DateTime PublishDate { get; set; }
        
        public  string GUID { get; set; }
    }
}