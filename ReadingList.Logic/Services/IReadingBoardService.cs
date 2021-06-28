using System.Threading.Tasks;
using ReadingList.Logic.Models;

namespace ReadingList.Logic.Services
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
