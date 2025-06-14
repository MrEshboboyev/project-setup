using Web.Api.Date;

namespace Web.Api.Data.Seeders;

internal abstract class SeederBase(
    AppDbContext context,
    ILogger logger,
    IDateTimeFaker dateTimeFaker)
    : ISeeder
{
    protected readonly AppDbContext _context = context;
    protected readonly ILogger _logger = logger;
    protected readonly IDateTimeFaker _dateTimeFaker = dateTimeFaker;

    public abstract Task SeedAsync();
}
