using Atlas.Contracts.Map;
using Atlas.Kit.Application.Messaging;
using Atlas.Map.Application.Abstractions;
using Atlas.Map.Domain;
using FluentResults;
using MediatR;

namespace Atlas.Map.Application.Findings.ReportFinding;

internal sealed class ReportFindingHandler(IFindingRepository findings, IIntegrationEventPublisher events, TimeProvider clock)
    : IRequestHandler<ReportFindingCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(ReportFindingCommand command, CancellationToken cancellationToken)
    {
        var species = await findings.FindSpeciesAsync(command.SpeciesId, cancellationToken);
        if (species is null)
        {
            return MapErrors.UnknownSpecies;
        }

        var now = clock.GetUtcNow();
        var finding = Finding.Report(
            species.Id,
            command.Latitude,
            command.Longitude,
            command.FoundAt ?? now,
            command.Note,
            command.AuthorId,
            command.AuthorName,
            now);

        if (finding.IsFailed)
        {
            return finding.ToResult();
        }

        findings.Add(finding.Value);
        await findings.SaveChangesAsync(cancellationToken);

        await events.PublishAsync(
            new FindingReported(
                finding.Value.Id,
                species.PolishName,
                finding.Value.Latitude,
                finding.Value.Longitude,
                finding.Value.Note,
                command.AuthorId,
                command.AuthorName,
                finding.Value.FoundAt),
            cancellationToken);

        return finding.Value.Id;
    }
}
