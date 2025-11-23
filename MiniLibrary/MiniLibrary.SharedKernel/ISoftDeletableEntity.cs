namespace MiniLibrary.SharedKernel;

public interface ISoftDeletableEntity
{
    DateTime? DeletedOnUtc { get; set; }
    string? DeletedBy { get; set; }
    bool IsDeleted { get; set; }
}
