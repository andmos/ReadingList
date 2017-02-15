using System;
using System.Collections.Generic;
using System.Configuration;

namespace ReadingList
{
	public class TrelloWebHookSourcesConfigFileReader : ITrelloWebHookSources
	{

		private readonly char SourcesDelimitorCharacter = ',';

		public IEnumerable<string> ValidWebhookSources()
		{
			return ConfigurationManager.AppSettings.Get("WebhookSources").Split(SourcesDelimitorCharacter);
		}


	}
}
