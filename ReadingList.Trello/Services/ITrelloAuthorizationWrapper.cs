using ReadingList.Trello.Models;

namespace ReadingList.Trello.Services
{
    /// <summary>
    /// Trello authorization wrapper - for setting up authorization against Trello.
    /// </summary>
    public interface ITrelloAuthorizationWrapper
    {
        bool IsValidKeys(ITrelloAuthModel authModel);
        void Authenticate();
    }
}
