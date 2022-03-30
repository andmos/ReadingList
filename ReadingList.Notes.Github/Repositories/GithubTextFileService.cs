using System.Net.Http;
using System.Threading.Tasks;
using Octokit;
using Readinglist.Notes.Logic.Models;
using ReadingList.Notes.Github.Helpers;

namespace ReadingList.Notes.Github.Repositories
{
    public class GithubTextFileService : IGithubTextFileService
    {
        private readonly HttpClient _httpClient;

        public GithubTextFileService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<BookRecord> GetBookRecord(RepositoryContent content)
        {
            var rawBookRecord = await _httpClient.GetStringAsync(content.DownloadUrl);

            return BookRecordParser.CreateBookRecordFromMarkdown(rawBookRecord);
        }
    }
}

