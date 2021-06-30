using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using ReadingList.Logic.Models;
using ReadingList.Logic.Services;

namespace ReadingList.Trello.Helpers
{
    public class BookMapper : IBookFactory
    {
        private char _bookTitleDelimitor => '-';
        private char _authorsDelimitor => ',';

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
                throw serializationExeption;
            }
        }

        private IEnumerable<string> ExtractAuthors(string authors)
        {
            return authors.Split(_authorsDelimitor).Select(author => author.Trim()).ToList();

        }

        private Label MapBookTypeLabel(string label)
        {
            if (string.IsNullOrEmpty(label)) return Label.None;
            try
            {
                return (Label)Enum.Parse(typeof(Label), label, true);
            }
            catch(ArgumentException ex)
            {
                return Label.None;
            }
        }
    }
}
