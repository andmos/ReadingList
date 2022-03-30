using System.Collections.Generic;
using Readinglist.Notes.Logic.Models;
using Readinglist.Notes.Logic.Services;
using Octokit;
using System.Linq;
using System.Threading.Tasks;
using ReadingList.Notes.Github.Helpers;

namespace ReadingList.Notes.Github.Repositories
{
    public class GithubReadingListDataService : IBookRecordRepository
    {
        private const string UserName = "andmos";
        private const string Repo = "ReadingList-Data";
        private const string NotesFolder = "BookNotes";
        private const string ApplicationName = "ReadingList.Notes";

        private readonly IGitHubClient _gitHubClient;
        private readonly IGithubTextFileService _githubTextFileService;

        public GithubReadingListDataService(IGithubTextFileService githubTextFileService)
        {
            _gitHubClient = new GitHubClient(new ProductHeaderValue(ApplicationName));
            _githubTextFileService = githubTextFileService;
        }

        public async Task<IEnumerable<BookRecord>> GetAllBookRecords()
        {
            var repo = await _gitHubClient.Repository.Content.GetAllContents(UserName, Repo, NotesFolder);

            var bookFiles = new List<BookRecord>();
            foreach (var content in repo)
            {
                bookFiles.Add(await _githubTextFileService.GetBookRecord(content));
            }

            return bookFiles;
        }
    }
}

