using Atlas.Map.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Atlas.Map.Infrastructure.Persistence.Configurations;

internal sealed class MushroomSpeciesConfiguration : IEntityTypeConfiguration<MushroomSpecies>
{
    public void Configure(EntityTypeBuilder<MushroomSpecies> builder)
    {
        builder.ToTable("species");
        builder.HasKey(s => s.Id);
        builder.Property(s => s.Id).HasMaxLength(60);
        builder.Property(s => s.PolishName).HasMaxLength(80).IsRequired();
        builder.Property(s => s.LatinName).HasMaxLength(80).IsRequired();
        builder.Property(s => s.Edibility).HasConversion<string>().HasMaxLength(20);
        builder.Property(s => s.Color).HasMaxLength(9);
    }
}
