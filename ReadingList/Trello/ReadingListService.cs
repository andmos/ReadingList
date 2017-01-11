using System;
using System.Collections.Generic;
using System.Linq;
using Manatee.Trello;

namespace ReadingList
{
	public class ReadingListService : IReadingListService
	{
		private string BoardId { get; set; }
		private string ListId { get; set; }
		private Board BooksBoard { get; set; }

		public ReadingListService(string boardId, string listId)
		{
			BoardId = boardId;
			ListId = listId;
			BooksBoard = new Board(boardId);
		}

		public IEnumerable<string> GetReadingList(string listName)
		{
			var readingListTable = new List(ListId);
			var cardList = readingListTable.Board.Lists.FirstOrDefault(l => l.Name.Equals(listName)).Cards;

			var readingList = new List<string>();

			foreach (var card in cardList) 
			{
				readingList.Add(card.Name);
			}

			return readingList;
		}

	}
}
