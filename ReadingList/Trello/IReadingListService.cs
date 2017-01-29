using System.Collections.Generic;

namespace ReadingList
{
	public interface IReadingListService
	{
		IEnumerable<string> GetReadingList(string listName, string label = null);
		bool AddBookToBacklog(string book, string author, string label);
	}
}