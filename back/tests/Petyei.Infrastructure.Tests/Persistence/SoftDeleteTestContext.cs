using Microsoft.EntityFrameworkCore;
using Petyei.Domain.Common;
using Petyei.Infrastructure.Persistence;

namespace Petyei.Infrastructure.Tests.Persistence;

public sealed class Owner : SoftDeletableEntity
{
    public Guid Id { get; init; } = Guid.NewGuid();

    public required string Email { get; init; }

    public List<Note> Notes { get; } = [];
}

public sealed class Note : SoftDeletableEntity
{
    public Guid Id { get; init; } = Guid.NewGuid();

    public Guid OwnerId { get; init; }

    public required string Text { get; init; }
}

/// <summary>Test-only model that exercises the platform conventions on a real database.</summary>
public sealed class SoftDeleteTestContext(DbContextOptions<SoftDeleteTestContext> options) : DbContext(options)
{
    public DbSet<Owner> Owners => Set<Owner>();

    public DbSet<Note> Notes => Set<Note>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Owner>().HasActiveUniqueIndex(owner => owner.Email);
        modelBuilder.Entity<Owner>().HasMany(owner => owner.Notes).WithOne().HasForeignKey(note => note.OwnerId);
        modelBuilder.ApplyPlatformConventions();
    }
}
