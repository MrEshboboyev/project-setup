using Contracts.Date;
using Microsoft.Extensions.Logging;

namespace Contracts.Data.Seeders;

public abstract class SeederBase(
    AppDbContext context,
    ILogger logger,
    IDateTimeFaker dateTimeFaker)
    : ISeeder
{
    private protected readonly AppDbContext _context = context;
    private protected readonly ILogger _logger = logger;
    private protected readonly IDateTimeFaker _dateTimeFaker = dateTimeFaker;

    public abstract Task SeedAsync();
}
