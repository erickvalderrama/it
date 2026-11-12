using Microsoft.EntityFrameworkCore;

namespace Petyei.Infrastructure.Persistence;

/// <summary>
/// Single database context for the modular monolith. Entity configurations are added per module
/// once the approved entity relationship model covers them.
/// </summary>
public sealed class PetyeiDbContext(DbContextOptions<PetyeiDbContext> options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ArgumentNullException.ThrowIfNull(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PetyeiDbContext).Assembly);
        modelBuilder.ApplyPlatformConventions();
    }
}
