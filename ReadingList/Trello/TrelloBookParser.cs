using System.Collections.Generic;
using System.Linq;

namespace ReadingList
{
	public class TrelloBookParser : IBookParser
	{
		private char m_bookTitleDelimitor => '-';
		private char m_authorsDelimitor => ',';

		public Book ParseBook(string bookString, string listLabel)
		{
			var bookArray = bookString.Split(m_bookTitleDelimitor);

			return new Book(bookArray[0].Trim(), ExtractAuthors(bookArray[1]), listLabel);
		}

		private IEnumerable<string> ExtractAuthors(string authors)
		{
			return authors.Split(m_authorsDelimitor).Select(author => author.Trim()).ToList();

		}
	}
}
