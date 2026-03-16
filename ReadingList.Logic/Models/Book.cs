using System;
using System.Collections.Generic;

namespace ReadingList.Logic.Models
{
    public class Book
    {
        public string Title { get; }
        public IEnumerable<string> Authors { get; }
        public Label Label { get; }
        public DateTime? DateStartedReading { get; }
        public DateTime? DateFinishedReading { get; }

        public Book(string title, IEnumerable<string> authors, Label label,
            DateTime? dateStartedReading = null, DateTime? dateFinishedReading = null)
        {
            Title = title;
            Authors = authors;
            Label = label;
            DateStartedReading = dateStartedReading;
            DateFinishedReading = dateFinishedReading;
        }
    }
    public enum Label
    {
        Fact,
        Fiction,
        Unspecified
    }
}
