using System.Collections.Concurrent;
using System.Collections.Generic;
using ReadingList.Logic.Models;

namespace ReadingList.Trello.Helpers
{
    public class ReadingListCache : IReadingListCache
    {
        private readonly ConcurrentDictionary<KeyValuePair<string, Label>, IEnumerable<Book>> _cache;

        public ReadingListCache()
        {
            _cache = new ConcurrentDictionary<KeyValuePair<string, Label>, IEnumerable<Book>>();
        }

        public bool TryGetValue(KeyValuePair<string, Label> listLabelPair, out IEnumerable<Book> bookListOut)
        {
            return _cache.TryGetValue(listLabelPair, out bookListOut);
        }

        public bool TryAdd(KeyValuePair<string, Label> listLabelPair, IEnumerable<Book> listOut)
        {
            return _cache.TryAdd(listLabelPair, listOut);
        }

        public void InvalidateCache()
        {
            _cache.Clear();
        }
    }
}
