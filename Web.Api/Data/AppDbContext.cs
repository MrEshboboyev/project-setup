using Microsoft.EntityFrameworkCore;
using Web.Api.Entities;

namespace Web.Api.Data;

internal sealed class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Product> Products => Set<Product>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    => modelBuilder.Entity<Product>(builder =>
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Name).IsRequired().HasMaxLength(100);
            builder.Property(p => p.Description).HasMaxLength(500);
            builder.Property(p => p.Price).HasPrecision(18, 2);
            builder.Property(p => p.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        });
}
