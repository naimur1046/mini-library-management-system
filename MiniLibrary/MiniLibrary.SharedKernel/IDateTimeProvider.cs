namespace MiniLibrary.SharedKernel;

public interface IDateTimeProvider
{
    DateTime UtcNow { get; }
}
