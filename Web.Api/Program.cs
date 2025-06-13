using Microsoft.EntityFrameworkCore;
using Web.Api.Data;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

WebApplication app = builder.Build();

app.MapDefaultEndpoints();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

ILogger<MigrationAutoRunner> migrationLogger = app.Services.GetRequiredService<ILogger<MigrationAutoRunner>>();
await MigrationAutoRunner.EnsureEfMigrationAsync(migrationLogger);

using (IServiceScope scope = app.Services.CreateScope())
{
    AppDbContext dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    ILogger logger = scope.ServiceProvider
        .GetRequiredService<ILoggerFactory>()
        .CreateLogger("ProductSeeder");

    await AppDbSeeder.SeedAsync(dbContext, logger);
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
await app.RunAsync();
