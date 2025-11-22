namespace MiniLibrary.Application.Borrowings.GetOverdue;

public sealed class OverdueBorrowingResponse
{
    public Guid BorrowId { get; set; }
    public Guid MemberId { get; set; }
    public string MemberName { get; set; } = string.Empty;
    public string MemberEmail { get; set; } = string.Empty;
    public DateTime DueDate { get; set; }
    public int DaysOverdue { get; set; }
    public List<OverdueBookItem> OverdueBooks { get; set; } = new();
}

public sealed class OverdueBookItem
{
    public Guid BookId { get; set; }
    public string BookTitle { get; set; } = string.Empty;
    public string BookAuthor { get; set; } = string.Empty;
}
