using System.Collections.Generic;
using System.Threading.Tasks;
using Readinglist.Notes.Logic.Models;

namespace Readinglist.Notes.Logic.Services
{
    public interface IBookRecordRepository
    {
        Task<IEnumerable<BookRecord>> GetAllBookRecords();
        Task<BookRecord> GetBookNotes(string book);
    }
}

