using Readinglist.Notes.Logic.Models;

namespace ReadingList.Notes.Github.Models;

public record GitBookRecord(string Sha, string FileName, BookRecord BookRecord);
