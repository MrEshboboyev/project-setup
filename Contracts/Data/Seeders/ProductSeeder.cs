using Bogus;
using Contracts.Date;
using Contracts.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Contracts.Data.Seeders;

public sealed class ProductSeeder(
    AppDbContext context, 
    ILogger<ProductSeeder> logger,
    IDateTimeFaker dateTimeFaker)
    : SeederBase(context, logger, dateTimeFaker)
{
    public override async Task SeedAsync()
    {
        if (await _context.Products.AnyAsync())
        {
            return;
        }

        _logger.LogInformation("🌱 Seeding Products...");

        Faker<Product>? faker = new Faker<Product>()
            .RuleFor(p => p.Name, f => f.Commerce.ProductName())
            .RuleFor(p => p.Description, f => f.Commerce.ProductDescription())
            .RuleFor(p => p.Price, f => f.Random.Decimal(10, 1000))
            .RuleFor(p => p.CreatedAt, _ => _dateTimeFaker.UtcPast());

        List<Product>? products = faker.Generate(1000);
        await _context.Products.AddRangeAsync(products);
        await _context.SaveChangesAsync();

        _logger.LogInformation("✅ Seeded 1000 Products.");
    }
}
