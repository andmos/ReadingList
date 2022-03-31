using System.Collections.Concurrent;
using System.Collections.Generic;
using Readinglist.Notes.Logic.Models;

namespace ReadingList.Notes.Github.Services
{
    public class BookRecordCache : IBookRecordCache
    {
        private readonly ConcurrentDictionary<string, BookRecord> _cache;
        public BookRecordCache()
        {
            _cache = new ConcurrentDictionary<string, BookRecord>();
        }
        public bool TryAdd(string key, BookRecord bookRecord)
        {
            return _cache.TryAdd(key, bookRecord);
        }

        public bool TryGetValue(string key, out BookRecord bookRecord)
        {
            return _cache.TryGetValue(key, out bookRecord);
        }

        public IEnumerable<BookRecord> GetAll()
        {
            return _cache.Values;
        }
    }
}