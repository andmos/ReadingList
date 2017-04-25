using System.Configuration;

namespace ReadingList
{
	public class TrelloAuthModel : ITrelloAuthModel
	{

		public string TrelloAPIKey => ConfigurationManager.AppSettings["TrelloAPIKey"];

		public string TrelloUserToken => ConfigurationManager.AppSettings["TrelloUserToken"];

	}
}
