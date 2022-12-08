using System.Collections.Generic;
using System.Threading.Tasks;
using Readinglist.Notes.Logic.Services;
using ReadingList.Logic.Models;
using ReadingList.Logic.Services;

namespace ReadingList.Carter.Helpers
{
    public class CacheWarmUpService
    {
        private readonly IReadingListService _readingListService;
        private readonly IBookNotesService _bookNotesService;

        public CacheWarmUpService(IReadingListService readingListService, IBookNotesService bookNotesService)
        {
            _readingListService = readingListService;
            _bookNotesService = bookNotesService;
        }

        public async Task WarmUpCaches()
        {
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
