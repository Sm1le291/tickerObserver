using System.Collections.Generic;
using TickerObserver.DomainModels;

namespace TickerObserver.Parsers
{
    public interface ISeekingAlphaRssParser
    {
        IEnumerable<SeekingAlphaRssItem> Parse(string data);
    }
}