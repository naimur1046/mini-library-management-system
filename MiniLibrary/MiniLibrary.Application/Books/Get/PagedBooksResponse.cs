namespace MiniLibrary.Application.Books.Get;

public sealed class PagedBooksResponse
{
    public List<BookResponse> Books { get; set; } = [];
    public bool HasMore { get; set; }
}
