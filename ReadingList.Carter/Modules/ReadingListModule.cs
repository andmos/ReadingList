using System.Collections.Generic;
using System.Linq;
using Carter;
using Carter.Response;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using ReadingList.Logic.Services;
using ReadingList.Trello.Models;
using ReadingList.Trello.Services;

namespace ReadingList.Carter.Modules
{
    public class ReadingListModule : ICarterModule
    {
        private readonly IReadingListService _readingListService;
        private readonly IReadingListCollectionService _readingListCollectionService;
        private readonly ITrelloAuthorizationWrapper _trelloAuthWrapper;

        private const string BaseUri = "/api";

        public ReadingListModule(
            ITrelloAuthorizationWrapper trelloAuthWrapper,
            IReadingListService readingListService,
            IReadingListCollectionService readingListCollectionService)
        {
            _trelloAuthWrapper = trelloAuthWrapper;
            _readingListService = readingListService;
            _readingListCollectionService = readingListCollectionService;
        }

        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet($"{BaseUri}/readingList", async (HttpRequest req, HttpResponse res) =>
            {
                string requestLabel = req.Query["label"];
                var readingList = await _readingListService.GetReadingList(TrelloBoardConstans.CurrentlyReading, requestLabel);
                await res.AsJson(readingList);
            });

            app.MapGet($"{BaseUri}/backlogList", async (HttpRequest req, HttpResponse res) =>
            {
                 string requestLabel = req.Query["label"];
                 var readingList = await _readingListService.GetReadingList(TrelloBoardConstans.Backlog, requestLabel);
                 await res.AsJson(readingList);
            });

            app.MapGet($"{BaseUri}/doneList", async (HttpRequest req, HttpResponse res) =>
            {
                string requestLabel = req.Query["label"];
                var readingList = await _readingListService.GetReadingList(TrelloBoardConstans.DoneReading, requestLabel);
                await res.AsJson(readingList);
            });

            app.MapGet($"{BaseUri}/allLists", async (HttpRequest req, HttpResponse res) =>
            {
                string requestLabel = req.Query["label"];
                var allLists = await _readingListCollectionService.GetAllReadingLists(requestLabel);
                await res.AsJson(allLists);
            });

            app.MapPost($"{BaseUri}/backlogList", async (HttpRequest req, HttpResponse res) =>
            {
                string author = req.Query["author"];
                string bookTitle = req.Query["title"];
                string bookLabel = req.Query["label"];
                
                var authTokens = CheckHeaderForMandatoryTokens(req);
                if (!authTokens.Value)
                {
                    res.StatusCode = 403;
                    await res.AsJson("TrelloAPIKey and TrelloUserToken is required in header to do this operation.");
                    return;
                }
                if (!CheckTokens(authTokens.Key))
                {
                    res.StatusCode = 403;
                    await res.AsJson("TrelloAPIKey and TrelloUserToken does not match configured APIKey or Token");
                    return;
                }
                
                if (string.IsNullOrWhiteSpace(author) || string.IsNullOrWhiteSpace(bookTitle) || string.IsNullOrWhiteSpace(bookLabel))
                {
                    res.StatusCode = 422;
                    await res.AsJson("author, title and label is required.");
                    return;
                }
                
                var addBookToBacklog = await _readingListService.AddBookToBacklog(bookTitle, author, bookLabel);
                res.StatusCode = addBookToBacklog ? 201 : 500;
                await res.AsJson(addBookToBacklog);
            });



            app.MapPut($"{BaseUri}/doneList", async (HttpRequest req, HttpResponse res) =>
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
                if (!CheckTokens(authTokens.Key))
                {
                    res.StatusCode = 403;
                    await res.AsJson("TrelloAPIKey and TrelloUserToken does not match configured APIKey or Token");
                    return;
                }
                
                var updateStatus = await _readingListService.UpdateDoneListFromReadingList(bookTitle);

                await res.AsJson(updateStatus);
            });            
        }

        private KeyValuePair<ITrelloAuthModel, bool> CheckHeaderForMandatoryTokens(HttpRequest request)
        {
            var providedApiKey = request.Headers["TrelloAPIKey"].FirstOrDefault();
            var providedUserToken = request.Headers["TrelloUserToken"].FirstOrDefault();

            return new KeyValuePair<ITrelloAuthModel, bool>(
                new TrelloAuthSettings { TrelloAPIKey = providedApiKey, TrelloUserToken = providedUserToken },
                !string.IsNullOrWhiteSpace(providedApiKey) && !string.IsNullOrWhiteSpace(providedUserToken));
        }
        private bool CheckTokens(ITrelloAuthModel authTokens) => _trelloAuthWrapper.IsValidKeys(authTokens);
    }
}