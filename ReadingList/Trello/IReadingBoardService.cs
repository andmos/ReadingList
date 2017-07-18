using System;
namespace ReadingList
{
	public interface IReadingBoardService
	{
		/// <summary>
		/// Gets all reading lists.
		/// </summary>
		/// <returns>All reading lists.</returns>
		ReadingBoard GetAllReadingLists();
	}
}
