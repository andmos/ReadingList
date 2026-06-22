using System;
using System.Collections.Generic;
using System.Linq;
using Carter;
using Carter.Response;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using ReadingList.Logic.Models;
using ReadingList.Logic.Services;

namespace ReadingList.Carter.Modules
{
    public record DoneBookResponse(
        string Title,
        IEnumerable<string> Authors,
        Label Label,
        DateTime? DateStartedReading,
        DateTime? DateFinishedReading,
        double? DaysToRead);

    public class ReadingListModule : ICarterModule
    {
        private const string BaseUri = "/api";

        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet($"{BaseUri}/readingList", async (HttpRequest req, HttpResponse res, IReadingListService readingListService) =>
            {
                string requestLabel = req.Query["label"];
                var result = await readingListService.GetReadingList(ReadingListConstants.CurrentlyReading, requestLabel);
                await res.Negotiate(result);
            });

            app.MapGet($"{BaseUri}/backlogList", async (HttpRequest req, HttpResponse res, IReadingListService readingListService) =>
            {
                string requestLabel = req.Query["label"];
                var result = await readingListService.GetReadingList(ReadingListConstants.Backlog, requestLabel);
                await res.Negotiate(result);
            });

            app.MapGet($"{BaseUri}/doneList", async (HttpRequest req, HttpResponse res, IReadingListService readingListService) =>
            {
                string requestLabel = req.Query["label"];
                var result = await readingListService.GetReadingList(ReadingListConstants.DoneReading, requestLabel, includeReadingDates: true);
                var response = result.Select(b =>
                {
                    double? daysToRead = null;
                    if (b.DateStartedReading.HasValue && b.DateFinishedReading.HasValue)
                    {
                        var days = Math.Round((b.DateFinishedReading.Value - b.DateStartedReading.Value).TotalDays, 1);
                        daysToRead = days >= 0 ? days : (double?)null;
                    }
                    return new DoneBookResponse(
                        b.Title,
                        b.Authors,
                        b.Label,
                        b.DateStartedReading,
                        b.DateFinishedReading,
                        daysToRead);
                });
                await res.Negotiate(response);
            });

            app.MapGet($"{BaseUri}/allLists", async (HttpRequest req, HttpResponse res, IReadingListCollectionService readingListCollectionService) =>
            {
                string requestLabel = req.Query["label"];
                var result = await readingListCollectionService.GetAllReadingLists(requestLabel);
                await res.Negotiate(result);
            });

            app.MapPost($"{BaseUri}/backlogList", async (HttpRequest req, HttpResponse res, IReadingListService readingListService) =>
            {
                string author = req.Query["author"];
                string bookTitle = req.Query["title"];
                string bookLabel = req.Query["label"];

                if (string.IsNullOrWhiteSpace(author) || string.IsNullOrWhiteSpace(bookTitle) || string.IsNullOrWhiteSpace(bookLabel))
                {
                    res.StatusCode = 422;
                    await res.Negotiate("author, title and label is required.");
                    return;
                }

                var addBookToBacklog = await readingListService.AddBookToBacklog(bookTitle, author, bookLabel);
                res.StatusCode = addBookToBacklog ? 201 : 500;
                await res.Negotiate(addBookToBacklog);
            }).RequireAuthorization();

            app.MapPut($"{BaseUri}/doneList", async (HttpRequest req, HttpResponse res, IReadingListService readingListService) =>
            {
                string bookTitle = req.Query["title"];
                if (string.IsNullOrWhiteSpace(bookTitle))
                {
                    res.StatusCode = 422;
                    await res.Negotiate("title is needed to move card from reading to done");
                    return;
                }

                var updateStatus = await readingListService.UpdateDoneListFromReadingList(bookTitle);
                await res.Negotiate(updateStatus);
            }).RequireAuthorization();
        }
    }
}