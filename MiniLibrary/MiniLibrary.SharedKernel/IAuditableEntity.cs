namespace MiniLibrary.SharedKernel;

public interface IAuditableEntity
{
    DateTime CreatedOnUtc { get; set; }
    string CreatedBy { get; set; }
    DateTime? ModifiedOnUtc { get; set; }
    string? ModifiedBy { get; set; }
}
