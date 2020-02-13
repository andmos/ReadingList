using System.Collections.Generic;
using System.Configuration;

namespace ReadingList.Trello
{
	public class TrelloWebHookSourcesConfigFileReader : ITrelloWebHookSources
	{
		private readonly char SourcesDelimitorCharacter = ',';

		public IEnumerable<string> ValidWebhookSources() => ConfigurationManager.AppSettings.Get("WebhookSources").Split(SourcesDelimitorCharacter);

	}
}
