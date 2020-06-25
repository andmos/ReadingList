using System.Collections.Generic;
using System.Linq;
using Nancy;
using ReadingList.Logic.Services;
using ReadingList.Modules;
using ReadingList.Trello.Models;
using ReadingList.Trello.Services;

namespace ReadingList.Web.Modules
{
    public class ReadingListModule : NancyModule
    {
        private readonly IReadingListService m_readingListService;
        private readonly IReadingBoardService m_readingBoardService;
        private readonly ITrelloAuthorizationWrapper m_trelloAuthWrapper;

        public ReadingListModule(ITrelloAuthorizationWrapper trelloAuthWrapper, IReadingListService readingListService, IReadingBoardService readingBoardService) : base("/api/")
        {
            this.EnableCors();
            m_trelloAuthWrapper = trelloAuthWrapper;
            m_readingListService = readingListService;
            m_readingBoardService = readingBoardService;
            StaticConfiguration.EnableHeadRouting = true;
            StaticConfiguration.DisableErrorTraces = false;

            Get["/readingList", true] = async (x, ct) =>
            {
                string requestLabel = Request.Query["label"];
                var readingList = await m_readingListService.GetReadingList(TrelloBoardConstans.CurrentlyReading, requestLabel);
                return Response.AsJson(readingList);
            };

            Get["/backlogList", true] = async (x, ct) =>
            {
                string requestLabel = Request.Query["label"];
                var readingList = await m_readingListService.GetReadingList(TrelloBoardConstans.Backlog, requestLabel);
                return Response.AsJson(readingList);
            };

            Get["/doneList", true] = async (x, ct) =>
            {
                string requestLabel = Request.Query["label"];
                var readingList = await m_readingListService.GetReadingList(TrelloBoardConstans.DoneReading, requestLabel);
                return Response.AsJson(readingList);
            };

            Get["/allLists", true] = async (x, ct) =>
            {
                string requestLabel = Request.Query["label"];
                var allLists = await m_readingBoardService.GetAllReadingLists(requestLabel);
                return Response.AsJson(allLists);
            };

            Post["/backlogList", true] = async (x, ct) =>
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

                var addBookToBacklog = await m_readingListService.AddBookToBacklog(bookTitle, author, bookLabel);
                response = Response.AsJson(addBookToBacklog);

                response.StatusCode = addBookToBacklog ? HttpStatusCode.Created : HttpStatusCode.InternalServerError;

                return response;
            };

            Put["/doneList", true] = async (x, ct) =>
            {
                string bookTitle = Request.Query["title"];
                Response response;
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

                var updateStatus = await m_readingListService.UpdateDoneListFromReadingList(bookTitle);

                response = Response.AsJson(updateStatus);
                response.StatusCode = HttpStatusCode.OK;
                return response;

            };

        }

        private KeyValuePair<ITrelloAuthModel, bool> CheckHeaderForMandatoryTokens(Request request)
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
