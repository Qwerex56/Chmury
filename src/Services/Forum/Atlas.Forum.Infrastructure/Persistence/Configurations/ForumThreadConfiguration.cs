using Atlas.Forum.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Atlas.Forum.Infrastructure.Persistence.Configurations;

internal sealed class ForumThreadConfiguration : IEntityTypeConfiguration<ForumThread>
{
    public void Configure(EntityTypeBuilder<ForumThread> builder)
    {
        builder.ToTable("threads");
        builder.HasKey(t => t.Id);
        builder.Property(t => t.Title).HasMaxLength(150).IsRequired();
        builder.Property(t => t.Category).HasConversion<string>().HasMaxLength(20);
        builder.Property(t => t.AuthorName).HasMaxLength(40);
        builder.HasIndex(t => t.LastActivityAt);
        builder.HasIndex(t => t.FindingId).IsUnique();

        builder.HasMany(t => t.Posts)
            .WithOne()
            .HasForeignKey(p => p.ThreadId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.Navigation(t => t.Posts).UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
