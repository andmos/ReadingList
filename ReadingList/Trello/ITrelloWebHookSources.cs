using System;
using System.Collections.Generic;
namespace ReadingList
{
	public interface ITrelloWebHookSources
	{
		IEnumerable<string> ValidWebhookSources();
	}
}
