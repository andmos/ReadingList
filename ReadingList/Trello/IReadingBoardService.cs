using System.Threading.Tasks;

namespace ReadingList.Trello
{
	public interface IReadingBoardService
	{
		/// <summary>
		/// Gets all reading lists.
		/// </summary>
		/// <returns>All reading lists.</returns>
		Task<ReadingBoard> GetAllReadingLists(string label);
	}
}
