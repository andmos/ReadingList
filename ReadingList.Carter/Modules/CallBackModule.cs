using ReadingList.Logging;
using ReadingList.Trello.Helpers;
using Carter;
using Carter.Response;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace ReadingList.Web.Modules
{
    public class CallbackModule : ICarterModule
    {
        private readonly IReadingListCache _readingListCache;
        private readonly ILog _logger;

        private const string BaseUri = "/api";

        public CallbackModule(
            IReadingListCache readingListCache,
            ILogFactory logger)
        {
            _logger = logger.GetLogger(GetType());
            _readingListCache = readingListCache;
        }

        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapMethods($"{BaseUri}/callBack", new[] { "HEAD" }, async (HttpRequest req, HttpResponse res) =>
            {
                res.StatusCode = 200;
                await res.AsJson("Head received");
            });

            app.MapPost($"{BaseUri}/callBack", async (HttpRequest req, HttpResponse res) =>
            {
                var callerIp = req.HttpContext.Connection.RemoteIpAddress;

                _logger.Info($"Got Callback from source: {callerIp}");
                _readingListCache.InvalidateCache();
                _logger.Info("Invalidating cache");
                res.StatusCode = 200;
                await res.AsJson("Callback received");
            });
        }
    }
}
