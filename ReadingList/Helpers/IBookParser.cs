namespace ReadingList
{
	public interface IBookParser
	{
		/// <summary>
		/// Parses the book string.
		/// </summary>
		/// <returns>The book object.</returns>
		/// <param name="bookString">Book string.</param>
		Book ParseBook(string bookString); 
	}
}
