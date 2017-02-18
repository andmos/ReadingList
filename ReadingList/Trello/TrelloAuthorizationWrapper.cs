using System;
using Manatee.Trello;
using Manatee.Trello.ManateeJson;
using Manatee.Trello.WebApi;

namespace ReadingList
{
	public class TrelloAuthorizationWrapper : ITrelloAuthorizationWrapper
	{

		public TrelloAuthorizationWrapper(ITrelloAuthModel authModel)
		{
			if (string.IsNullOrEmpty(authModel.TrelloAPIKey) || string.IsNullOrEmpty(authModel.TrelloUserToken)) 
			{
				throw new ArgumentNullException(); 
			}
			var serializer = new ManateeSerializer();
			TrelloConfiguration.Serializer = serializer;
			TrelloConfiguration.Deserializer = serializer;
			TrelloConfiguration.JsonFactory = new ManateeFactory();
			TrelloConfiguration.RestClientProvider = new WebApiClientProvider();
			TrelloAuthorization.Default.AppKey = authModel.TrelloAPIKey;
			TrelloAuthorization.Default.UserToken = authModel.TrelloUserToken;
		}

	}
}
