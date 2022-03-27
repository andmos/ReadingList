using System.Collections.Generic;
using Readinglist.Notes.Logic.Models;

namespace Readinglist.Notes.Logic.Services
{
    public class DummyBookRecordRepository : IBookRecordRepository
    {
        private List<BookRecord> _dummyBookRecords;
        public DummyBookRecordRepository()
        {
            _dummyBookRecords = new List<BookRecord> {
                new BookRecord(
                    bookTitle: "Waking Up: A Guide to Spirituality Without Religion",
                    authors: new[] {"Sam Harris"},
                    notes: new []
                    {
                        "Every experience you have ever had has been shaped by your mind. Every relationship is as good or as bad as it is because of the minds involved. If you are perpetually angry, depressed, confused, and unloving, or your attention is elsewhere, it won’t matter how successful you become or who is in your life—you won’t enjoy any of it.",
                        "The word spirit comes from the Latin spiritus, which is a translation of the Greek pneuma, meaning “breath.” Around the thirteenth century, the term became entangled with beliefs about immaterial souls, supernatural beings, ghosts, and so forth. It acquired other meanings as well: We speak of the spirit of a thing as its most essential principle or of certain volatile substances and liquors as spirits. Nevertheless, many nonbelievers now consider all things “spiritual” to be contaminated by medieval superstition.",
                        "Beyond ensuring our survival, civilization is a vast machine invented by the human mind to regulate its states. We are ever in the process of creating and repairing a world that our minds want to be in.",
                        "There is now little question that how one uses one’s attention, moment to moment, largely determines what kind of person one becomes."
                    }),
                new BookRecord(
                    bookTitle: "Think Again: The Power of Knowing What You Don't Know",
                    authors: new[] {"Adam Grant"},
                    notes: new []
                    {
                        "The higher IQ people have, the better they are at pattern matching, and thus are quicker to fall for stereotypes.",
                        "If Knowledge is power, knowing what we don't know is wisdom.",
                        "When we lack the knowledge and skills to archive excellence, we sometimes lack the knowledge and skills to judge excellence.",
                        "Who you are should be a question about what you value, not what you believe.",
                        "It's a sign of wisdom to avoid believing every thought that enters your mind."
                    }) };
        }
        public IEnumerable<BookRecord> GetAllBookRecords() => _dummyBookRecords;
    }
}

