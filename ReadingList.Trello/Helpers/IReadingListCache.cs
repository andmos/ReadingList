using System.Collections.Generic;
using ReadingList.Logic.Models;

namespace ReadingList.Trello.Helpers
{
	public interface IReadingListCache
    {
		void InvalidateCache();
		bool TryGetValue(KeyValuePair<string, string> listLabelPair, out IEnumerable<Book> listOut);
		bool TryAdd(KeyValuePair<string, string> listLabelPair, IEnumerable<Book> listOut);
    }
}
