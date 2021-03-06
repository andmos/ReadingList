using System.Collections.Generic;

namespace ReadingList.Trello.Models
{
    /// <summary>
    /// Trello web hook sources.
    /// </summary>
    public interface ITrelloWebHookSources
    {
        /// <summary>
        /// Valid webhook sources.
        /// </summary>
        /// <returns>The valid webhook sources.</returns>
        IEnumerable<string> ValidWebhookSources();
    }
}
