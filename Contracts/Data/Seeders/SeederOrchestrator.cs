using Microsoft.Extensions.Logging;

namespace Contracts.Data.Seeders;

public sealed class SeederOrchestrator(
    IEnumerable<ISeeder> seeders,
    ILogger<SeederOrchestrator> logger)
{
    public async Task ExecuteAsync()
    {
        logger.LogInformation("🚀 Starting database seeders...");

        foreach (ISeeder seeder in seeders)
        {
            string seederName = seeder.GetType().Name;
            logger.LogInformation("🔧 Running seeder: {Seeder}", seederName);

            try
            {
                await seeder.SeedAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "❌ Failed to seed: {Seeder}", seederName);
            }
        }

        logger.LogInformation("🎉 All seeders finished.");
    }
}
