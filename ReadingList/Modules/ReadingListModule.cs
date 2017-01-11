using System;
using Nancy;

namespace ReadingList
{
	public class ReadingListModule : NancyModule 
	{
		private readonly IReadingListService m_readingListService;
		private readonly ITrelloAuthorizationWrapper m_trelloAuth; 

		public ReadingListModule(ITrelloAuthorizationWrapper trelloAuthService, IReadingListService readingListService) : base("/api/")
		{
			if (m_trelloAuth == null) 
			{
				m_trelloAuth = trelloAuthService;
			}

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
