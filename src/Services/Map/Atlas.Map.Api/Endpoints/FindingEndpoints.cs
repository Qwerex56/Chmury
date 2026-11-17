using System.Security.Claims;
using Atlas.Kit.Api.Endpoints;
using Atlas.Kit.Api.Errors;
using Atlas.Kit.Api.Security;
using Atlas.Map.Application.Abstractions;
using Atlas.Map.Application.Findings.GetFindings;
using Atlas.Map.Application.Findings.ReportFinding;
using MediatR;

namespace Atlas.Map.Api.Endpoints;

internal sealed class FindingEndpoints : IEndpoint
{
    public void Map(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/findings").WithTags("Findings");

        group.MapGet("/", async (
                double? south, double? west, double? north, double? east,
                string? species, int? days,
                ISender sender, CancellationToken ct) =>
            {
                BoundingBox? area = south is not null && west is not null && north is not null && east is not null
                    ? new BoundingBox(south.Value, west.Value, north.Value, east.Value)
                    : null;

                return Results.Ok(await sender.Send(new GetFindingsQuery(new FindingFilter(area, species, days)), ct));
            });

        group.MapPost("/", async (ReportFindingRequest request, ClaimsPrincipal user, ISender sender, CancellationToken ct) =>
            {
                var result = await sender.Send(
                    new ReportFindingCommand(
                        request.SpeciesId,
                        request.Latitude,
                        request.Longitude,
                        request.FoundAt,
                        request.Note,
                        user.GetUserId(),
                        user.GetUserName()),
                    ct);
                return result.ToHttpResult(id => Results.Created($"/findings/{id}", new IdResponse(id)));
            })
            .RequireAuthorization();
    }
}
