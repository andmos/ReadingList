using System;
using System.Collections.Generic;
using System.Linq;
using Manatee.Trello;

namespace ReadingList
{
	public class ReadingBoardService : IReadingBoardService
	{
		private readonly IReadingListService m_readingListService;
		private readonly string BoardId;

		public ReadingBoardService(IReadingListService readingListService, string boardId)
		{
			m_readingListService = readingListService;
			BoardId = boardId;
		}

		public ReadingBoard GetAllReadingLists()
		{
			var readingListBoard = new Board(BoardId);
			IEnumerable<string> listNames = new List<string>(readingListBoard.Lists.Select(l => l.Name).ToList());
	        var readingBoard = new ReadingBoard { ReadingLists = new Dictionary<string, IEnumerable<Book>>() };

			foreach (var listName in listNames)
			{
				readingBoard.ReadingLists.Add(listName, m_readingListService.GetReadingList(listName));
			}

			return readingBoard;
		}
	}
}
