using System;
using System.Collections.Generic;

namespace Readinglist.Notes.Logic.Models
{
    public class BookRecord
    {
        public string Title { get; }
        public IEnumerable<string> Authors { get; }
        public IEnumerable<string> Notes { get; }

        public BookRecord(string bookTitle, IEnumerable<string> authors, IEnumerable<string> notes)
        {
            Title = bookTitle;
            Authors = authors;
            Notes = notes;
        }
    }
}

