using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using ReadingList.Logging;
using ReadingList.Logic.Models;
using ReadingList.Logic.Services;

namespace ReadingList.Trello.Helpers
{
    public class ReadingListServiceProfiler : IReadingListService
    {
        private IReadingListService _readingListService;
        private readonly ILog _logger;
        public ReadingListServiceProfiler(IReadingListService readingListService, ILogFactory logFactory)
        {
            _readingListService = readingListService;
            _logger = logFactory.GetLogger(GetType());
        }

        public async Task<bool> AddBookToBacklog(string book, string authors, string label)
        {
            var stopWatch = Stopwatch.StartNew();
            var addResult = await _readingListService.AddBookToBacklog(book, authors, label);
            stopWatch.Stop();
            _logger.Info($"AddBookToBacklig: Call took {stopWatch.ElapsedMilliseconds} Ms");
            return addResult;
        }

        public async Task<IEnumerable<Book>> GetReadingList(string listName, string label = null)
        {
            var stopWatch = Stopwatch.StartNew();
            var list = await _readingListService.GetReadingList(listName, label);
            stopWatch.Stop();
            _logger.Info($"GetReadingList: Call for list {listName} took {stopWatch.ElapsedMilliseconds} Ms");
            return list;
        }

        public async Task<bool> UpdateDoneListFromReadingList(string book)
        {
            var stopWatch = Stopwatch.StartNew();
            var result = await _readingListService.UpdateDoneListFromReadingList(book);
            stopWatch.Stop();
            _logger.Info($"UpdateDoneList: Call took {stopWatch.ElapsedMilliseconds}");
            return result;
        }
    }
}
