using Atlas.Map.Application.Common;

namespace Atlas.Map.Application.Abstractions;

public interface IMapReader
{
    Task<IReadOnlyList<FindingView>> GetFindingsAsync(FindingFilter filter, CancellationToken cancellationToken);

    Task<IReadOnlyList<SpeciesView>> GetSpeciesAsync(CancellationToken cancellationToken);
}
