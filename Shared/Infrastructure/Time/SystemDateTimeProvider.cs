using Shared.Application.Abstractions.Time;

namespace Shared.Infrastructure.Time;

public sealed class SystemDateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}
