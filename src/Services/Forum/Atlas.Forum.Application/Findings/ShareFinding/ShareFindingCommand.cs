using MediatR;

namespace Atlas.Forum.Application.Findings.ShareFinding;

/// <summary>
/// Zakłada wątek w kategorii Znaleziska dla znaleziska dodanego na mapie.
/// Idempotentne: drugi raz dla tego samego znaleziska nic nie robi (RabbitMQ może dostarczyć wiadomość ponownie).
/// </summary>
public sealed record ShareFindingCommand(
    Guid FindingId,
    string SpeciesName,
    double Latitude,
    double Longitude,
    string? Note,
    Guid AuthorId,
    string AuthorName) : IRequest;
