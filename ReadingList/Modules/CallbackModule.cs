using System;
using System.Linq;
using Nancy;
namespace ReadingList.Modules
{
	public class CallbackModule : NancyModule 
    {
		private readonly IReadingListService m_readingListService;
        private readonly ITrelloWebHookSources m_webHookSource;
		private readonly ILog m_logger;

		public CallbackModule(IReadingListService readingListService, ILogFactory logger, ITrelloWebHookSources webHookSource) : base("/api/")
        {
			this.EnableCors();
			m_logger = logger.GetLogger(GetType());
			m_readingListService = readingListService;
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

                // TODO: This is a bit leaky..
                if (m_readingListService is ReadingListCache)
                {
                    ((ReadingListCache)m_readingListService).InvalidateCache();
                    m_logger.Info("Invalidating cache");
                }

                respons = Response.AsJson("Callback recived");
                respons.StatusCode = HttpStatusCode.OK;
                return respons;
            };
		}


    }
}
