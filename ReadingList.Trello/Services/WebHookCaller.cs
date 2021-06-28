using System.Net.Http;
using System.Threading.Tasks;
using ReadingList.Logging;
using ReadingList.Trello.Models;

namespace ReadingList.Trello.Services
{
    public class WebHookCaller : IWebHookCaller
    {
        private readonly ILog _logger;
        private readonly ITrelloAuthModel _authModel;

        public WebHookCaller(ILogFactory logFactory, ITrelloAuthModel authModel)
        {
            _logger = logFactory.GetLogger(GetType());
            _authModel = authModel;

        }

        public bool Configured { get; set; }

        public async Task SetUpWebHook(TrelloWebhook webHook)
        {
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.PostAsJsonAsync($"https://api.trello.com/1/tokens/{_authModel.TrelloUserToken}/webhooks/?key={_authModel.TrelloAPIKey}", webHook);
                var resultString = await response.Content.ReadAsStringAsync();

                _logger.Info(resultString);

            }
        }
    }
}
