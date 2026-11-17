using Atlas.Map.Domain;

namespace Atlas.Map.Domain.Tests;

public sealed class FindingTests
{
    private static readonly DateTimeOffset Now = new(2026, 9, 15, 12, 0, 0, TimeSpan.Zero);

    [Fact]
    public void Report_stores_location_as_lon_lat_point_in_wgs84()
    {
        var result = Finding.Report("kania", 52.33, 20.55, Now, " pod sosną ", Guid.NewGuid(), "Kania", Now);

        Assert.True(result.IsSuccess);
        Assert.Equal(20.55, result.Value.Location.X);
        Assert.Equal(52.33, result.Value.Location.Y);
        Assert.Equal(Finding.Srid, result.Value.Location.SRID);
        Assert.Equal("pod sosną", result.Value.Note);
    }

    [Theory]
    [InlineData(91, 20)]
    [InlineData(52, 181)]
    public void Report_rejects_invalid_coordinates(double lat, double lon)
    {
        var result = Finding.Report("kania", lat, lon, Now, null, Guid.NewGuid(), "Kania", Now);

        Assert.True(result.IsFailed);
    }

    [Fact]
    public void Report_rejects_date_from_future()
    {
        var result = Finding.Report("kania", 52, 20, Now.AddDays(1), null, Guid.NewGuid(), "Kania", Now);

        Assert.True(result.IsFailed);
    }

    [Fact]
    public void Species_season_is_inclusive()
    {
        var species = MushroomSpecies.Create("kurka", "Kurka", "Cantharellus cibarius", Edibility.Jadalny, 6, 10, "#f4a300");

        Assert.True(species.IsInSeason(6));
        Assert.True(species.IsInSeason(10));
        Assert.False(species.IsInSeason(11));
    }
}
