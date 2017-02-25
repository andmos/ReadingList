﻿using System;
using System.Collections.Generic;
using System.Linq;
using Manatee.Trello;

namespace ReadingList
{
	public class ReadingListService : IReadingListService
	{
		private string BoardId;
		private string ListId;
		private Board BooksBoard; 
		private IBookParser m_bookParser; 

		public ReadingListService(string boardId, string listId, IBookParser bookParser)
		{
			BoardId = boardId;
			ListId = listId;
			BooksBoard = new Board(boardId);
			m_bookParser = bookParser; 

		}

		public IEnumerable<Book> GetReadingList(string listName, string label = null)
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

			var readingList = new List<Book>();

			foreach (var card in cardList)
			{
				readingList.Add(m_bookParser.ParseBook(card.Name));
			}

			return readingList;
		}

		public bool AddBookToBacklog(string book,string authors, string label)
		{
			var readingListTable = new List(ListId);
			string backlogCardListId = readingListTable.Board.Lists.FirstOrDefault(l => l.Name.Equals(TrelloBoardConstans.Backlog)).Id;
			var backlogCardList = new List(backlogCardListId);
			try
			{
				var bookLabel = readingListTable.Board.Labels.FirstOrDefault(l => l.Name.ToLower().Equals(label.ToLower()));
				backlogCardList.Cards.Add(name: FormatCardName(book, authors), labels: new[] { bookLabel});
				return true;
			}
			catch(Exception ex)
			{
				throw ex;
			}
		}

		public bool UpdateDoneList(string book)
		{
			var readingListTable = new List(ListId);
			string doneCardListId = readingListTable.Board.Lists.FirstOrDefault(l => l.Name.Equals(TrelloBoardConstans.DoneReading)).Id;
			var doneCardList = new List(doneCardListId);
			try
			{
				var card = readingListTable.Cards.SingleOrDefault(c => c.Name.ToLower().Contains(book.ToLower()));
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
