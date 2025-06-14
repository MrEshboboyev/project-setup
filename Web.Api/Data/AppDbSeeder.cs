using Microsoft.EntityFrameworkCore;
using Bogus;
using Web.Api.Entities;

namespace Web.Api.Data;

internal static class AppDbSeeder
{
    public static async Task SeedAsync(this IHost host)
    {
        using IServiceScope scope = host.Services.CreateScope();
        AppDbContext context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        ILogger logger = scope.ServiceProvider.GetRequiredService<ILoggerFactory>()
            .CreateLogger(nameof(AppDbSeeder));

        if (await context.Products.AnyAsync())
        {
            return;
        }

        logger.LogInformation("🌱 Seeding Product table...");

        Faker<Product> productFaker = new Faker<Product>()
            .RuleFor(p => p.Name, f => f.Commerce.ProductName())
            .RuleFor(p => p.Description, f => f.Commerce.ProductDescription())
            .RuleFor(p => p.Price, f => f.Random.Decimal(10, 1000))
            .RuleFor(p => p.CreatedAt, f => f.Date.Past());

        List<Product> products = productFaker.Generate(1000);

        await context.Products.AddRangeAsync(products);
        await context.SaveChangesAsync();

        logger.LogInformation("✅ Seeding completed with 1000 products.");
    }
}
