using Microsoft.EntityFrameworkCore;
using Web.Api.Data;
using Web.Api.Data.Seeders;
using Web.Api.Date;
using Web.Api.Infrastructure;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// 🔧 Add services
builder.AddServiceDefaults();
builder.Services
    .AddControllers()
    .ConfigureApplicationPartManager(manager 
        => manager.FeatureProviders.Add(new InternalControllerFeatureProvider()));

builder.Services.AddOpenApi();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Database")));

builder.Services.AddScoped<ISeeder, ProductSeeder>();
builder.Services.AddScoped<SeederOrchestrator>();

builder.Services.AddSingleton<IDateTimeProvider, SystemDateTimeProvider>();
builder.Services.AddSingleton<IDateTimeFaker, DateTimeFaker>();

WebApplication app = builder.Build();

// 🌐 Configure endpoints & middleware
app.MapDefaultEndpoints();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

await app.EnsureMigrationAppliedAsync(); // 🛠 Apply migrations

await app.SeedAsync(); // 🌱 Seed database

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

await app.RunAsync();
