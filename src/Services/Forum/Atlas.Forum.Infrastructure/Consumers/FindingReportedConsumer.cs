using Atlas.Contracts.Map;
using Atlas.Forum.Application.Findings.ShareFinding;
using MassTransit;
using MediatR;

namespace Atlas.Forum.Infrastructure.Consumers;

/// <summary>Znalezisko z mapy (RabbitMQ) → wątek w kategorii Znaleziska.</summary>
public sealed class FindingReportedConsumer(ISender sender) : IConsumer<FindingReported>
{
    public Task Consume(ConsumeContext<FindingReported> context)
    {
        var message = context.Message;
        return sender.Send(
            new ShareFindingCommand(
                message.FindingId,
                message.SpeciesName,
                message.Latitude,
                message.Longitude,
                message.Note,
                message.AuthorId,
                message.AuthorName),
            context.CancellationToken);
    }
}
