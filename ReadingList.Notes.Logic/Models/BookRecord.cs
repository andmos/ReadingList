using System;
using System.Collections.Generic;

namespace Readinglist.Notes.Logic.Models
{
    public class BookRecord
    {
        public string Title { get; }
        public IEnumerable<string> Authors { get; }
        public IEnumerable<string> Notes { get; }
        public string RawFileName { get; }

        public BookRecord(string title, IEnumerable<string> authors, IEnumerable<string> notes, string rawFileName)
        {
            Title = title;
            Authors = authors;
            Notes = notes;
            RawFileName = rawFileName;
        }
    }
}

