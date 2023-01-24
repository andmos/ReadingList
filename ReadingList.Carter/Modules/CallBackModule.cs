using System.Linq;
using ReadingList.Logging;
using ReadingList.Trello.Helpers;
using ReadingList.Trello.Services;
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
        private readonly ITrelloWebHookSources _webHookSourceService;
        private readonly ILog _logger;

        private const string BaseUri = "/api";

        public CallbackModule(
            IReadingListCache readingListCache,
            ILogFactory logger,
            ITrelloWebHookSources webHookSource)
        {
            _logger = logger.GetLogger(GetType());
            _readingListCache = readingListCache;
            _webHookSourceService = webHookSource;
        }

        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapMethods($"{BaseUri}/callBack", new[] {"HEAD"}, async (HttpRequest req, HttpResponse res) =>
            {
                res.StatusCode = 200;
                await res.AsJson("Head received");
            });

            app.MapPost($"{BaseUri}/callBack", async (HttpRequest req, HttpResponse res) =>
            {
                var callerIp = req.HttpContext.Connection.RemoteIpAddress;
                if (_webHookSourceService.IsValidWebHookSource(callerIp))
                {
                    _logger.Info($"Got Callback from valid source: {callerIp}");
                    _readingListCache.InvalidateCache();
                    _logger.Info("Invalidating cache");
                    res.StatusCode = 200;
                    await res.AsJson("Callback received");
                }
                else
                {
                    _logger.Info($"Got Callback from non-valid source: {callerIp}");
                }
            });
        }
    }
}
