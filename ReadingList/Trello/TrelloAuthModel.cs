using System.Configuration;

namespace ReadingList
{
	public class TrelloAuthModel : ITrelloAuthModel
	{

		public TrelloAuthModel(string apiKey, string userToken) 
		{
			TrelloAPIKey = apiKey;
			TrelloUserToken = userToken;
		}

		public string TrelloAPIKey { get; }

		public string TrelloUserToken { get; }

	}
}
