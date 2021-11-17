using Carter;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace ReadingList.Web.Modules
{
    public class PingModule : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/api/ping", (HttpResponse res) =>
            {
                return Results.Json("pong");
            });
        }
    }
}