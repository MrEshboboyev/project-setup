namespace Contracts.Date;

public interface IDateTimeFaker
{
    DateTime UtcPast(int years = 1);
    DateTime UtcFuture(int years = 1);
    DateTime UtcRecent(int days = 30);
    DateTime UtcSoon(int days = 30);
}
