using System.Collections.Generic;

namespace ReadingList.Logic.Models
{
    public class Book
    {
        public string Title { get; }
        public IEnumerable<string> Authors { get; }
        public Label Label { get; }
        public Book(string title, IEnumerable<string> authors, Label label)
        {
            Title = title;
            Authors = authors;
            Label = label;
        }
    }
    public enum Label
    {
        Fact,
        Fiction,
        None
    }
}
