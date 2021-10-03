using System;
using System.Collections.Generic;
using ReadingList.Trello.Models;

namespace ReadingList.Trello.Services
{
    public interface ITrelloStatusService
    {
        IEnumerable<TrelloIncident> GetStatusIncidents();   
    }
}
