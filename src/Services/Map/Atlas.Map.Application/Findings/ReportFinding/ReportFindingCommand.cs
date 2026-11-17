using FluentResults;
using MediatR;

namespace Atlas.Map.Application.Findings.ReportFinding;

/// <summary>
/// Dodaje znalezisko na mapie. Gatunek musi istnieć w słowniku. Po zapisie publikuje
/// <c>FindingReported</c>, z którego Forum zakłada wątek.
/// </summary>
public sealed record ReportFindingCommand(
    string SpeciesId,
    double Latitude,
    double Longitude,
    DateTimeOffset? FoundAt,
    string? Note,
    Guid AuthorId,
    string AuthorName) : IRequest<Result<Guid>>;
