using System.Collections.Generic;
using Readinglist.Notes.Logic.Models;
using Readinglist.Notes.Logic.Services;
using Octokit;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using ReadingList.Notes.Github.Helpers;

namespace ReadingList.Notes.Github.Repositories
{
    public class GithubReadingListDataRepository : IBookRecordRepository
    {
        private const string UserName = "andmos";
        private const string Repo = "ReadingList-Data";
        private const string NotesFolder = "BookNotes";
        private const string ApplicationName = "ReadingList.Notes";

        private readonly IGitHubClient _gitHubClient;
        private readonly HttpClient _httpClient;
        public GithubReadingListDataRepository()
        {
            _httpClient = new HttpClient();
            _gitHubClient = new GitHubClient(new ProductHeaderValue(ApplicationName));
        }

        public async Task<IEnumerable<BookRecord>> GetAllBookRecords()
        {
            var rawBookRecords = await GetRawBookRecords();
            return rawBookRecords.Select(BookRecordParser.CreateBookRecordFromMarkdown);
        }

        private async Task<List<string>> GetRawBookRecords()
        {
            var repo = await _gitHubClient.Repository.Content.GetAllContents(UserName, Repo, NotesFolder);

            var bookFiles = new List<string>();
            foreach (var file in repo)
            {
                bookFiles.Add(await _httpClient.GetStringAsync(file.DownloadUrl));
            }

            return bookFiles;
        }
    }
}

