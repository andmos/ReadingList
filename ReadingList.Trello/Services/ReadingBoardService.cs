using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Manatee.Trello;
using ReadingList.Logic.Models;
using ReadingList.Logic.Services;
using ReadingList.Trello.Models;

namespace ReadingList.Trello.Services
{
    public class ReadingBoardService : IReadingListCollectionService
    {
        private readonly IReadingListService _readingListService;
        private readonly IBoard _readingListBoard;

        public ReadingBoardService(
            ITrelloFactory factory,
            IReadingListService readingListService)
        {
            _readingListService = readingListService;
            _readingListBoard = factory.Board(TrelloBoardConstants.BoardId);
        }

        public async Task<ReadingListCollection> GetAllReadingLists(string label)
        {
            await _readingListBoard.Lists.Refresh();
            var listNames = _readingListBoard.Lists.Select(l => l.Name).ToList();
            
            var tasks = listNames.Select(async listName => 
                (listName, books: await _readingListService.GetReadingList(listName, label)));
            var results = await Task.WhenAll(tasks);
            
            var readingBoard = new ReadingListCollection 
            { 
                ReadingLists = results.ToDictionary(r => r.listName, r => r.books) 
            };

            return readingBoard;
        }
    }
}
