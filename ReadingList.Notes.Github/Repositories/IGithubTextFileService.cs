using System;
using System.Threading.Tasks;
using Octokit;

namespace ReadingList.Notes.Github.Repositories
{
    public interface IGithubTextFileService
    {
        Task<string> GetRawBookRecord(RepositoryContent content);
    }
}

