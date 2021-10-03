using System;
namespace ReadingList.Trello.Models
{
    public class TrelloIncident
    {
        public string Title { get; set; }
        public string Id { get; set; }
        public DateTimeOffset Published { get; set; }
        public DateTimeOffset Updated { get; set; }
        public string Content { get; set; }
        public bool Resolved => Content.ToLower().Contains("resolved");
    }
}
