using Atlas.Map.Application.Abstractions;
using Atlas.Map.Application.Common;
using MediatR;

namespace Atlas.Map.Application.Findings.GetFindings;

internal sealed class GetFindingsHandler(IMapReader reader) : IRequestHandler<GetFindingsQuery, IReadOnlyList<FindingView>>
{
    public Task<IReadOnlyList<FindingView>> Handle(GetFindingsQuery query, CancellationToken cancellationToken) =>
        reader.GetFindingsAsync(query.Filter, cancellationToken);
}
