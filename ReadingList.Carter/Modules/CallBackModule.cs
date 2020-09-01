using System;
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
        private readonly IReadingListCache m_readingListCache;
        private readonly ITrelloWebHookSources m_webHookSource;
        private readonly ILog m_logger;

        public CallbackModule(IReadingListCache readingListCache, ILogFactory logger, ITrelloWebHookSources webHookSource) : base("/api")
        {
            m_logger = logger.GetLogger(GetType());
            m_readingListCache = readingListCache;
            m_webHookSource = webHookSource;

            Head("/callBack", async (req, res) =>
            {
                res.StatusCode = 200;
                await res.AsJson("Head received");
                
            });

            Post("/callBack", async (req, res) =>
            {

                var callerIp = req.HttpContext.Connection.RemoteIpAddress;
                if (m_webHookSource.ValidWebhookSources().ToList().Any(source => source.Equals(callerIp)))
                {
                    m_logger.Info($"Got Callback from valid source: {callerIp}");
                }


                m_readingListCache.InvalidateCache();
                m_logger.Info("Invalidating cache");

                res.StatusCode = 200;
                await res.AsJson("Callback received");
                
            });
        }


    }
}
