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

        private readonly HttpClient httpClient; 

        public GithubReadingListDataRepository()
        {
            httpClient = new HttpClient();
        }

        public async Task<IEnumerable<BookRecord>> GetAllBookRecords()
        {
            var rawBookRecords = await GetRawBookRecords();
            return rawBookRecords.Select(BookRecordMapper.CreateBookRecordFromMarkdown);
        }

        private async Task<List<string>> GetRawBookRecords()
        {
            var github = new GitHubClient(new ProductHeaderValue("ReadingList.Notes"));
            var repo = await github.Repository.Content.GetAllContents(UserName, Repo, NotesFolder);

            var bookFiles = new List<string>();
            foreach (var file in repo)
            {
                bookFiles.Add(await httpClient.GetStringAsync(file.DownloadUrl));
            }

            return bookFiles;
        }
    }
}

