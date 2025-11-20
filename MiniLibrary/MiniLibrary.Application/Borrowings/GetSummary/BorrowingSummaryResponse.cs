namespace MiniLibrary.Application.Borrowings.GetSummary;

public sealed class BorrowingSummaryResponse
{
    public int TotalBooksBorrowed { get; set; }
    public int TotalBooksReturned { get; set; }
    public int ActiveBorrowRecords { get; set; }
    public MostBorrowedBookResponse? MostBorrowedBook { get; set; }
}

public sealed class MostBorrowedBookResponse
{
    public Guid BookId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public int BorrowCount { get; set; }
}
