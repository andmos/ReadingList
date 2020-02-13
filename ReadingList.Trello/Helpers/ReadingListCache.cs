using System.Collections.Concurrent;
using System.Collections.Generic;
using ReadingList.Logic.Models;

namespace ReadingList.Trello.Helpers
{
	public class ReadingListCache : IReadingListCache
    {
		private readonly ConcurrentDictionary<KeyValuePair<string, string>, IEnumerable<Book>> m_cache;

        public ReadingListCache()
        {
			m_cache = new ConcurrentDictionary<KeyValuePair<string, string>, IEnumerable<Book>>();
		}

		public bool TryGetValue(KeyValuePair<string, string> listLabelPair, out IEnumerable<Book> listOut)
		{
			return m_cache.TryGetValue(listLabelPair, out listOut);
		}

		public bool TryAdd(KeyValuePair<string, string> listLabelPair, IEnumerable<Book> listOut)
		{
			return m_cache.TryAdd(listLabelPair, listOut);
		}

		public void InvalidateCache()
        {
			m_cache.Clear();
        }
	}
}
