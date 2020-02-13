using System.Collections.Generic;
using System.Configuration;
using ReadingList.Trello.Models;

namespace ReadingList.Web.Trello
{
	public class TrelloWebHookSourcesConfigFileReader : ITrelloWebHookSources
	{
		private readonly char SourcesDelimitorCharacter = ',';

		public IEnumerable<string> ValidWebhookSources() => ConfigurationManager.AppSettings.Get("WebhookSources").Split(SourcesDelimitorCharacter);

	}
}
