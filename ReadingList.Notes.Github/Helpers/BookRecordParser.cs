using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Markdig;
using Readinglist.Notes.Logic.Models;

namespace ReadingList.Notes.Github.Helpers
{
    public static class BookRecordParser
    {
        /// Markdown format: 
        ///  # Title
        ///          
        /// ![](picture)
        ///         
        /// ### Metadata
        /// 
        /// - Author: Author 1, Author 2
        /// - Full Title: Title
        /// - Category: #book
        /// 
        /// ### Highlights
        /// - Some Note or quote
        /// - Some Note or quote
        /// - Some Note or quote
        /// - Some Note or quote
        public static BookRecord CreateBookRecordFromMarkdown(string markdown)
        {
            var plainText = Markdown.ToPlainText(markdown);

            return new BookRecord(
                MapTitle(plainText),
                MapAuthors(plainText),
                MapNotes(plainText));
        }

        private static string MapTitle(string plainText)
        {
            var titleMatchingRegex = @"(?<=Full Title:\s)(\w+).*";
            return Regex.Match(plainText, titleMatchingRegex).Groups[0].Value;
        }

        private static IEnumerable<string> MapAuthors(string plainText)
        {
            var authorsMatchingRegex = @"(?<=Author:\s)(\w+).*";
            var authorsDelimiter = ',';
            var authorsMatch = Regex.Match(plainText, authorsMatchingRegex).Groups[0].Value;
            return authorsMatch.Split(authorsDelimiter).Select(author => author.Trim()).ToList();
        }

        private static IEnumerable<string> MapNotes(string plainText)
        {
            var notes = plainText[plainText.IndexOf("Highlights", StringComparison.Ordinal)..].Split(Environment.NewLine).ToList();
            notes.Remove(string.Empty);
            notes.Remove("Highlights");

            return notes;
        }
    }
}

