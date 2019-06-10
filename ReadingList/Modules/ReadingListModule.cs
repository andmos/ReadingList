using System;
using Nancy;
using System.Linq;
using Nancy.Routing;
using System.Collections.Generic;

namespace ReadingList
{
	public class ReadingListModule : NancyModule
	{
		private readonly IReadingListService m_readingListService;
		private readonly IReadingBoardService m_readingBoardService;
		private readonly ITrelloAuthorizationWrapper m_trelloAuthWrapper; 
		private readonly ILog m_logger;

		public ReadingListModule(ITrelloAuthorizationWrapper trelloAuthWrapper, IReadingListService readingListService, IReadingBoardService readingBoardService, ILogFactory logger) : base("/api/")
		{
			this.EnableCors();
			m_trelloAuthWrapper = trelloAuthWrapper;
			m_readingListService = readingListService;
			m_readingBoardService = readingBoardService;
			m_logger = logger.GetLogger(GetType());
			StaticConfiguration.EnableHeadRouting = true;
			StaticConfiguration.DisableErrorTraces = false;
            
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

			Get["/allLists"] = parameters =>
			{
                string requestLabel = Request.Query["label"];
                var allLists = m_readingBoardService.GetAllReadingLists(requestLabel);
				return Response.AsJson(allLists);
			};

			Put["/backlogList"] = parameters =>
			{
				Response response;
				string author = Request.Query["author"];
				string bookTitle = Request.Query["title"];
				string bookLabel = Request.Query["label"];

				if (string.IsNullOrWhiteSpace(author) || string.IsNullOrWhiteSpace(bookTitle) || string.IsNullOrWhiteSpace(bookLabel))
				{
					response = Response.AsJson("author, title and label is required.");
					response.StatusCode = HttpStatusCode.UnprocessableEntity;
					return response;
				}

				var authTokens = CheckHeaderForMandatoryTokens(Request);
				if (!authTokens.Value)
				{
					response = Response.AsJson("TrelloAPIKey and TrelloUserToken is required in header to do this operation.");
					response.StatusCode = HttpStatusCode.Forbidden;
					return response;
				}
				if (!CheckTokens(authTokens.Key, m_trelloAuthWrapper))
				{
					response = Response.AsJson("TrelloAPIKey and TrelloUserToken does not match configured APIKey or Token");
					response.StatusCode = HttpStatusCode.Forbidden;
					return response;
				}

				var addBookToBacklog = m_readingListService.AddBookToBacklog(bookTitle, author, bookLabel);
				response = Response.AsJson(addBookToBacklog);

				if (addBookToBacklog)
				{
					response.StatusCode = HttpStatusCode.Created;
				}
				else
				{
					response.StatusCode = HttpStatusCode.InternalServerError;
				}

				return response;
			};

			Put["/doneList"] = parameters =>
			{
				string bookTitle = Request.Query["title"];
				Response response = new Response();
				if (string.IsNullOrWhiteSpace(bookTitle))
				{
					response = Response.AsJson("title is needed to move card from reading to done");
					response.StatusCode = HttpStatusCode.UnprocessableEntity;
				}

				var authTokens = CheckHeaderForMandatoryTokens(Request);
				if (!authTokens.Value) 
				{
					response = Response.AsJson("TrelloAPIKey and TrelloUserToken is required in header to do this operation.");
					response.StatusCode = HttpStatusCode.Forbidden;
					return response;
				}
				if (!CheckTokens(authTokens.Key, m_trelloAuthWrapper))
				{ 
					response = Response.AsJson("TrelloAPIKey and TrelloUserToken does not match configured APIKey or Token");
					response.StatusCode = HttpStatusCode.Forbidden;
					return response;
				}


				var updateStatus = m_readingListService.UpdateDoneListFromReadingList(bookTitle);

				response = Response.AsJson(updateStatus);
				response.StatusCode = HttpStatusCode.OK;
				return response;

			};
            
		}

		private KeyValuePair<ITrelloAuthModel, bool> CheckHeaderForMandatoryTokens(Request request)
		{
			string providedAPIKey = request.Headers["TrelloAPIKey"].FirstOrDefault();
			string providedUserToken = request.Headers["TrelloUserToken"].FirstOrDefault();

			return new KeyValuePair<ITrelloAuthModel, bool>(new TrelloAuthModel(providedAPIKey, providedUserToken), !string.IsNullOrWhiteSpace(providedAPIKey) && !string.IsNullOrWhiteSpace(providedUserToken));
		}

		private bool CheckTokens(ITrelloAuthModel authTokens, ITrelloAuthorizationWrapper authWrapper) => authWrapper.IsValidKeys(authTokens);

	}
}
