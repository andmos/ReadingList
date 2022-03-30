using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Octokit;
using Readinglist.Notes.Logic.Models;
using ReadingList.Logging;
using ReadingList.Notes.Github.Helpers;

namespace ReadingList.Notes.Github.Repositories
{
    public class GithubBookRecordService : IGithubBookRecordService
    {
        private readonly HttpClient _httpClient;

        private const string UserName = "andmos";
        private const string Repo = "ReadingList-Data";
        private const string NotesFolder = "BookNotes";
        private const string ApplicationName = "ReadingList.Notes";

        private readonly IGitHubClient _gitHubClient;
        private readonly ILog _logger;
        private readonly ConcurrentDictionary<string, BookRecord> _cache;

        public GithubBookRecordService(ILogFactory logFactory)
        {
            _httpClient = new HttpClient();
            _gitHubClient = new GitHubClient(new ProductHeaderValue(ApplicationName));
            _cache = new ConcurrentDictionary<string, BookRecord>();
            _logger = logFactory.GetLogger(GetType());
        }

        public async Task<IEnumerable<BookRecord>> GetAllBookRecords()
        {
            try
            {
                var repo = await _gitHubClient.Repository.Content.GetAllContents(UserName, Repo, NotesFolder);

                var bookFiles = new List<BookRecord>();
                foreach (var content in repo)
                {
                    bookFiles.Add(await GetBookRecord(content));
                }

                return bookFiles;
            }
            catch (RateLimitExceededException rateLimitException)
            {
                _logger.Error("Rate limit exceeded against Github, falling back to cache: ", rateLimitException);
                return _cache.Values;
            }
        }

        private async Task<BookRecord> GetBookRecord(RepositoryContent content)
        {
            if (_cache.TryGetValue(content.Sha, out BookRecord bookRecord))
            {
                return bookRecord;
            }
            else
            {
                bookRecord = await GetBookRecordFromApi(content);
                _cache.TryAdd(content.Sha, bookRecord);
                return bookRecord;
            }
        }

        private async Task<BookRecord> GetBookRecordFromApi(RepositoryContent content)
        {
            var rawBookRecord = await _httpClient.GetStringAsync(content.DownloadUrl);

            return BookRecordParser.CreateBookRecordFromMarkdown(rawBookRecord);
        }
    }
}

