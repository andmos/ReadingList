using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Octokit;
using ReadingList.Logging;
using ReadingList.Notes.Github.Helpers;
using Readinglist.Notes.Logic.Models;

namespace ReadingList.Notes.Github.Services
{
    public class GithubBookRecordService : IGithubBookRecordService
    {
        private readonly IBookRecordCache _bookRecordCache;
        private readonly HttpClient _httpClient;

        private const string UserName = "andmos";
        private const string Repo = "ReadingList-Data";
        private const string NotesFolder = "BookNotes";
        private const string ApplicationName = "ReadingList.Notes";

        private readonly IGitHubClient _gitHubClient;
        private readonly ILog _logger;

        public GithubBookRecordService(IBookRecordCache bookRecordCache, ILogFactory logFactory)
        {
            _bookRecordCache = bookRecordCache;
            _httpClient = new HttpClient();
            _gitHubClient = new GitHubClient(new ProductHeaderValue(ApplicationName));
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
                return _bookRecordCache.GetAll();
            }
        }

        private async Task<BookRecord> GetBookRecord(RepositoryContent content)
        {
            if (_bookRecordCache.TryGetValue(content.Sha, out var bookRecord))
            {
                return bookRecord;
            }
            bookRecord = await GetBookRecordFromApi(content);
            _bookRecordCache.TryAdd(content.Sha, bookRecord);
            return bookRecord;
        }

        private async Task<BookRecord> GetBookRecordFromApi(RepositoryContent content)
        {
            var rawBookRecord = await _httpClient.GetStringAsync(content.DownloadUrl);

            return BookRecordParser.CreateBookRecordFromMarkdown(rawBookRecord);
        }
    }
}

