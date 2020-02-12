using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Xml.Linq;
using TickerObserver.Core;
using TickerObserver.DomainModels;

namespace TickerObserver.Parsers
{
    public class SeekingAlphaRssParser : ISeekingAlphaRssParser
    {
        private readonly XNamespace _nsSaNamespaceUri = "https://seekingalpha.com/api/1.0";
        
        public IEnumerable<SeekingAlphaRssItem> Parse(string data)
        {
            if (string.IsNullOrEmpty(data))
            {
                return null;
            }
            
            var doc = XDocument.Parse(data);
            
            Collection<SeekingAlphaRssItem> collection = new Collection<SeekingAlphaRssItem>();
            
            var xNamespace = doc.Root.GetDefaultNamespace();

            foreach (XElement itemElement in doc.Descendants(xNamespace + "item"))
            {
                collection.Add(ParseItem(itemElement));
            }

            return collection;
        }

        private SeekingAlphaRssItem ParseItem(XElement item)
        {
            var rssItem = new SeekingAlphaRssItem()
            {
                Title = item.GetSafeElementString("title").Trim(),
                Source = item.GetSafeElementString("author_name", _nsSaNamespaceUri).Trim(),
                PublishDate = item.GetSafeElementDate("pubDate"),
                FullUrl = item.GetSafeElementString("link").Trim(),
                Description = GetDescription(item),
                GUID = item.GetSafeElementString("guid").Trim()
            };

            return rssItem;
        }

        private string GetDescription(XElement item)
        {
            var description = item.GetSafeElementString("description").Trim();

            if (string.IsNullOrEmpty(description))
            {
                return "No description";
            }

            return description;
        }
    }
}