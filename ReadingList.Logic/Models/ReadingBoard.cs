using System.Collections.Generic;


namespace ReadingList.Logic.Models
{
    public class ReadingBoard
    {
        public IDictionary<string, IEnumerable<Book>> ReadingLists { get; set; }
    }
}