using System;
using System.Collections.Generic;
using System.Linq;
using Markdig;
using Readinglist.Notes.Logic.Models;

namespace ReadingList.Notes.Github.Helpers
{
    public static class BookRecordMapper
    {
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
            return string.Empty;
        }

        private static IEnumerable<string> MapAuthors(string plainText)
        {
            return new List<string>();
        }

        private static List<string> MapNotes(string plainText)
        {
            var notes = plainText.Substring(plainText.IndexOf("Highlights")).Split(Environment.NewLine).ToList();
            notes.Remove(string.Empty);
            notes.Remove("Highlights");

            return notes;
        }
    }
}

