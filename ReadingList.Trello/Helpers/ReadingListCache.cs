using System.Collections.Concurrent;
using System.Collections.Generic;
using ReadingList.Logic.Models;

namespace ReadingList.Trello.Helpers
{
    public class ReadingListCache : IReadingListCache
    {
        private readonly ConcurrentDictionary<KeyValuePair<string, string>, IEnumerable<Book>> _cache;

        public ReadingListCache()
        {
            _cache = new ConcurrentDictionary<KeyValuePair<string, string>, IEnumerable<Book>>();
        }

        public bool TryGetValue(KeyValuePair<string, string> listLabelPair, out IEnumerable<Book> bookListOut)
        {
            return _cache.TryGetValue(listLabelPair, out bookListOut);
        }

        public bool TryAdd(KeyValuePair<string, string> listLabelPair, IEnumerable<Book> listOut)
        {
            return _cache.TryAdd(listLabelPair, listOut);
        }

        public void InvalidateCache()
        {
            _cache.Clear();
        }
    }
}
