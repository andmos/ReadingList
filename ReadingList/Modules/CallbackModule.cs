using System;
using System.Linq;
using Nancy;
using ReadingList.Trello.Helpers;
using ReadingList.Trello.Models;
using ReadingList.Trello.Logging;
using ReadingList.Modules;

namespace ReadingList.Web.Modules
{
	public class CallbackModule : NancyModule 
    {
		private readonly IReadingListCache m_readingListCache;
        private readonly ITrelloWebHookSources m_webHookSource;
		private readonly ILog m_logger;

		public CallbackModule(IReadingListCache readingListCache, ILogFactory logger, ITrelloWebHookSources webHookSource) : base("/api/")
        {
			this.EnableCors();
			m_logger = logger.GetLogger(GetType());
			m_readingListCache = readingListCache;
            m_webHookSource = webHookSource;

			Head["/callBack"] = parameters =>
            {
                Response respons;
                respons = Response.AsJson("Head recived");
                respons.StatusCode = HttpStatusCode.OK;
                return respons;
            };

            Post["/callBack"] = parameters =>
            {
                Response respons;

                if (m_webHookSource.ValidWebhookSources().ToList().Any(source => source.Equals(Request.UserHostAddress)))
                {
                    m_logger.Info($"Got Callback from valid source: {Request.UserHostAddress}");
                }

                
				m_readingListCache.InvalidateCache();
                m_logger.Info("Invalidating cache");
                

                respons = Response.AsJson("Callback recived");
                respons.StatusCode = HttpStatusCode.OK;
                return respons;
            };
		}


    }
}
