using Carter;
using Carter.Request;
using Carter.Response;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Readinglist.Notes.Logic.Services;

namespace ReadingList.Carter.Modules
{
    public class BookNotesModule : ICarterModule
    {
        private const string BaseUri = "/api/notes";

        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet($"{BaseUri}/random", async (HttpResponse res, IBookNotesService bookNotesService) =>
            {
                await res.Negotiate(await bookNotesService.GetRandomBookNote());
            });

            app.MapGet($"{BaseUri}/all", async (HttpResponse res, IBookNotesService bookNotesService) =>
            {
                await res.Negotiate(await bookNotesService.GetAllBookNotes());
            });
            
            app.MapGet($"{BaseUri}/book", async (HttpRequest req, HttpResponse res, IBookNotesService bookNotesService) =>
            {
                await res.Negotiate(await bookNotesService.GetBookNotes(req.Query.As<string>("title")));
            });
        }
    }
}

