using MiniLibrary.Domain.Enums;

namespace MiniLibrary.Domain.Common;

public abstract class BaseEntity : IBaseEntity
{
    private readonly List<IDomainEvent> _domainEvents = new();
    public int Id { get; set; }
    public DateTime CreationDate { get; set; }
    public DateTime ModificationDate { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public string CreatedByName { get; set; } = string.Empty;
    public string ModifiedBy { get; set; } = string.Empty;
    public string ModifiedByName { get; set; } = string.Empty;
    public EntityStatus Status { get; set; } = EntityStatus.Active;
    public long Version { get; set; }
    public IReadOnlyCollection <IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();
    
    public void IncrementVersion()
    {
        Version++;
        ModificationDate = DateTime.UtcNow;
    }

    public void MarkAsCreated(string userId, string userName)
    {
        CreationDate = DateTime.UtcNow;
        ModificationDate = DateTime.UtcNow;
        CreatedBy = userId;
        CreatedByName = userName;
        ModifiedBy = userId;
        ModifiedByName = userName;
        Status = EntityStatus.Active;
        Version = 1;
    }

    public void MarkAsModified(string userId, string userName)
    {
        ModificationDate = DateTime.UtcNow;
        ModifiedBy = userId;
        ModifiedByName = userName;
        IncrementVersion();
    }

    public void MarkAsDeleted(string userId, string userName)
    {
        Status = EntityStatus.Deleted;
        MarkAsModified(userId, userName);
    }

    public void Activate(string userId, string userName)
    {
        Status = EntityStatus.Active;
        MarkAsModified(userId, userName);
    }

    public void Deactivate(string userId, string userName)
    {
        Status = EntityStatus.Inactive;
        MarkAsModified(userId, userName);
    }

    public void MarkAsPending(string userId, string userName)
    {
        Status = EntityStatus.Pending;
        MarkAsModified(userId, userName);
    }

    public bool IsActive() => Status == EntityStatus.Active;
    public bool IsDeleted() => Status == EntityStatus.Deleted;
    public bool IsInactive() => Status == EntityStatus.Inactive;
    public bool IsPending() => Status == EntityStatus.Pending;

    protected void RaiseDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
}