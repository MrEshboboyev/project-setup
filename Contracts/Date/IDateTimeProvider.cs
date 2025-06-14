namespace Contracts.Date;

public interface IDateTimeProvider
{
    DateTime UtcNow { get; }
}
