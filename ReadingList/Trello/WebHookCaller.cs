using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace ReadingList
{
	public class WebHookCaller : IWebHookCaller
	{
		private readonly ILog m_logger; 

		public WebHookCaller(ILogFactory logFactory) 
		{
			m_logger = logFactory.GetLogger(GetType());
		}

		public async Task<string> SetUpWebHook(TrelloWebhook webHook, string apiKey, string token)
		{
			using (var httpClient = new HttpClient()) 
			{
				var response = await httpClient.PostAsJsonAsync($"https://api.trello.com/1/tokens/{token}/webhooks/?key={apiKey}", webHook);
				var resultString = await response.Content.ReadAsStringAsync();

				m_logger.Info(resultString);

				return response.Content.ToString();
			}
		}
	}
}
