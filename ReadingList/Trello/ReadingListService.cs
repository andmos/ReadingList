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
		private ILog m_logger;


		public ReadingListService(string boardId, IBookParser bookParser, ILogFactory logFactory)
		{
			m_board = new Board(boardId);
			m_bookParser = bookParser;
			m_logger = logFactory.GetLogger(this.GetType());

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
			bool updateSuccessful; 
			string doneCardListId = m_board.Lists.FirstOrDefault(l => l.Name.Equals(TrelloBoardConstans.DoneReading)).Id;
			var doneCardList = new List(doneCardListId);
			try
			{
				var card = m_board.Lists.Where(l => l.Name.Equals(TrelloBoardConstans.CurrentlyReading)).FirstOrDefault().Cards.SingleOrDefault(c => c.Name.ToLower().Contains(book.ToLower()));
				if (card == null) 
				{
					m_logger.Info($"Could not find {book} in {TrelloBoardConstans.CurrentlyReading}, so can't move to {TrelloBoardConstans.DoneReading}.");
					return updateSuccessful = false;
				}
				card.List = doneCardList;
				card.Position = new Position(1);
				updateSuccessful = true;
			}
			catch (Exception ex)
			{
				m_logger.Error($"Error when trying to move {book} to {TrelloBoardConstans.DoneReading}: ", ex);
				updateSuccessful = false;
			}
			return updateSuccessful;

		}

		private string FormatCardName(string book, string author)
		{
			return $"{book} - {author}";
		}

	}
}
