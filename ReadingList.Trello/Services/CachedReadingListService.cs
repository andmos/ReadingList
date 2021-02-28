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

        // abstraher ut selve cache, med ICacheSomething -> GetOrAdd og Invalidate

		private readonly IReadingListService m_readingListService;
		private readonly ILog m_logger;
		private readonly IReadingListCache m_readingListCache;

		public CachedReadingListService(
			IReadingListService readingListService, 
			IReadingListCache readingListCache, 
			ILogFactory logFactory)
		{
			m_readingListService = readingListService;
			m_logger = logFactory.GetLogger(this.GetType());
			m_readingListCache = readingListCache;
		}

		public async Task<bool> AddBookToBacklog(string book, string authors, string label)
		{
			InvalidateCache();
			return await m_readingListService.AddBookToBacklog(book, authors, label);
		}

		public async Task<IEnumerable<Book>> GetReadingList(string listName, string label = null)
		{
			IEnumerable<Book> books;
			if (m_readingListCache.TryGetValue(new KeyValuePair<string, string>(listName, label), out books))
			{
				return !string.IsNullOrEmpty(label) ? books.Where(b => b.Label.ToLower().Equals(label.ToLower())) : books;
			}

			m_logger.Info($"Cache miss for {listName}, {label}");
			books = await m_readingListService.GetReadingList(listName, label);
			m_readingListCache.TryAdd(new KeyValuePair<string, string>(listName, label), books);
			return books; 

		}

		public async Task<bool> UpdateDoneListFromReadingList(string book)
		{
            InvalidateCache();
			return await m_readingListService.UpdateDoneListFromReadingList(book); 

		}

		public void InvalidateCache() 
		{
			m_logger.Info("Invalidating cache");
			m_readingListCache.InvalidateCache();
		}
	}
}
