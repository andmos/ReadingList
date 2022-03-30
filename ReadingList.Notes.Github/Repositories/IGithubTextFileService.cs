using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Octokit;
using Readinglist.Notes.Logic.Models;

namespace ReadingList.Notes.Github.Repositories
{
    public interface IGithubBookRecordService
    {
        Task<IEnumerable<BookRecord>> GetAllBookRecords();
    }
}

