using System.Reflection;
using System.Text.Json.Serialization;
using Atlas.Kit.Api.Endpoints;
using Atlas.Kit.Api.Errors;
using Atlas.Kit.Api.Security;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Scalar.AspNetCore;

namespace Atlas.Kit.Api;

public static class ServiceDefaults
{
    /// <summary>Wspólna konfiguracja serwisu: JWT, ProblemDetails, OpenAPI, health check i endpointy z assembly Api.</summary>
    public static WebApplicationBuilder AddKitApi(this WebApplicationBuilder builder, Assembly apiAssembly)
    {
        builder.Services.AddKitJwtAuthentication(builder.Configuration);
        builder.Services.ConfigureHttpJsonOptions(o => o.SerializerOptions.Converters.Add(new JsonStringEnumConverter()));
        builder.Services.AddProblemDetails();
        builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
        builder.Services.AddOpenApi();
        builder.Services.AddHealthChecks();
        builder.Services.AddEndpoints(apiAssembly);
        return builder;
    }

    /// <summary>Middleware i mapowanie endpointów. <paramref name="serviceName"/> trafia do odpowiedzi /health.</summary>
    public static WebApplication UseKitApi(this WebApplication app, string serviceName)
    {
        app.UseExceptionHandler();
        app.UseAuthentication();
        app.UseAuthorization();

        app.MapOpenApi();
        if (app.Environment.IsDevelopment())
        {
            app.MapScalarApiReference();
        }

        app.MapHealthChecks("/health/ready");
        app.MapGet("/health", () => Results.Ok(new { service = serviceName, status = "ok", time = DateTimeOffset.UtcNow }))
            .WithTags("Health");

        app.MapEndpoints();
        return app;
    }
}
