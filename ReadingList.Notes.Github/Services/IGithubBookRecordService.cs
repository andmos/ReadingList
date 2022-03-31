using System.Collections.Generic;
using System.Threading.Tasks;
using Readinglist.Notes.Logic.Models;

namespace ReadingList.Notes.Github.Services
{
    public interface IGithubBookRecordService
    {
        Task<IEnumerable<BookRecord>> GetAllBookRecords();
    }
}

