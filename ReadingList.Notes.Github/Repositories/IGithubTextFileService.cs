using System;
using System.Threading.Tasks;
using Octokit;
using Readinglist.Notes.Logic.Models;

namespace ReadingList.Notes.Github.Repositories
{
    public interface IGithubTextFileService
    {
        Task<BookRecord> GetBookRecord(RepositoryContent content);
    }
}

