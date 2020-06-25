using System;
using Manatee.Trello;
using ReadingList.Trello.Models;

namespace ReadingList.Trello.Services
{
	public class TrelloAuthorizationWrapper : ITrelloAuthorizationWrapper
	{

		private readonly ITrelloAuthModel _authModel;

		public TrelloAuthorizationWrapper(ITrelloAuthModel authModel)
		{
			if (string.IsNullOrEmpty(authModel.TrelloAPIKey) || string.IsNullOrEmpty(authModel.TrelloUserToken)) 
			{
				throw new ArgumentNullException(); 
			}

			_authModel = authModel;

			TrelloAuthorization.Default.AppKey = _authModel.TrelloAPIKey;
			TrelloAuthorization.Default.UserToken = _authModel.TrelloUserToken;
		}

		public bool IsValidKeys(ITrelloAuthModel authModel)
		{
			return (authModel.TrelloAPIKey.Equals(_authModel.TrelloAPIKey) && authModel.TrelloUserToken.Equals(_authModel.TrelloUserToken));
		}
    }
}
