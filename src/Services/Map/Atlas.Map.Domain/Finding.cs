using Atlas.Kit.Domain;
using FluentResults;
using NetTopologySuite.Geometries;

namespace Atlas.Map.Domain;

/// <summary>Znalezisko grzyba na mapie. Lokalizacja to punkt WGS84 (SRID 4326), X = długość, Y = szerokość.</summary>
public sealed class Finding
{
    public const int Srid = 4326;

    private Finding()
    {
    }

    public Guid Id { get; private set; }

    public string SpeciesId { get; private set; } = string.Empty;

    public Point Location { get; private set; } = null!;

    public DateTimeOffset FoundAt { get; private set; }

    public string? Note { get; private set; }

    public Guid AuthorId { get; private set; }

    public string AuthorName { get; private set; } = string.Empty;

    public double Latitude => Location.Y;

    public double Longitude => Location.X;

    public static Result<Finding> Report(
        string speciesId,
        double latitude,
        double longitude,
        DateTimeOffset foundAt,
        string? note,
        Guid authorId,
        string authorName,
        DateTimeOffset now)
    {
        if (latitude is < -90 or > 90 || longitude is < -180 or > 180)
        {
            return MapErrors.InvalidLocation;
        }

        if (foundAt > now.AddMinutes(5))
        {
            return MapErrors.FoundInFuture;
        }

        return new Finding
        {
            Id = Guid.CreateVersion7(),
            SpeciesId = speciesId,
            Location = new Point(longitude, latitude) { SRID = Srid },
            FoundAt = foundAt,
            Note = string.IsNullOrWhiteSpace(note) ? null : note.Trim(),
            AuthorId = authorId,
            AuthorName = authorName,
        };
    }
}
