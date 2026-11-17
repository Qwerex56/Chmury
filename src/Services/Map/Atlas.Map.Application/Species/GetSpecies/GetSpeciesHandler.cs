using Atlas.Map.Application.Abstractions;
using Atlas.Map.Application.Common;
using MediatR;

namespace Atlas.Map.Application.Species.GetSpecies;

internal sealed class GetSpeciesHandler(IMapReader reader) : IRequestHandler<GetSpeciesQuery, IReadOnlyList<SpeciesView>>
{
    public Task<IReadOnlyList<SpeciesView>> Handle(GetSpeciesQuery query, CancellationToken cancellationToken) =>
        reader.GetSpeciesAsync(cancellationToken);
}
