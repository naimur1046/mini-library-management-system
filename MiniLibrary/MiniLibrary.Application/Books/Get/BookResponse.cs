namespace MiniLibrary.Application.Books.Get;

public sealed class BookResponse
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public string ISBN { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public int CopiesAvailable { get; set; }
    public int PublishedYear { get; set; }
    public bool IsAvailable { get; set; }
    public DateTime CreatedOnUtc { get; set; }
}
