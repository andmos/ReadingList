using System;
using Manatee.Trello;
using Manatee.Trello.ManateeJson;
using Manatee.Trello.WebApi;

namespace ReadingList
{
	public class TrelloAuthorizationWrapper : ITrelloAuthorizationWrapper
	{
		private string ApiKey { get; set; }
		private string UserToken { get; set; }

		public TrelloAuthorizationWrapper(string apiKey, string userToken)
		{
			if (string.IsNullOrEmpty(apiKey) || string.IsNullOrEmpty(userToken)) 
			{
				throw new ArgumentNullException(); 
			}
		}

		public void Authorize() 
		{
			var serializer = new ManateeSerializer();
			TrelloConfiguration.Serializer = serializer;
			TrelloConfiguration.Deserializer = serializer;
			TrelloConfiguration.JsonFactory = new ManateeFactory();
			TrelloConfiguration.RestClientProvider = new WebApiClientProvider();
			TrelloAuthorization.Default.AppKey = ApiKey;
			TrelloAuthorization.Default.UserToken = UserToken;
		}
	
	}
}
