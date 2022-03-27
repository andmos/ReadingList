using System.Collections.Generic;

namespace Readinglist.Notes.Logic.Models
{
	public class BookNote
	{
        public string Title { get; }
        public IEnumerable<string> Authors { get; }
        public string Note { get; }

        public BookNote(string bookTitle, IEnumerable<string> authors, string note)
        {
            Title = bookTitle;
            Authors = authors;
            Note = note;
        }
    }
}

