using System;
using System.Collections.Generic;
using System.Linq;
using Manatee.Trello;
using ReadingList.Logic.Models;

namespace ReadingList.Trello.Helpers
{
    public static class BookMapper
    {
        private static char _bookTitleDelimiter => '-';
        private static char _authorsDelimiter => ',';

        public static Book CreateBook(string bookString, string listLabel, ICard card = null)
        {
            var bookArray = bookString.Split(_bookTitleDelimiter);

            DateTime? dateStarted = null;
            DateTime? dateFinished = null;

            if (card != null)
            {
                var listMoveActions = card.Actions
                    .Where(a => a.Type == ActionType.UpdateCard && a.Data.ListAfter != null)
                    .OrderBy(a => a.Date)
                    .ToList();

                dateStarted = listMoveActions
                    .LastOrDefault(a => a.Data.ListAfter?.Name == ReadingList.Logic.Models.ReadingListConstants.CurrentlyReading)
                    ?.Date;

                dateFinished = listMoveActions
                    .LastOrDefault(a => a.Data.ListAfter?.Name == ReadingList.Logic.Models.ReadingListConstants.DoneReading)
                    ?.Date;
            }

            return new Book(
                ExtractTitle(bookArray),
                ExtractAuthors(bookArray.Last()),
                MapBookTypeLabel(listLabel),
                dateStarted,
                dateFinished);
        }

        public static ReadingList.Logic.Models.Label MapBookTypeLabel(string label)
        {
            try
            {
                return (ReadingList.Logic.Models.Label)Enum.Parse(typeof(ReadingList.Logic.Models.Label), label, true);
            }
            catch(ArgumentException)
            {
                return ReadingList.Logic.Models.Label.Unspecified;
            }
        }

        private static string ExtractTitle(string[] bookArray)
        {
            // To support titles with the title-author delimiter in it, the string needs to be joined on it and last element (after the actual delimiter)
            // containing authors taken out of the array.
            return string.Join(_bookTitleDelimiter.ToString(), bookArray.Take(bookArray.Length - 1)).Trim();
        }

        private static IEnumerable<string> ExtractAuthors(string authors)
        {
            return authors.Split(_authorsDelimiter).Select(author => author.Trim()).ToList();
        }
    }
}
