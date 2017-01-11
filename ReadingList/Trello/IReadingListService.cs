using System.Collections.Generic;

namespace ReadingList
{
	public interface IReadingListService
	{
		IEnumerable<string> GetReadingList(string listName);
	}
}