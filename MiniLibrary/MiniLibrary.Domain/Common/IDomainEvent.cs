namespace MiniLibrary.Domain.Common;

public interface IDomainEvent
{
    DateTime OccurredOn { get; }
}