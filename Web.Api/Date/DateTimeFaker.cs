using Bogus;

namespace Web.Api.Date;

internal sealed class DateTimeFaker(IDateTimeProvider clock) : IDateTimeFaker
{
    private readonly Faker _faker = new();

    public DateTime UtcPast(int years = 1)
    {
        DateTime local = _faker.Date.Past(years);
        return ConvertToUtc(local);
    }

    public DateTime UtcFuture(int years = 1)
    {
        DateTime local = _faker.Date.Future(years);
        return ConvertToUtc(local);
    }

    public DateTime UtcRecent(int days = 30)
    {
        DateTime local = _faker.Date.Recent(days);
        return ConvertToUtc(local);
    }

    public DateTime UtcSoon(int days = 30)
    {
        DateTime local = _faker.Date.Soon(days);
        return ConvertToUtc(local);
    }

    private DateTime ConvertToUtc(DateTime local)
    {
        TimeSpan utcOffset = clock.UtcNow - DateTime.UtcNow;
        return DateTime.SpecifyKind(local + utcOffset, DateTimeKind.Utc);
    }
}
