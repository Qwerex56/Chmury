using Atlas.Map.Application.Abstractions;
using Atlas.Map.Application.Common;
using MediatR;

namespace Atlas.Map.Application.Findings.GetFindings;

/// <summary>Znaleziska w widocznym obszarze mapy (max 500, najnowsze najpierw).</summary>
public sealed record GetFindingsQuery(FindingFilter Filter) : IRequest<IReadOnlyList<FindingView>>;
