using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using ReadingList.Logic.Models;
using ReadingList.Logic.Services;
using ReadingList.Logging;

namespace ReadingList.Trello.Helpers
{
    public class BookMapper : IBookFactory
    {
        private char _bookTitleDelimiter => '-';
        private char _authorsDelimiter => ',';

        private ILog _logger; 

        public BookMapper(ILogFactory logFactory)
        {
            _logger = logFactory.GetLogger(this.GetType());
        }

        public Book Create(string bookString, string listLabel)
        {

            var bookArray = bookString.Split(_bookTitleDelimiter);

            return new Book(
                ExtractTitle(bookArray),
                ExtractAuthors(bookArray.Last()), MapBookTypeLabel(listLabel));
        }

        public Book Create(string jsonString)
        {
            try
            {
                return JsonConvert.DeserializeObject<Book>(jsonString);
            }
            catch (JsonSerializationException serializationException)
            {
                _logger.Error("Could not create Book from JSON string", serializationException);
                throw serializationException;
            }
        }

        private string ExtractTitle(string[] bookArray)
        {
            // To support titles with the title-author delimiter in it, the string needs to be joined on it and last element (after the actual delimiter)
            // containing authors taken out of the array.
            return string.Join(_bookTitleDelimiter.ToString(), bookArray.Take(bookArray.Length - 1)).Trim();
        }

        private IEnumerable<string> ExtractAuthors(string authors)
        {
            return authors.Split(_authorsDelimiter).Select(author => author.Trim()).ToList();
        }

        private Label MapBookTypeLabel(string label)
        {
            try
            {
                return (Label)Enum.Parse(typeof(Label), label, true);
            }
            catch(ArgumentException ex)
            {
                _logger.Error($"Could not parse the label to any known labels: {label}", ex);
                return Label.Unspecified;
            }
        }
    }
}
