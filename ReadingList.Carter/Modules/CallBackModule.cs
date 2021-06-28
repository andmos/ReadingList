using System.Linq;
using ReadingList.Logging;
using ReadingList.Trello.Helpers;
using ReadingList.Trello.Models;
using Carter;
using Carter.Response;


namespace ReadingList.Web.Modules
{
    public class CallbackModule : CarterModule
    {
        private readonly IReadingListCache _readingListCache;
        private readonly ITrelloWebHookSources _webHookSource;
        private readonly ILog _logger;

        public CallbackModule(
            IReadingListCache readingListCache,
            ILogFactory logger,
            ITrelloWebHookSources webHookSource) : base("/api")
        {
            _logger = logger.GetLogger(GetType());
            _readingListCache = readingListCache;
            _webHookSource = webHookSource;

            Head("/callBack", async (req, res) =>
            {
                res.StatusCode = 200;
                await res.AsJson("Head received");

            });

            Post("/callBack", async (req, res) =>
            {

                var callerIp = req.HttpContext.Connection.RemoteIpAddress;
                if (_webHookSource.ValidWebhookSources().ToList().Any(source => source.Equals(callerIp)))
                {
                    _logger.Info($"Got Callback from valid source: {callerIp}");
                }


                _readingListCache.InvalidateCache();
                _logger.Info("Invalidating cache");

                res.StatusCode = 200;
                await res.AsJson("Callback received");

            });
        }


    }
}
