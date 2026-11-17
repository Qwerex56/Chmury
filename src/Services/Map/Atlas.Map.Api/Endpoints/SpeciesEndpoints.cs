using Atlas.Kit.Api.Endpoints;
using Atlas.Map.Application.Species.GetSpecies;
using MediatR;

namespace Atlas.Map.Api.Endpoints;

internal sealed class SpeciesEndpoints : IEndpoint
{
    public void Map(IEndpointRouteBuilder app) =>
        app.MapGet("/species", async (ISender sender, CancellationToken ct) =>
                Results.Ok(await sender.Send(new GetSpeciesQuery(), ct)))
            .WithTags("Species");
}
