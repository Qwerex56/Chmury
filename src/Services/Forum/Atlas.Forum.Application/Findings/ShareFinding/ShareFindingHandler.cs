using System.Globalization;
using Atlas.Forum.Application.Abstractions;
using Atlas.Forum.Domain;
using MediatR;

namespace Atlas.Forum.Application.Findings.ShareFinding;

internal sealed class ShareFindingHandler(IForumThreadRepository threads, TimeProvider clock) : IRequestHandler<ShareFindingCommand>
{
    public async Task Handle(ShareFindingCommand command, CancellationToken cancellationToken)
    {
        if (await threads.ExistsForFindingAsync(command.FindingId, cancellationToken))
        {
            return;
        }

        var coordinates = string.Create(CultureInfo.InvariantCulture, $"{command.Latitude:F4}, {command.Longitude:F4}");
        var content = string.IsNullOrWhiteSpace(command.Note)
            ? $"Nowe znalezisko na mapie ({coordinates})."
            : $"{command.Note}\n\nMiejsce na mapie: {coordinates}.";

        threads.Add(ForumThread.Start(
            $"Znalezisko: {command.SpeciesName}",
            ForumCategory.Znaleziska,
            content,
            command.AuthorId,
            command.AuthorName,
            clock.GetUtcNow(),
            command.FindingId));

        await threads.SaveChangesAsync(cancellationToken);
    }
}
