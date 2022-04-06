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
            _logger.Info($"GetAllBookRecords: Call took {stopWatch.ElapsedMilliseconds} Ms");
            return allBookRecords;
        }
    }
}