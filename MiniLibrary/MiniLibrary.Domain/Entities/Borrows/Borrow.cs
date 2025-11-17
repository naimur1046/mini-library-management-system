using MiniLibrary.Domain.Common;
using MiniLibrary.Domain.Entities.Members;
using MiniLibrary.Domain.Entities.Books;

namespace MiniLibrary.Domain.Entities.Borrows;

public class Borrow : BaseEntity
{
    public int MemberId { get; private set; }
    public Member Member { get; private set; }
    public DateTime BorrowDate { get; private set; }
    public DateTime DueDate { get; private set; }

    private readonly List<BorrowItem> _borrowItems = new();
    public IReadOnlyCollection<BorrowItem> BorrowItems => _borrowItems.AsReadOnly();

    public bool AllReturned() => _borrowItems.All(bi => bi.ReturnDate.HasValue);
}
