using NetTools;
using ReadingList.Trello.Models;
using System.Net;

namespace ReadingList.Trello.Services
{
    // Valid Trello Sources: https://developer.atlassian.com/cloud/trello/guides/rest-api/webhooks/#webhook-sources
    public class TrelloWebHookSourcesService : ITrelloWebHookSources
    {

        private readonly IPAddressRange _validTrelloIPRange;
        private readonly IPAddressRange _localRange;
        private readonly IPAddressRange _localIPv6Range;
        public TrelloWebHookSourcesService()
        {
          _validTrelloIPRange = IPAddressRange.Parse("18.234.32.224/28");
          _localRange = IPAddressRange.Parse("127.0.0.1");
          _localIPv6Range = IPAddressRange.Parse("::/0");
        }

        public bool IsValidWebHookSource(IPAddress ip) => 
            (_validTrelloIPRange.Contains(ip) ||
              _localRange.Contains(ip) ||
              _localIPv6Range.Contains(ip));
    }
}
