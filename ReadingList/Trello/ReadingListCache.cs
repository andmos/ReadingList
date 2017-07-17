using System;
using System.Linq; 
using System.Collections.Generic;
using System.Collections.Concurrent;

namespace ReadingList
{
	public class ReadingListCache : IReadingListService
	{
		private readonly IReadingListService m_readingListService;
		private readonly ConcurrentDictionary<KeyValuePair<string, string>, IEnumerable<Book>> m_cache;
		private readonly ILog m_logger; 

		public ReadingListCache(IReadingListService readingListService, ILogFactory logFactory)
		{
			m_readingListService = readingListService;
			m_logger = logFactory.GetLogger(this.GetType());
			m_cache = new ConcurrentDictionary<KeyValuePair<string, string>, IEnumerable<Book>>();
		}

		public bool AddBookToBacklog(string book, string authors, string label)
		{
			InvalidateCache();
			return m_readingListService.AddBookToBacklog(book, authors, label);
		}

		public IEnumerable<Book> GetReadingList(string listName, string label = null)
		{
			IEnumerable<Book> books;
			if (m_cache.TryGetValue(new KeyValuePair<string, string>(listName, label), out books))
			{
				if (!string.IsNullOrEmpty(label)) 
				{
					return books.Where(b => b.Label.ToLower().Equals(label.ToLower()));
				}
				return books;
			}
			m_logger.Info($"Cache miss for {listName}, {label}");
			books = m_readingListService.GetReadingList(listName, label);
			m_cache.TryAdd(new KeyValuePair<string, string>(listName, label), books);
			return books; 

		}

		public bool UpdateDoneListFromReadingList(string book)
		{
            InvalidateCache();
			return m_readingListService.UpdateDoneListFromReadingList(book); 

		}

		public void InvalidateCache() 
		{
			m_logger.Info("Invalidating cache");
			m_cache.Clear();
		}
	}
}
