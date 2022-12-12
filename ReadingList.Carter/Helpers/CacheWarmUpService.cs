using System.Collections.Generic;
using System.Threading.Tasks;
using Readinglist.Notes.Logic.Services;
using ReadingList.Logging;
using ReadingList.Logic.Models;
using ReadingList.Logic.Services;

namespace ReadingList.Carter.Helpers
{
    public class CacheWarmUpService
    {
        private readonly IReadingListService _readingListService;
        private readonly IBookNotesService _bookNotesService;
        private readonly ILog _logger;

        public CacheWarmUpService(IReadingListService readingListService, 
            IBookNotesService bookNotesService,
            ILogFactory logFactory)
        {
            _readingListService = readingListService;
            _bookNotesService = bookNotesService;
            _logger = logFactory.GetLogger(this.GetType());
        }

        public async Task WarmUpCaches()
        {
            _logger.Info("Starting cache warmup");
            var cacheTasks = new List<Task> 
            {
                _bookNotesService.GetAllBookNotes(),
                _readingListService.GetReadingList(ReadingListConstants.Backlog),
                _readingListService.GetReadingList(ReadingListConstants.CurrentlyReading),
                _readingListService.GetReadingList(ReadingListConstants.DoneReading)
            };
            
            await Task.WhenAll(cacheTasks);
        }
    }
}
