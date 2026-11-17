using Atlas.Map.Application.Common;
using MediatR;

namespace Atlas.Map.Application.Species.GetSpecies;

/// <summary>Słownik gatunków (do legendy mapy i formularza znaleziska).</summary>
public sealed record GetSpeciesQuery : IRequest<IReadOnlyList<SpeciesView>>;
