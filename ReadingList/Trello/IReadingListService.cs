using System.Collections.Generic;

namespace ReadingList
{
	public interface IReadingListService
	{
		/// <summary>
		/// Gets the reading list.
		/// </summary>
		/// <returns>The reading list.</returns>
		/// <param name="listName">List name.</param>
		/// <param name="label">Label.</param>
		IEnumerable<Book> GetReadingList(string listName, string label = null);
		/// <summary>
		/// Adds the book to backlog.
		/// </summary>
		/// <returns><c>true</c>, if book to backlog was added, <c>false</c> otherwise.</returns>
		/// <param name="book">Book.</param>
		/// <param name="authors">Authors.</param>
		/// <param name="label">Label.</param>
		bool AddBookToBacklog(string book, string authors, string label);


		bool UpdateDoneList(string book);
	}
}