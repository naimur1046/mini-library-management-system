namespace MiniLibrary.Domain.Entities.Books;

using MiniLibrary.Domain.Common;

public class Book : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public string ISBN { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public int CopiesAvailable { get; private set; }
    public int PublishedYear { get; set; }
    public bool IsAvailable { get; private set; }
}