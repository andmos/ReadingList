using System.Threading.Tasks;
using ReadingList.Logic.Models;

namespace ReadingList.Logic.Services
{
    public interface IReadingListCollectionService
    {
        /// <summary>
        /// Gets all reading lists.
        /// </summary>
        /// <returns>All reading lists.</returns>
        Task<ReadingListCollection> GetAllReadingLists(string label);
    }
}
