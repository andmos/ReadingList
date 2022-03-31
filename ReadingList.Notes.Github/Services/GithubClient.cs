using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Octokit;

namespace ReadingList.Notes.Github.Services
{
    public class GithubClient
    {
        public HttpClient Client { get; }
        private readonly IGitHubClient _gitHubClient;
        private const string ApplicationName = "ReadingList.Notes";

        public GithubClient(HttpClient client)
        {
            client.DefaultRequestHeaders.Add("Accept","application/vnd.github.v3+json");
            client.DefaultRequestHeaders.Add("User-Agent", ApplicationName);
            Client = client;

            _gitHubClient = new GitHubClient(new ProductHeaderValue(ApplicationName));
        }

        public async Task<string> GetRepositoryTextFile(string url) => await Client.GetStringAsync(url);

        public async Task<IReadOnlyList<RepositoryContent>> GetRepositoryContent(string username, string repo, string repoPath) =>await _gitHubClient.Repository.Content.GetAllContents(username, repo, repoPath);
        
    }
}

