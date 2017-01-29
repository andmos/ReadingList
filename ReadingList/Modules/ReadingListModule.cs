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

			Put["/backlogList"] = parameters =>
			{
				string author = Request.Query["author"];
				string bookTitle = Request.Query["title"];
				string bookLabel = Request.Query["label"];

				if (string.IsNullOrWhiteSpace(author) || string.IsNullOrWhiteSpace(bookTitle))
				{
					return Response.AsText("author, title and label is required.").StatusCode = HttpStatusCode.BadRequest;
				}
				try
				{
					return Response.AsJson(m_readingListService.AddBookToBacklog(bookTitle, author, bookLabel));
				}
				catch(Exception ex) 
				{
					return Response.AsText(ex.ToString());
				}
			}; 
		
		
		}
	
	}
}
