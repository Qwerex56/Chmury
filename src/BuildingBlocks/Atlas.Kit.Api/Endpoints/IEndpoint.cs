using Microsoft.AspNetCore.Routing;

namespace Atlas.Kit.Api.Endpoints;

/// <summary>Jeden endpoint (lub mała grupa) Minimal API; wykrywany automatycznie przez <see cref="EndpointRegistration"/>.</summary>
public interface IEndpoint
{
    void Map(IEndpointRouteBuilder app);
}
