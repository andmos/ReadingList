namespace ReadingList.Notes.Github.Helpers
{
    public record MarkdownFile(string Content, string FileName)
    {
        public string Content { get; } = Content;
        public string FileName { get; } = FileName;
    }
}