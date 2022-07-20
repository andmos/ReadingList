using Carter;
using Carter.Response;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using ReadingList.Logic.Models;
using ReadingList.Logic.Services;
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
                var readingList = await _readingListService.GetReadingList(ReadingListConstants.CurrentlyReading, requestLabel);
                await res.AsJson(readingList);
            });

            app.MapGet($"{BaseUri}/backlogList", async (HttpRequest req, HttpResponse res) =>
            {
                string requestLabel = req.Query["label"];
                var readingList = await _readingListService.GetReadingList(ReadingListConstants.Backlog, requestLabel);
                await res.AsJson(readingList);
            });

            app.MapGet($"{BaseUri}/doneList", async (HttpRequest req, HttpResponse res) =>
            {
                string requestLabel = req.Query["label"];
                var readingList = await _readingListService.GetReadingList(ReadingListConstants.DoneReading, requestLabel);
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

                if (string.IsNullOrWhiteSpace(author) || string.IsNullOrWhiteSpace(bookTitle) || string.IsNullOrWhiteSpace(bookLabel))
                {
                    res.StatusCode = 422;
                    await res.AsJson("author, title and label is required.");
                    return;
                }

                var addBookToBacklog = await _readingListService.AddBookToBacklog(bookTitle, author, bookLabel);
                res.StatusCode = addBookToBacklog ? 201 : 500;
                await res.AsJson(addBookToBacklog);
            }).RequireAuthorization();

            app.MapPut($"{BaseUri}/doneList", async (HttpRequest req, HttpResponse res) =>
            {
                string bookTitle = req.Query["title"];
                if (string.IsNullOrWhiteSpace(bookTitle))
                {
                    res.StatusCode = 422;
                    await res.AsJson("title is needed to move card from reading to done");
                    return;
                }

                var updateStatus = await _readingListService.UpdateDoneListFromReadingList(bookTitle);

                await res.AsJson(updateStatus);
            }).RequireAuthorization();
        }
    }
}