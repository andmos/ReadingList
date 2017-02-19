using System;
namespace ReadingList
{
	/// <summary>
	/// Trello auth model.
	/// </summary>
	public interface ITrelloAuthModel
	{
		/// <summary>
		/// Gets the trello API Key.
		/// </summary>
		/// <value>The trello API Key.</value>
		string TrelloAPIKey { get; }

		/// <summary>
		/// Gets the trello user token.
		/// </summary>
		/// <value>The trello user token.</value>
		string TrelloUserToken { get; }
	}
}
