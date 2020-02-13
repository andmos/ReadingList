using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Manatee.Trello;
using ReadingList.Models;

namespace ReadingList.Trello
{
	public class ReadingBoardService : IReadingBoardService
	{
		private readonly IReadingListService m_readingListService;
		private readonly IBoard m_readingListBoard; 

		public ReadingBoardService(ITrelloFactory factory, IReadingListService readingListService, string boardId)
		{
			m_readingListService = readingListService;
			m_readingListBoard = factory.Board(boardId);
		}

		public async Task<ReadingBoard> GetAllReadingLists(string label)
		{
			await m_readingListBoard.Lists.Refresh();
			IEnumerable<string> listNames = new List<string>(m_readingListBoard.Lists.Select(l => l.Name).ToList());
	        var readingBoard = new ReadingBoard { ReadingLists = new Dictionary<string, IEnumerable<Book>>() };

			foreach (var listName in listNames)
			{
				var list = await m_readingListService.GetReadingList(listName, label);
				readingBoard.ReadingLists.Add(listName, list);
			}

			return readingBoard;
		}
	}
}
