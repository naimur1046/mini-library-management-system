using MiniLibrary.SharedKernel;
using Domain.Books;

namespace Domain.Borrows;

public sealed class BorrowItem : Entity, IAuditableEntity
{
    public Guid BorrowId { get; set; }
    public Borrow Borrow { get; set; } = null!;
    public Guid BookId { get; set; }
    public Book Book { get; set; } = null!;
    public DateTime? ReturnDate { get; set; }
    public DateTime CreatedOnUtc { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public DateTime? ModifiedOnUtc { get; set; }
    public string? ModifiedBy { get; set; }
}
