using Microsoft.EntityFrameworkCore;

namespace Web.Api.Data;

internal static class MigrationAutoRunner
{
    public static async Task EnsureMigrationAppliedAsync(this IHost host)
    {
        using IServiceScope scope = host.Services.CreateScope();

        AppDbContext dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        ILogger logger = scope.ServiceProvider.GetRequiredService<ILoggerFactory>()
            .CreateLogger(nameof(MigrationAutoRunner));

        try
        {
            logger.LogInformation("🚀 Applying pending migrations...");
            await dbContext.Database.MigrateAsync();
            logger.LogInformation("✅ Migrations applied successfully.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "❌ Migration failed during EnsureEfMigrationAsync");
            throw new InvalidOperationException("Migration failed in MigrationAutoRunner", ex);
        }
    }
}
