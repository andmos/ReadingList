using System.Collections.Generic;
using ReadingList.Models;

namespace ReadingList
{
	public class ReadingBoard
	{
		public IDictionary<string, IEnumerable<Book>> ReadingLists { get; set; }
	}
}
