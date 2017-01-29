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

		public IEnumerable<string> GetReadingList(string listName, string label = null)
		{
			var readingListTable = new List(ListId);
			IEnumerable<Card> cardList;

			if (string.IsNullOrEmpty(label))
			{
				cardList = readingListTable.Board.Lists.FirstOrDefault(l => l.Name.Equals(listName)).Cards;
			}
			else 
			{
				cardList = readingListTable.Board.Lists.FirstOrDefault(l => l.Name.Equals(listName)).Cards.Where(c => c.Labels.All(l => l.Name.ToLower().Equals(label.ToLower())));
			}

			var readingList = new List<string>();

			foreach (var card in cardList) 
			{
				readingList.Add(card.Name);
			}

			return readingList;
		}

		public bool AddBookToBacklog(string book,string author, string label)
		{
			var readingListTable = new List(ListId);
			string backlogCardListId = readingListTable.Board.Lists.FirstOrDefault(l => l.Name.Equals(TrelloBoardConstans.Backlog)).Id;
			var backlogCardList = new List(backlogCardListId);
			try
			{
				var bookLabel = readingListTable.Board.Labels.FirstOrDefault(l => l.Name.ToLower().Equals(label.ToLower()));
				backlogCardList.Cards.Add(name: FormatCardName(book, author), labels: new[] { bookLabel});
				return true;
			}

			catch(Exception ex) 
			{
				throw ex;
			}

		}

		private string FormatCardName(string book, string author) 
		{
			return $"{book} - {author}";
		}
	}
}
