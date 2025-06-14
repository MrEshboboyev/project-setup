namespace Web.Api.Date;

internal interface IDateTimeProvider
{
    DateTime UtcNow { get; }
}
