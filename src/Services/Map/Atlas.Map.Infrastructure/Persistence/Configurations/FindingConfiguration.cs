using Atlas.Map.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Atlas.Map.Infrastructure.Persistence.Configurations;

internal sealed class FindingConfiguration : IEntityTypeConfiguration<Finding>
{
    public void Configure(EntityTypeBuilder<Finding> builder)
    {
        builder.ToTable("findings");
        builder.HasKey(f => f.Id);
        builder.Property(f => f.SpeciesId).HasMaxLength(60).IsRequired();
        builder.Property(f => f.Location).HasColumnType("geometry(Point,4326)").IsRequired();
        builder.Property(f => f.Note).HasMaxLength(1000);
        builder.Property(f => f.AuthorName).HasMaxLength(40);
        builder.Ignore(f => f.Latitude);
        builder.Ignore(f => f.Longitude);

        builder.HasIndex(f => f.Location).HasMethod("gist");
        builder.HasIndex(f => f.FoundAt);
        builder.HasOne<MushroomSpecies>().WithMany().HasForeignKey(f => f.SpeciesId);
    }
}
