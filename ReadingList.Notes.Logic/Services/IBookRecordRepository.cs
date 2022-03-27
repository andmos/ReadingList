using System;
using System.Collections.Generic;
using Readinglist.Notes.Logic.Models;

namespace Readinglist.Notes.Logic.Services
{
    public interface IBookRecordRepository
    {
        IEnumerable<BookRecord> GetAllBookRecords();
    }
}

