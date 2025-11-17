using MiniLibrary.Domain.Common;
using MiniLibrary.Domain.Entities.Books;

namespace MiniLibrary.Domain.Entities.Borrows;

public class BorrowItem : BaseEntity
{
    public int BorrowId { get; private set; }
    public Borrow Borrow { get; private set; }
    public int BookId { get; private set; }
    public Book Book { get; private set; }
    public DateTime? ReturnDate { get; private set; }
}
