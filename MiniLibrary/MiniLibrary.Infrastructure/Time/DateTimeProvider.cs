using MiniLibrary.SharedKernel;

namespace MiniLibrary.Infrastructure.Time;

internal sealed class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}
