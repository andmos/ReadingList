namespace ReadingList.Trello.Models
{
    /// <summary>
    /// DTO for setting up Trello Webhook
    /// </summary>
    public class TrelloWebhook
    {
        public string description { get; set; }
        public string callbackURL { get; set; }
        public string idModel { get; set; }
    }
}
