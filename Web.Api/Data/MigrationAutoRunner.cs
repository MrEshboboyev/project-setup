using System.Diagnostics;
using System.Globalization;

namespace Web.Api.Data;

internal sealed record MigrationAutoRunner
{
    public static async Task EnsureEfMigrationAsync(ILogger<MigrationAutoRunner> logger)
    {
        if (!await EfInstalledAsync(logger).ConfigureAwait(false))
        {
            logger.LogWarning("🛠 dotnet-ef was not found. Installing...");
            await InstallEfToolAsync(logger).ConfigureAwait(false);
        }

        bool hasMigrations = Directory.Exists("Migrations")
            && Directory.GetFiles("Migrations", "*.cs")
                        .Any(file => !file.EndsWith("Snapshot.cs", StringComparison.OrdinalIgnoreCase));

        if (!hasMigrations)
        {
            logger.LogInformation("⚙️ Creating initial AUTO migration...");
            await RunDotNetEfAsync("migrations add Initial_AUTO", logger).ConfigureAwait(false);
        }

        logger.LogInformation("📥 Applying migrations to the database...");
        await RunDotNetEfAsync("database update", logger).ConfigureAwait(false);
        logger.LogInformation("✅ Database updated successfully.");
    }

    private static async Task<bool> EfInstalledAsync(ILogger logger)
    {
        var checkInfo = new ProcessStartInfo
        {
            FileName = "dotnet",
            Arguments = "tool list -g",
            RedirectStandardOutput = true,
            UseShellExecute = false
        };

        using Process process = Process.Start(checkInfo)
            ?? throw new InvalidOperationException("Failed to run 'dotnet tool list -g' command.");

        string output = await process.StandardOutput.ReadToEndAsync().ConfigureAwait(false);
        await process.WaitForExitAsync().ConfigureAwait(false);

        bool result = output.Contains("dotnet-ef", StringComparison.OrdinalIgnoreCase);
        logger.LogDebug("dotnet-ef installed: {Result}", result);
        return result;
    }

    private static async Task InstallEfToolAsync(ILogger logger)
        => await RunDotNetProcessAsync("tool install --global dotnet-ef", logger).ConfigureAwait(false);

    private static async Task RunDotNetEfAsync(string arguments, ILogger logger)
        => await RunDotNetProcessAsync(string.Format(CultureInfo.InvariantCulture, "ef {0}", arguments), logger).ConfigureAwait(false);

    private static async Task RunDotNetProcessAsync(string arguments, ILogger logger)
    {
        var processInfo = new ProcessStartInfo
        {
            FileName = "dotnet",
            Arguments = arguments,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false
        };

        using var process = new Process();
        process.StartInfo = processInfo;
        process.Start();

        string output = await process.StandardOutput.ReadToEndAsync().ConfigureAwait(false);
        string error = await process.StandardError.ReadToEndAsync().ConfigureAwait(false);
        await process.WaitForExitAsync().ConfigureAwait(false);

        if (process.ExitCode != 0)
        {
            logger.LogError("❌ 'dotnet {Arguments}' failed with error:\n{Error}", arguments, error);
            throw new InvalidOperationException(string.Format(
                CultureInfo.InvariantCulture,
                "❌ Command 'dotnet {0}' failed:\n{1}",
                arguments,
                error));
        }

        logger.LogInformation("{Output}", output);
    }
}
