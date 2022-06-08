using System.Collections.Generic;
using Readinglist.Notes.Logic.Models;
using Readinglist.Notes.Logic.Services;
using System.Threading.Tasks;
using ReadingList.Notes.Github.Services;

namespace ReadingList.Notes.Github.Repositories
{
    public class ReadingListDataRepository : IBookRecordRepository
    {

        private readonly IGithubBookRecordService _githubBookRecordService;

        public ReadingListDataRepository(IGithubBookRecordService githubBookRecordService)
        {
            _githubBookRecordService = githubBookRecordService;
        }

        public async Task<IEnumerable<BookRecord>> GetAllBookRecords()
        {
            return await _githubBookRecordService.GetAllBookRecords();
        }

        public Task<BookRecord> GetBookNotes(string book)
        {
            return _githubBookRecordService.GetBookNotes(book);
        }
    }
}

