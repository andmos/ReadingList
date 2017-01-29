using System;
using Nancy;

namespace ReadingList
{
	public class ReadingListModule : NancyModule 
	{
		private readonly IReadingListService m_readingListService;

		public ReadingListModule(ITrelloAuthorizationWrapper trelloAuthService, IReadingListService readingListService) : base("/api/")
		{
			this.EnableCors();
			m_readingListService = readingListService;

			Get["/ping"] = parameters =>
			{
				return "pong";
			};

			Get["/readingList"] = parameters =>
			{
				string requestLabel = Request.Query["label"];
				var readingList = m_readingListService.GetReadingList(TrelloBoardConstans.CurrentlyReading, requestLabel);
				return Response.AsJson(readingList); 
			};

			Get["/backlogList"] = parameters =>
			{
				string requestLabel = Request.Query["label"];
				var readingList = m_readingListService.GetReadingList(TrelloBoardConstans.Backlog, requestLabel);
				return Response.AsJson(readingList);
			};

			Get["/doneList"] = parameters =>
			{
				string requestLabel = Request.Query["label"];
				var readingList = m_readingListService.GetReadingList(TrelloBoardConstans.DoneReading, requestLabel);
				return Response.AsJson(readingList);
			};
		
		}
	
	}
}
