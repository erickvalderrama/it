using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Time.Testing;
using Petyei.Application.Common;
using Petyei.Domain.Common;
using Petyei.Infrastructure.Persistence;

namespace Petyei.Infrastructure.Tests.Persistence;

public sealed class SoftDeleteTests(PostgresFixture database) : IClassFixture<PostgresFixture>, IAsyncLifetime
{
    private static readonly Guid ActorId = Guid.Parse("7d5f0c2e-3b4a-4f7e-9a51-0c6a2b9d1e11");
    private readonly FakeTimeProvider clock = new(new DateTimeOffset(2026, 11, 12, 15, 0, 0, TimeSpan.Zero));

    public async Task InitializeAsync()
    {
        await using var context = CreateContext();
        await context.Database.EnsureDeletedAsync();
        await context.Database.EnsureCreatedAsync();
    }

    public Task DisposeAsync() => Task.CompletedTask;

    [Fact]
    public async Task Remove_ChangesStateAndKeepsTheRow()
    {
        var owner = new Owner { Email = "ana@example.com" };
        await using (var context = CreateContext())
        {
            context.Owners.Add(owner);
            await context.SaveChangesAsync();
            context.Owners.Remove(owner);
            await context.SaveChangesAsync();
        }

        await using var check = CreateContext();
        Assert.False(await check.Owners.AnyAsync(o => o.Id == owner.Id));

        var stored = await check.Owners.IgnoreQueryFilters().SingleAsync(o => o.Id == owner.Id);
        Assert.Equal(RecordState.Deleted, stored.State);
        Assert.Equal(clock.GetUtcNow(), stored.DeletedAt);
        Assert.Equal(ActorId, stored.DeletedBy);
    }

    [Fact]
    public async Task ActiveUniqueIndex_AllowsReuseAfterDeletionOnly()
    {
        await using var context = CreateContext();
        var first = new Owner { Email = "reuse@example.com" };
        context.Owners.Add(first);
        await context.SaveChangesAsync();

        context.Owners.Add(new Owner { Email = "reuse@example.com" });
        await Assert.ThrowsAsync<DbUpdateException>(() => context.SaveChangesAsync());
        context.ChangeTracker.Clear();

        context.Owners.Remove(await context.Owners.SingleAsync(o => o.Id == first.Id));
        await context.SaveChangesAsync();
        context.Owners.Add(new Owner { Email = "reuse@example.com" });
        await context.SaveChangesAsync();

        Assert.Equal(2, await context.Owners.IgnoreQueryFilters().CountAsync(o => o.Email == "reuse@example.com"));
    }

    [Fact]
    public void Model_RestrictsDeletesAndIndexesState()
    {
        using var context = CreateContext();
        var model = context.Model;

        Assert.All(
            model.GetEntityTypes().SelectMany(entity => entity.GetForeignKeys()),
            foreignKey => Assert.Equal(DeleteBehavior.Restrict, foreignKey.DeleteBehavior));
        Assert.All(
            model.GetEntityTypes(),
            entity => Assert.Contains(entity.GetIndexes(), index => index.Properties.Single().Name == nameof(SoftDeletableEntity.State)));
    }

    [Fact]
    public async Task Restore_ReturnsTheRecordToNormalQueries()
    {
        var owner = new Owner { Email = "restore@example.com" };
        await using (var context = CreateContext())
        {
            context.Owners.Add(owner);
            await context.SaveChangesAsync();
            context.Owners.Remove(owner);
            await context.SaveChangesAsync();

            var deleted = await context.Owners.IgnoreQueryFilters().SingleAsync(o => o.Id == owner.Id);
            deleted.Restore();
            await context.SaveChangesAsync();
        }

        await using var check = CreateContext();
        var restored = await check.Owners.SingleAsync(o => o.Id == owner.Id);
        Assert.Null(restored.DeletedAt);
    }

    private SoftDeleteTestContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<SoftDeleteTestContext>()
            .UseNpgsql(database.ConnectionString)
            .UseSnakeCaseNamingConvention()
            .AddInterceptors(new SoftDeleteInterceptor(new FixedActor(), clock))
            .Options;
        return new SoftDeleteTestContext(options);
    }

    private sealed class FixedActor : ICurrentActor
    {
        public Guid? AccountId => ActorId;
    }
}
