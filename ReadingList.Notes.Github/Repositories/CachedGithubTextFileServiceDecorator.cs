using System.Collections.Concurrent;
using System.Threading.Tasks;
using Octokit;
using Readinglist.Notes.Logic.Models;

namespace ReadingList.Notes.Github.Repositories
{
    public class CachedGithubTextFileServiceDecorator : IGithubTextFileService
    {

        private readonly ConcurrentDictionary<string, BookRecord> _cache;
        private readonly IGithubTextFileService _githubTextFileService;

        public CachedGithubTextFileServiceDecorator(IGithubTextFileService githubTextFileService)
        {
            _cache = new ConcurrentDictionary<string, BookRecord>();
            _githubTextFileService = githubTextFileService;
        }

        public async Task<BookRecord> GetBookRecord(RepositoryContent content)
        {
            if (_cache.TryGetValue(content.Sha, out BookRecord bookRecord))
            {
                return bookRecord;
            }
            else
            {
                bookRecord = await _githubTextFileService.GetBookRecord(content);
                _cache.TryAdd(content.Sha, bookRecord);
                return bookRecord;
            }
        }
    }
}

