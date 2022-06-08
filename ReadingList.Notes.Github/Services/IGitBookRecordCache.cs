using System.Collections.Generic;
using ReadingList.Notes.Github.Models;

namespace ReadingList.Notes.Github.Services
{
    public interface IGitBookRecordCache
    {
        bool TryAdd(string key, GitBookRecord bookRecord);
        bool TryGetValue(string key, out GitBookRecord bookRecord);
        IEnumerable<GitBookRecord> GetAll();
    }
}