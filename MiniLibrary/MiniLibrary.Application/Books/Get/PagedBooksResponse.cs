namespace MiniLibrary.Application.Books.Get;

public sealed class PagedBooksResponse
{
    public List<BookResponse> Books { get; set; } = [];
    public Guid? NextCursor { get; set; }
    public Guid? PreviousCursor { get; set; }
    public bool HasMore { get; set; }
    public int TotalReturned { get; set; }
}
