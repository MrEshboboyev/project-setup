using Web.Api.Data.Seeders;

namespace Web.Api.Data;

internal static class AppDbSeeder
{
    public static async Task SeedAsync(this IHost host)
    {
        using IServiceScope scope = host.Services.CreateScope();

        SeederOrchestrator orchestrator = scope.ServiceProvider.GetRequiredService<SeederOrchestrator>();
        await orchestrator.ExecuteAsync();
    }
}
