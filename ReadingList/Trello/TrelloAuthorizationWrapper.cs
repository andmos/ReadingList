using System;
using Manatee.Trello;
using Manatee.Trello.ManateeJson;
using Manatee.Trello.WebApi;

namespace ReadingList
{
	public class TrelloAuthorizationWrapper : ITrelloAuthorizationWrapper
	{

		public TrelloAuthorizationWrapper(string apiKey, string userToken)
		{
			if (string.IsNullOrEmpty(apiKey) || string.IsNullOrEmpty(userToken)) 
			{
				throw new ArgumentNullException(); 
			}
			var serializer = new ManateeSerializer();
			TrelloConfiguration.Serializer = serializer;
			TrelloConfiguration.Deserializer = serializer;
			TrelloConfiguration.JsonFactory = new ManateeFactory();
			TrelloConfiguration.RestClientProvider = new WebApiClientProvider();
			TrelloAuthorization.Default.AppKey = apiKey;
			TrelloAuthorization.Default.UserToken = userToken;
		}

	}
}
