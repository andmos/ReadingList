using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using ReadingList.Logic.Models;
using ReadingList.Logic.Services;
using ReadingList.Trello.Logging;

namespace ReadingList.Trello.Helpers
{
    public class ReadingListServiceProfiler : IReadingListService
	{
		private IReadingListService m_readingListService;
		private readonly ILog m_logger;
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
			m_logger.Info($"AddBookToBacklig: Call took {stopWatch.ElapsedMilliseconds} Ms");
			return addResult;
		}

		public async Task<IEnumerable<Book>> GetReadingList(string listName, string label = null)
		{
			var stopWatch = Stopwatch.StartNew();
			var list = await m_readingListService.GetReadingList(listName, label);
			stopWatch.Stop();
			m_logger.Info($"GetReadingList: Call for list {listName} took {stopWatch.ElapsedMilliseconds} Ms");
			return list;
		}

		public bool UpdateDoneListFromReadingList(string book)
		{
			var stopWatch = Stopwatch.StartNew();
			var result = m_readingListService.UpdateDoneListFromReadingList(book);
			stopWatch.Stop();
			m_logger.Info($"UpdateDoneList: Call took {stopWatch.ElapsedMilliseconds}");
			return result;
		}
	}
}
