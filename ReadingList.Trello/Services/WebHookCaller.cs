using System.Net.Http;
using System.Threading.Tasks;
using ReadingList.Logging;
using ReadingList.Trello.Models;

namespace ReadingList.Trello.Services
{
	public class WebHookCaller : IWebHookCaller
	{
		private readonly ILog m_logger;
		private readonly ITrelloAuthModel m_authModel;

		public WebHookCaller(ILogFactory logFactory, ITrelloAuthModel authModel)
		{
			m_logger = logFactory.GetLogger(GetType());
      		m_authModel = authModel; 

        }

        public bool Configured { get; set; }

        public async Task SetUpWebHook(TrelloWebhook webHook)
		{
			using (var httpClient = new HttpClient())
			{
				var response = await httpClient.PostAsJsonAsync($"https://api.trello.com/1/tokens/{m_authModel.TrelloUserToken}/webhooks/?key={m_authModel.TrelloAPIKey}", webHook);
				var resultString = await response.Content.ReadAsStringAsync();

				m_logger.Info(resultString);	

			}
		}
	}
}