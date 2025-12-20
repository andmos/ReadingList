using ReadingList.Logging;
using ReadingList.Trello.Helpers;
using Carter;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace ReadingList.Web.Modules
{
    public class CallbackModule : ICarterModule
    {
        private const string BaseUri = "/api";

        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapMethods($"{BaseUri}/callBack", new[] { "HEAD" }, () =>
            {
                return Results.Ok("Head received");
            });

            app.MapPost($"{BaseUri}/callBack", (HttpRequest req, IReadingListCache readingListCache, ILogFactory logFactory) =>
            {
                var logger = logFactory.GetLogger(typeof(CallbackModule));
                var callerIp = req.HttpContext.Connection.RemoteIpAddress;

                logger.Info($"Got Callback from source: {callerIp}");
                readingListCache.InvalidateCache();
                logger.Info("Invalidating cache");
                return Results.Ok("Callback received");
            });
        }
    }
}
