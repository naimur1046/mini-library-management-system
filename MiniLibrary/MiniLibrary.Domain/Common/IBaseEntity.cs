using MiniLibrary.Domain.Enums;
namespace MiniLibrary.Domain.Common;

public interface IBaseEntity
{
    int Id { get; set; }
    DateTime CreationDate { get; set; }
    DateTime ModificationDate { get; set; }
    EntityStatus Status { get; set; }
    string CreatedBy { get; set; }
    string CreatedByName { get; set; }
    string ModifiedBy { get; set; }
    string ModifiedByName { get; set; }
    long Version { get; set; }
    
    void IncrementVersion();
}