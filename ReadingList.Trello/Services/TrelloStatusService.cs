using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Xml;
using ReadingList.Trello.Models;

namespace ReadingList.Trello.Services
{
    public class TrelloStatusService : ITrelloStatusService
    {
        private const string TrelloStatusFeedUrl = @"https://trellostatus.com/history.atom";

        public IEnumerable<TrelloIncident> GetStatusIncidents()
        {
            using var reader = XmlReader.Create(TrelloStatusFeedUrl);
            var feed = SyndicationFeed.Load(reader);

            return feed.Items.Select(MapToIncident);
        }

        private static TrelloIncident MapToIncident(SyndicationItem syndicationItem)
        {
            TextSyndicationContent content = (TextSyndicationContent)syndicationItem.Content;
            return new TrelloIncident
            {
                Title = syndicationItem.Title.Text,
                Content = content.Text,
                Published = syndicationItem.PublishDate,
                Updated = syndicationItem.LastUpdatedTime
            };
        }
    }
}
