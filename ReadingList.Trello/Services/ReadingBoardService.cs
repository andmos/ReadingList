using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Manatee.Trello;
using ReadingList.Logic.Models;
using ReadingList.Logic.Services;

namespace ReadingList.Trello.Services
{
    public class ReadingBoardService : IReadingBoardService
    {
        private readonly IReadingListService _readingListService;
        private readonly IBoard _readingListBoard;

        public ReadingBoardService(
            ITrelloFactory factory,
            IReadingListService readingListService,
            string boardId)
        {
            _readingListService = readingListService;
            _readingListBoard = factory.Board(boardId);
        }

        public async Task<ReadingBoard> GetAllReadingLists(string label)
        {
            await _readingListBoard.Lists.Refresh();
            IEnumerable<string> listNames = new List<string>(_readingListBoard.Lists.Select(l => l.Name).ToList());
            var readingBoard = new ReadingBoard { ReadingLists = new Dictionary<string, IEnumerable<Book>>() };

            foreach (var listName in listNames)
            {
                var list = await _readingListService.GetReadingList(listName, label);
                readingBoard.ReadingLists.Add(listName, list);
            }

            return readingBoard;
        }
    }
}
