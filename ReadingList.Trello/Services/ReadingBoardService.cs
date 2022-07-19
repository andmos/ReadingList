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
            IEnumerable<string> listNames = new List<string>(_readingListBoard.Lists.Select(l => l.Name).ToList());
            var readingBoard = new ReadingListCollection { ReadingLists = new Dictionary<string, IEnumerable<Book>>() };

            foreach (var listName in listNames)
            {
                var list = await _readingListService.GetReadingList(listName, label);
                readingBoard.ReadingLists.Add(listName, list);
            }

            return readingBoard;
        }
    }
}
