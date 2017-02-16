using System;
using System.Threading.Tasks;

namespace ReadingList
{
	public interface IWebHookCaller
	{
		Task<string> SetUpWebHook(TrelloWebhook webHook, string apiKey, string token);
	}
}
