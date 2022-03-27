using System;
using System.Linq;
using System.Threading.Tasks;
using Readinglist.Notes.Logic.Models;

namespace Readinglist.Notes.Logic.Services
{
    public class BookNotesService : IBookNotesService
    {
        private readonly IBookRecordRepository _bookRecordRepository;

        public BookNotesService(IBookRecordRepository bookRecordRepository)
        {
            _bookRecordRepository = bookRecordRepository;
        }

        public Task<BookNote> GetRandomBookNote()
        {
            var randomizer = new Random();
            var records = _bookRecordRepository.GetAllBookRecords();
            var randomRecord = records.ElementAt(randomizer.Next(0, records.Count()));
            var randomNoteFromBook = randomRecord.Notes.ElementAt(randomizer.Next(0, randomRecord.Notes.Count()));

            return Task.FromResult(new BookNote(randomRecord.BookTitle, randomRecord.Authors, randomNoteFromBook));

        }
    }
}

