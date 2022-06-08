using System.Collections.Concurrent;
using System.Collections.Generic;
using ReadingList.Notes.Github.Models;

namespace ReadingList.Notes.Github.Services
{
    public class GitBookRecordCache : IBookRecordCache
    {
        private readonly ConcurrentDictionary<string, GitBookRecord> _cache;
        public GitBookRecordCache()
        {
            _cache = new ConcurrentDictionary<string, GitBookRecord>();
        }
        public bool TryAdd(string key, GitBookRecord bookRecord)
        {
            return _cache.TryAdd(key, bookRecord);
        }

        public bool TryGetValue(string key, out GitBookRecord bookRecord)
        {
            return _cache.TryGetValue(key, out bookRecord);
        }

        public IEnumerable<GitBookRecord> GetAll()
        {
            return _cache.Values;
        }
    }
}