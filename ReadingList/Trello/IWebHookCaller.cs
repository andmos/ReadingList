using System.Threading.Tasks;

namespace ReadingList
{
	/// <summary>
	/// Web hook caller.
	/// </summary>
	public interface IWebHookCaller
	{
		/// <summary>
		/// Sets up web hook, fire and forget style
		/// </summary>
		/// <returns>The up web hook.</returns>
		/// <param name="webHook">Web hook.</param>
		Task SetUpWebHook(TrelloWebhook webHook);
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="T:ReadingList.IWebHookCaller"/> is configured.
        /// </summary>
        /// <value><c>true</c> if configured; otherwise, <c>false</c>.</value>
        bool Configured { get; set; }
    }
}
