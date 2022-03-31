using System.Collections.Generic;
using Readinglist.Notes.Logic.Models;

namespace ReadingList.Notes.Github.Services
{
    public interface IBookRecordCache
    {
        bool TryAdd(string key, BookRecord bookRecord);
        bool TryGetValue(string key, out BookRecord bookRecord);
        IEnumerable<BookRecord> GetAll();
    }
}