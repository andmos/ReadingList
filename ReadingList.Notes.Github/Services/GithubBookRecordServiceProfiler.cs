using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using ReadingList.Logging;
using Readinglist.Notes.Logic.Models;

namespace ReadingList.Notes.Github.Services
{
    public class GithubBookRecordServiceProfiler : IGithubBookRecordService
    {
        private readonly IGithubBookRecordService _githubBookRecordService;
        private readonly ILog _logger;
        
        public GithubBookRecordServiceProfiler(IGithubBookRecordService githubBookRecordService, ILogFactory logFactory)
        {
            _githubBookRecordService = githubBookRecordService;
            _logger = logFactory.GetLogger(GetType());
        }
        public async Task<IEnumerable<BookRecord>> GetAllBookRecords()
        {
            var stopWatch = Stopwatch.StartNew();
            var allBookRecords = await _githubBookRecordService.GetAllBookRecords();
            stopWatch.Stop();
            _logger.Info($"{nameof(GetAllBookRecords)}: took {stopWatch.ElapsedMilliseconds} ms");
            return allBookRecords;
        }

        public async Task<BookRecord> GetBookNotes(string book)
        {
            var stopWatch = Stopwatch.StartNew();
            var bookNotes = await _githubBookRecordService.GetBookNotes(book);
            stopWatch.Stop();
            _logger.Info($"{nameof(GetBookNotes)}: took {stopWatch.ElapsedMilliseconds} ms");
            return bookNotes;
        }
    }
}