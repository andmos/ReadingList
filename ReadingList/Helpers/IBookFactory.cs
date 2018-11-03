namespace ReadingList
{
	public interface IBookFactory
	{
		/// <summary>
		/// Create the Book object from specified bookString and listLabel.
		/// 
		/// </summary>
		/// <returns>The Book Object</returns>
		/// <param name="bookString">Book string. format: [BookTitle - Author1, Author2]</param>
		/// <param name="listLabel">List string.</param>
		Book Create(string bookString, string listLabel); 
	}
}
