using System;
namespace ReadingList
{
	public interface ITrelloAuthModel
	{
		string TrelloAPIKey { get; }
		string TrelloUserToken { get; }
	}
}
