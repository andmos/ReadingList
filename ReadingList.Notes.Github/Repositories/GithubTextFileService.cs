using System.Net.Http;
using System.Threading.Tasks;
using Octokit;

namespace ReadingList.Notes.Github.Repositories
{
    public class GithubTextFileService : IGithubTextFileService
    {
        private readonly HttpClient _httpClient;

        public GithubTextFileService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<string> GetRawBookRecord(RepositoryContent content) => await _httpClient.GetStringAsync(content.DownloadUrl);
    }
}

