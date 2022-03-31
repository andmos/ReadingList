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

        private const string UserName = "andmos";
        private const string Repo = "ReadingList-Data";
        private const string NotesFolder = "BookNotes";

        private readonly ILog _logger;

        private readonly GithubClient _githubFileClient;

        public GithubBookRecordService(IBookRecordCache bookRecordCache, ILogFactory logFactory, GithubClient githubFileClient)
        {
            _bookRecordCache = bookRecordCache;
            _githubFileClient = githubFileClient;
            _logger = logFactory.GetLogger(GetType());
        }

        public async Task<IEnumerable<BookRecord>> GetAllBookRecords()
        {
            try
            {
                var repo = await _githubFileClient.GetRepositoryContent(UserName, Repo, NotesFolder);

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

        private async Task<BookRecord> GetBookRecord(RepositoryContentInfo content)
        {
            if (_bookRecordCache.TryGetValue(content.Sha, out var bookRecord))
            {
                return bookRecord;
            }
            bookRecord = await GetBookRecordFromApi(content);
            _bookRecordCache.TryAdd(content.Sha, bookRecord);
            return bookRecord;
        }

        private async Task<BookRecord> GetBookRecordFromApi(RepositoryContentInfo content)
        {
            var rawBookRecord = await _githubFileClient.GetRepositoryTextFile(content.DownloadUrl);

            return BookRecordParser.CreateBookRecordFromMarkdown(rawBookRecord);
        }
    }
}

