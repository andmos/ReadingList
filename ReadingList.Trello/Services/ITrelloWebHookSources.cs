using System.Net;

namespace ReadingList.Trello.Services
{
    /// <summary>
    /// Trello web hook sources.
    /// </summary>
    public interface ITrelloWebHookSources
    {
        /// <summary>
        /// Validates if request IP is valid webhook source
        /// </summary>
        /// <returns>True if valid</returns>
        bool IsValidWebHookSource(IPAddress ip);
    }
}
