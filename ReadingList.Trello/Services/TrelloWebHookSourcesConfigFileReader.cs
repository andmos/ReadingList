using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using ReadingList.Trello.Models;

namespace ReadingList.Trello.Services
{
    public class TrelloWebHookSourcesConfigFileReader : ITrelloWebHookSources
    {
        private readonly char SourcesDelimitorCharacter = ',';
        private readonly IConfiguration _configuration;

        public TrelloWebHookSourcesConfigFileReader(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IEnumerable<string> ValidWebhookSources() => _configuration["WebhookSources"].Split(SourcesDelimitorCharacter);

    }
}
