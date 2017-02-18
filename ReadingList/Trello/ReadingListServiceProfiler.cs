using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace ReadingList
{
	public class ReadingListServiceProfiler : IReadingListService
	{
		private IReadingListService m_readingListService; 
		private ILog m_logger; 
		public ReadingListServiceProfiler(IReadingListService readingListService, ILogFactory logFactory)
		{
			m_readingListService = readingListService;
			m_logger = logFactory.GetLogger(GetType());

		}

		public bool AddBookToBacklog(string book, string authors, string label)
		{
			var stopWatch = Stopwatch.StartNew();
			var addResult = m_readingListService.AddBookToBacklog(book, authors, label);
			stopWatch.Stop();
			m_logger.Info(string.Format("AddBookToBacklig: Call took {0} Ms", stopWatch.ElapsedMilliseconds));
			return addResult;
		}

		public IEnumerable<Book> GetReadingList(string listName, string label = null)
		{
			var stopWatch = Stopwatch.StartNew();
			var list = m_readingListService.GetReadingList(listName, label);
			stopWatch.Stop();
			m_logger.Info(string.Format("GetReadingList: Call for list {0} took {1} Ms", listName, stopWatch.ElapsedMilliseconds));
			return list; 
		}
	}
}
