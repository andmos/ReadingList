using ReadingList.Logic.Models;

namespace ReadingList.Logic.Services
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
        /// <summary>
        /// Create Book object from valid json String.
        /// </summary>
		/// <returns>The Book Object</returns>
        /// <param name="jsonString">Valid Json string.</param>
		Book Create(string jsonString);
    }
}
