using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using ReadingList.Trello.Models;

namespace ReadingList.Carter.Trello
{
    public class TrelloWebHookSourcesConfigFileReader : ITrelloWebHookSources
    {
        private readonly char SourcesDelimitorCharacter = ',';
        private readonly IConfiguration m_configuration;

        public TrelloWebHookSourcesConfigFileReader(IConfiguration configuration)
        {
            m_configuration = configuration;
        }

        public IEnumerable<string> ValidWebhookSources() => m_configuration["WebhookSources"].Split(SourcesDelimitorCharacter);

    }
}
