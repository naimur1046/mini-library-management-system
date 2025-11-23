using MiniLibrary.SharedKernel;
using Domain.Members;

namespace Domain.Borrows;

public sealed class Borrow : Entity, IAuditableEntity
{
    public Guid MemberId { get; set; }
    public Member Member { get; set; } = null!;
    public DateTime BorrowDate { get; set; }
    public DateTime DueDate { get; set; }
    public ICollection<BorrowItem> BorrowItems { get; set; } = new List<BorrowItem>();
    public DateTime CreatedOnUtc { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public DateTime? ModifiedOnUtc { get; set; }
    public string? ModifiedBy { get; set; }
}
