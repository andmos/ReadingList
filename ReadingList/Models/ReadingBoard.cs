using System.Collections.Generic;

namespace ReadingList
{
	public class ReadingBoard
	{
		public IDictionary<string, IEnumerable<Book>> ReadingLists { get; set; }
	}
}
