using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Markdig;
using ReadingList.Notes.Github.Models;
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
        public static BookRecord CreateBookRecordFromMarkdown(MarkdownFile markdownFile)
        {
            var plainText = Markdown.ToPlainText(markdownFile.Content);

            return new BookRecord(
                MapTitle(plainText),
                MapAuthors(plainText),
                MapNotes(plainText));
        }

        private static string MapTitle(string plainText)
        {
            const string titleMatchingRegex = @"(?<=Full Title:\s)(\w+).*";
            return Regex.Match(plainText, titleMatchingRegex).Groups[0].Value;
        }

        private static IEnumerable<string> MapAuthors(string plainText)
        {
            const string authorsMatchingRegex = @"(?<=Author:\s)(\w+).*";
            const char authorsDelimiter = ',';
            var authorsMatch = Regex.Match(plainText, authorsMatchingRegex).Groups[0].Value;
            return authorsMatch.Split(authorsDelimiter).Select(author => author.Trim());
        }

        private static IEnumerable<string> MapNotes(string plainText)
        {
            const string regexLocationTag = @"\(Location.*?\)";
            var notes = plainText[plainText.IndexOf("Highlights", StringComparison.Ordinal)..]
                .Split(Environment.NewLine)
                .Select(l => Regex.Replace(l, regexLocationTag, string.Empty).TrimEnd()).ToList();

            notes.Remove(string.Empty);
            notes.Remove("Highlights");

            return notes;
        }
    }
}

