namespace ReadingList.Notes.Github.Models
{
    public record MarkdownFile(string Content)
    {
        public string Content { get; } = Content;
    }
}