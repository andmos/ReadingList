using System.Collections.Generic;
using System.Linq;
using Carter;
using Carter.Response;
using Microsoft.AspNetCore.Http;
using ReadingList.Logic.Services;
using ReadingList.Trello.Models;
using ReadingList.Trello.Services;

namespace ReadingList.Carter.Modules
{
    public class ReadingListModule : CarterModule 
    { 
	    private readonly IReadingListService m_readingListService;
		private readonly IReadingBoardService m_readingBoardService;
		private readonly ITrelloAuthorizationWrapper m_trelloAuthWrapper; 

		public ReadingListModule(
			ITrelloAuthorizationWrapper trelloAuthWrapper, 
			IReadingListService readingListService, 
			IReadingBoardService readingBoardService) : base("/api")
		{
			m_trelloAuthWrapper = trelloAuthWrapper;
			m_readingListService = readingListService;
			m_readingBoardService = readingBoardService;

            
			Get<GetReadingList>("/readingList", async (req, res) =>
			{
				string requestLabel = req.Query["label"];
				var readingList = await m_readingListService.GetReadingList(TrelloBoardConstans.CurrentlyReading, requestLabel);
				await res.AsJson(readingList);
			});

			Get<GetBacklogList>("/backlogList",async (req, res) =>
			{
				string requestLabel = req.Query["label"];
				var readingList = await m_readingListService.GetReadingList(TrelloBoardConstans.Backlog, requestLabel);
				await res.AsJson(readingList);
			});

			Get<GetDoneList>("/doneList", async (req, res) =>
			{
				string requestLabel = req.Query["label"];
				var readingList = await m_readingListService.GetReadingList(TrelloBoardConstans.DoneReading, requestLabel);
				await res.AsJson(readingList);
			});

			Get<GetReadingList>("/allLists", async (req, res) =>
			{
                string requestLabel = req.Query["label"];
                var allLists = await m_readingBoardService.GetAllReadingLists(requestLabel);
				await res.AsJson(allLists);
			});

			Post("/backlogList", async (req, res) =>
			{
				string author = req.Query["author"];
				string bookTitle = req.Query["title"];
				string bookLabel = req.Query["label"];

				if (string.IsNullOrWhiteSpace(author) || string.IsNullOrWhiteSpace(bookTitle) || string.IsNullOrWhiteSpace(bookLabel))
				{
					res.StatusCode = 422;
					await res.AsJson("author, title and label is required.");
					return;
				}

				var authTokens = CheckHeaderForMandatoryTokens(req);
				if (!authTokens.Value)
				{
					res.StatusCode = 403;
					await res.AsJson("TrelloAPIKey and TrelloUserToken is required in header to do this operation.");
					return;
				}
				if (!CheckTokens(authTokens.Key, m_trelloAuthWrapper))
				{
					res.StatusCode = 403;
					await res.AsJson("TrelloAPIKey and TrelloUserToken does not match configured APIKey or Token");
					return;
				}

				var addBookToBacklog = await m_readingListService.AddBookToBacklog(bookTitle, author, bookLabel);
				res.StatusCode = addBookToBacklog ? 201 : 500;
				await res.AsJson(addBookToBacklog);
			});

			Put("/doneList", async (req, res) =>
			{
				string bookTitle = req.Query["title"];
				if (string.IsNullOrWhiteSpace(bookTitle))
				{
					res.StatusCode = 422;
					await res.AsJson("title is needed to move card from reading to done");
					return;
				}

				var authTokens = CheckHeaderForMandatoryTokens(req);
				if (!authTokens.Value)
				{
					res.StatusCode = 403;
					await res.AsJson("TrelloAPIKey and TrelloUserToken is required in header to do this operation.");
					return;
				}
				if (!CheckTokens(authTokens.Key, m_trelloAuthWrapper))
				{
					res.StatusCode = 403;
					await res.AsJson("TrelloAPIKey and TrelloUserToken does not match configured APIKey or Token");
					return;
				}


				var updateStatus = await m_readingListService.UpdateDoneListFromReadingList(bookTitle);
				
				await res.AsJson(updateStatus);
			});      
		}

		private KeyValuePair<ITrelloAuthModel, bool> CheckHeaderForMandatoryTokens(HttpRequest request)
		{
			string providedApiKey = request.Headers["TrelloAPIKey"].FirstOrDefault();
			string providedUserToken = request.Headers["TrelloUserToken"].FirstOrDefault();

			return new KeyValuePair<ITrelloAuthModel, bool>(
				new TrelloAuthSettings { TrelloAPIKey = providedApiKey, TrelloUserToken = providedUserToken },
				!string.IsNullOrWhiteSpace(providedApiKey) && !string.IsNullOrWhiteSpace(providedUserToken));
		}
		private bool CheckTokens(ITrelloAuthModel authTokens, ITrelloAuthorizationWrapper authWrapper) => authWrapper.IsValidKeys(authTokens);
    }
}