using Readinglist.Notes.Logic.Models;

namespace ReadingList.Notes.Github.Models;

public record GitBookRecord
{
    public string Sha { get; init; } 
    public string FileName { get; init;}
    public BookRecord BookRecord { get; init; }
}