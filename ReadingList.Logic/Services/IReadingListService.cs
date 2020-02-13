using System.Collections.Generic;
using System.Threading.Tasks;
using ReadingList.Logic.Models;

namespace ReadingList.Logic.Services
{
	public interface IReadingListService
	{
		/// <summary>
		/// Gets the reading list.
		/// </summary>
		/// <returns>The reading list.</returns>
		/// <param name="listName">List name.</param>
		/// <param name="label">Label.</param>
		Task<IEnumerable<Book>> GetReadingList(string listName, string label = null);
		/// <summary>
		/// Adds the book to backlog.
		/// </summary>
		/// <returns><c>true</c>, if book to backlog was added, <c>false</c> otherwise.</returns>
		/// <param name="book">Book.</param>
		/// <param name="authors">Authors.</param>
		/// <param name="label">Label.</param>
		bool AddBookToBacklog(string book, string authors, string label);

		/// <summary>
		/// Update the done list with a book that is read.
		/// </summary>
		/// <returns><c>true</c>, if done list was updated, <c>false</c> otherwise.</returns>
		/// <param name="book">Book.</param>
		bool UpdateDoneListFromReadingList(string book);

}
}