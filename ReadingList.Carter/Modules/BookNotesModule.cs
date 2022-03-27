﻿using Carter;
using Carter.Response;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Readinglist.Notes.Logic.Services;

namespace ReadingList.Carter.Modules
{
    public class BookNotesModule : ICarterModule
    {
        private readonly IBookNotesService _bookNotesService;

        private const string BaseUri = "/api/notes";

        public BookNotesModule(IBookNotesService bookNotesService)
        {
            _bookNotesService = bookNotesService;
        }

        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet($"{BaseUri}/random", async (HttpRequest req, HttpResponse res) =>
            {
                var randomNote = await _bookNotesService.GetRandomBookNote();
                await res.AsJson(randomNote);
            });
        }
    }
}

