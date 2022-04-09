using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ReadingList.Logging;
using ReadingList.Logic.Models;
using ReadingList.Logic.Services;
using ReadingList.Trello.Helpers;

namespace ReadingList.Trello.Services
{
    public class CachedReadingListService : IReadingListService
    {
        private readonly IReadingListService _readingListService;
        private readonly ILog _logger;
        private readonly IReadingListCache _readingListCache;

        public CachedReadingListService(
            IReadingListService readingListService,
            IReadingListCache readingListCache,
            ILogFactory logFactory)
        {
            _readingListService = readingListService;
            _logger = logFactory.GetLogger(this.GetType());
            _readingListCache = readingListCache;
        }

        public async Task<bool> AddBookToBacklog(string book, string authors, string label)
        {
            InvalidateCache();
            return await _readingListService.AddBookToBacklog(book, authors, label);
        }

        public async Task<IEnumerable<Book>> GetReadingList(string listName, string label = null)
        {
            IEnumerable<Book> books;
            if (_readingListCache.TryGetValue(new KeyValuePair<string, string>(listName, label), out books))
            {
                return !string.IsNullOrEmpty(label) ? books.Where(b => b.Label.ToString().ToLower().Equals(label.ToLower())) : books;
            }

            _logger.Info($"Cache miss for {listName}, {label}");
            books = await _readingListService.GetReadingList(listName, label);
            _readingListCache.TryAdd(new KeyValuePair<string, string>(listName, label), books);
            return books;

        }

        public async Task<bool> UpdateDoneListFromReadingList(string book)
        {
            InvalidateCache();
            return await _readingListService.UpdateDoneListFromReadingList(book);

        }

        public void InvalidateCache()
        {
            _logger.Info("Invalidating cache");
            _readingListCache.InvalidateCache();
        }
    }
}
