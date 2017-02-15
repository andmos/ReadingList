using System;
namespace ReadingList
{
	/// <summary>
	/// DTO for setting up Trello Webhook
	/// </summary>
	public class TrelloWebhook
	{
		public string description { get; set; }
		public string callBackURL { get; set; }
		public string idModel { get; set; }
	}
}
