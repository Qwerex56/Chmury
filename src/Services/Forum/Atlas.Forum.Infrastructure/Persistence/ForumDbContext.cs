using Atlas.Forum.Domain;
using Microsoft.EntityFrameworkCore;

namespace Atlas.Forum.Infrastructure.Persistence;

public sealed class ForumDbContext(DbContextOptions<ForumDbContext> options) : DbContext(options)
{
    public DbSet<ForumThread> Threads => Set<ForumThread>();

    public DbSet<Post> Posts => Set<Post>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("forum");
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ForumDbContext).Assembly);
    }
}
