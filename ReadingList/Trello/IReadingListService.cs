using System.Collections.Generic;

namespace ReadingList
{
	public interface IReadingListService
	{
		IEnumerable<Book> GetReadingList(string listName, string label = null);
		bool AddBookToBacklog(string book, string authors, string label);
	}
}