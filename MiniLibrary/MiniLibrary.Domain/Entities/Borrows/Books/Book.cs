using MiniLibrary.SharedKernel;

namespace MiniLibrary.Domain.Books;

public sealed class Book : Entity, IAuditableEntity, ISoftDeletableEntity
{
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public string ISBN { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public int CopiesAvailable { get; set; }
    public int PublishedYear { get; set; }
    public bool IsAvailable { get; set; }
    public DateTime CreatedOnUtc { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public DateTime? ModifiedOnUtc { get; set; }
    public string? ModifiedBy { get; set; }
    public DateTime? DeletedOnUtc { get; set; }
    public string? DeletedBy { get; set; }
    public bool IsDeleted { get; set; }
}