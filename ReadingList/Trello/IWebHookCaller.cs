using System;
using System.Threading.Tasks;

namespace ReadingList
{
	public interface IWebHookCaller
	{
		Task SetUpWebHook(TrelloWebhook webHook, string apiKey, string token);
	}
}
