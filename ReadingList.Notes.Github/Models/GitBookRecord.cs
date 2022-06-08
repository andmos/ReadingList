using Readinglist.Notes.Logic.Models;

namespace ReadingList.Notes.Github.Models;

public record GitBookRecord(string Sha, string FileName, BookRecord BookRecord)
{
    public string Sha { get; } = Sha;
    public string FileName { get; } = FileName;
    public BookRecord BookRecord { get; } = BookRecord;
}