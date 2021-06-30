using System.Collections.Generic;


namespace ReadingList.Logic.Models
{
    public class ReadingListCollection
    {
        public IDictionary<string, IEnumerable<Book>> ReadingLists { get; set; }
    }
}