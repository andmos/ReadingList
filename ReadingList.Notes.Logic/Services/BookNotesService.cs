using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Readinglist.Notes.Logic.Models;

namespace Readinglist.Notes.Logic.Services
{
    public class BookNotesService : IBookNotesService
    {
        private readonly IBookRecordRepository _bookRecordRepository;
        private readonly Random _randomizer;

        public BookNotesService(IBookRecordRepository bookRecordRepository)
        {
            _randomizer = new Random();
            _bookRecordRepository = bookRecordRepository;
        }

        public async Task<IEnumerable<BookRecord>> GetAllBookNotes()
        {
            return await _bookRecordRepository.GetAllBookRecords();
        }

        public async Task<BookNote> GetRandomBookNote()
        {
            var records = (await _bookRecordRepository.GetAllBookRecords()).ToList();
            var randomRecord = records.ElementAt(_randomizer.Next(0, records.Count));
            var randomNoteFromBook = randomRecord.Notes.ElementAt(_randomizer.Next(0, randomRecord.Notes.Count()));

            return new BookNote(randomRecord.Title, randomRecord.Authors, randomNoteFromBook);
        }
    }
}

