using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using ReadingList.Logic.Models;

namespace ReadingList.Trello.Helpers
{
    public static class BookMapper
    {
        private static char _bookTitleDelimiter => '-';
        private static char _authorsDelimiter => ',';

        public static Book CreateBook(string bookString, string listLabel)
        {

            var bookArray = bookString.Split(_bookTitleDelimiter);

            return new Book(
                ExtractTitle(bookArray),
                ExtractAuthors(bookArray.Last()), MapBookTypeLabel(listLabel));
        }

        public static Label MapBookTypeLabel(string label)
        {
            try
            {
                return (Label)Enum.Parse(typeof(Label), label, true);
            }
            catch(ArgumentException)
            {
                return Label.Unspecified;
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
