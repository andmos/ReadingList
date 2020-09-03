namespace ReadingList.Trello.Models
{
	public class TrelloAuthSettings : ITrelloAuthModel
	{
		public string TrelloAPIKey { get; set; }

		public string TrelloUserToken { get; set; }

	}
}
