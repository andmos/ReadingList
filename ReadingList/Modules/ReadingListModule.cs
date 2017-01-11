using System;
using Nancy;

namespace ReadingList
{
	public class ReadingListModule : NancyModule 
	{
		private readonly IReadingListService m_readingListService;

		public ReadingListModule(ITrelloAuthorizationWrapper trelloAuthService, IReadingListService readingListService) : base("/api/")
		{
			m_readingListService = readingListService;

			Get["/readingList"] = parameters =>
			{
				var readingList = m_readingListService.GetReadingList(TrelloBoardConstans.CurrentlyReading);
				return Response.AsJson(readingList); 
			};

			Get["/backlogList"] = parameters =>
			{
				var readingList = m_readingListService.GetReadingList(TrelloBoardConstans.Backlog);
				return Response.AsJson(readingList);
			};

			Get["/doneList"] = parameters =>
			{
				var readingList = m_readingListService.GetReadingList(TrelloBoardConstans.DoneReading);
				return Response.AsJson(readingList);
			};
		
		}
	
	}
}
