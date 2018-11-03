using System;
using System.Collections.Generic;
using System.Linq;
using Manatee.Trello;

namespace ReadingList
{
	public class ReadingListService : IReadingListService
	{
		private readonly Board m_board;
		private IBookParser m_bookParser;


		public ReadingListService(string boardId, IBookParser bookParser)
		{
			m_board = new Board(boardId);
			m_bookParser = bookParser;

		}

		public IEnumerable<Book> GetReadingList(string listName, string label = null)
		{
			IEnumerable<Card> cardList;

			if (string.IsNullOrEmpty(label))
			{
				cardList = m_board.Lists.FirstOrDefault(l => l.Name.Equals(listName)).Cards;
			}
			else
			{
				cardList = m_board.Lists.FirstOrDefault(l => l.Name.Equals(listName)).Cards.Where(c => c.Labels.All(l => l.Name.ToLower().Equals(label.ToLower())));
			}

			var readingList = new List<Book>();

			foreach (var card in cardList)
			{
				readingList.Add(m_bookParser.ParseBook(card.Name, card.Labels.FirstOrDefault().Name.ToLower()));
			}

			return readingList;
		}

		public bool AddBookToBacklog(string book,string authors, string label)
		{
			string backlogCardListId = m_board.Lists.FirstOrDefault(l => l.Name.Equals(TrelloBoardConstans.Backlog)).Id;
			var backlogCardList = new List(backlogCardListId);
			try
			{
				var bookLabel = m_board.Labels.FirstOrDefault(l => l.Name.ToLower().Equals(label.ToLower()));
				backlogCardList.Cards.Add(name: FormatCardName(book, authors), labels: new[] { bookLabel});
				return true;
			}
			catch(Exception ex)
			{
				throw ex;
			}
		}

		public bool UpdateDoneListFromReadingList(string book)
		{
			string doneCardListId = m_board.Lists.FirstOrDefault(l => l.Name.Equals(TrelloBoardConstans.DoneReading)).Id;
			var doneCardList = new List(doneCardListId);
			try
			{
				var card = m_board.Cards.SingleOrDefault(c => c.Name.ToLower().Contains(book.ToLower()));
				card.List = doneCardList;
				card.Position = new Position(1);
				return true;
			}
			catch (Exception ex)
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
