using System;
using System.Collections.Generic;
using ReadingList.Models;

namespace ReadingList.Helpers
{
	public interface IReadingListCache
    {
		void InvalidateCache();
		bool TryGetValue(KeyValuePair<string, string> listLabelPair, out IEnumerable<Book> listOut);
		bool TryAdd(KeyValuePair<string, string> listLabelPair, IEnumerable<Book> listOut);
    }
}
