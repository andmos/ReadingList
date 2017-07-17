using System.Collections.Generic;

namespace ReadingList
{
	public class Book
	{
		public string Title { get; private set; }
		public IEnumerable<string> Authors { get; private set; }
		public string Label { get; private set;  }

		public Book(string title, IEnumerable<string> authors, string label)
		{
			Title = title;
			Authors = authors;
			Label = label; 
		}
	}
}
