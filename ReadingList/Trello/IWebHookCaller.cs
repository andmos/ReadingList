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
	}
}
