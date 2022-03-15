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
        private char _bookTitleDelimitor => '-';
        private char _authorsDelimitor => ',';

        private ILog _logger; 

        public BookMapper(ILogFactory logFactory)
        {
            _logger = logFactory.GetLogger(this.GetType());
        }

        public Book Create(string bookString, string listLabel)
        {

            var bookArray = bookString.Split(_bookTitleDelimitor);

            return new Book(
                string.Join(_bookTitleDelimitor.ToString(), bookArray.Take(bookArray.Length - 1)).Trim(),
                ExtractAuthors(bookArray.Last()), MapBookTypeLabel(listLabel));
        }

        public Book Create(string jsonString)
        {
            try
            {
                return JsonConvert.DeserializeObject<Book>(jsonString);
            }
            catch (JsonSerializationException serializationExeption)
            {
                _logger.Error("Could not create Book from JSON string", serializationExeption);
                throw serializationExeption;
            }
        }

        private IEnumerable<string> ExtractAuthors(string authors)
        {
            return authors.Split(_authorsDelimitor).Select(author => author.Trim()).ToList();
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
