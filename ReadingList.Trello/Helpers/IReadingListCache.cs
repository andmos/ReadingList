using System.Collections.Generic;
using ReadingList.Logic.Models;

namespace ReadingList.Trello.Helpers
{
	public interface IReadingListCache
    {
		void InvalidateCache();
		bool TryGetValue(KeyValuePair<string, Label> listLabelPair, out IEnumerable<Book> bookListOut);
		bool TryAdd(KeyValuePair<string, Label> listLabelPair, IEnumerable<Book> books);
    }
}
