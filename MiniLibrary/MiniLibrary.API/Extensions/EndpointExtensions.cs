using System.Reflection;
using MiniLibrary.API.Endpoints;

namespace MiniLibrary.API.Extensions;

public static class EndpointExtensions
{
    public static IServiceCollection AddEndpoints(
        this IServiceCollection services,
        Assembly assembly)
    {
        // Get all types that implement IEndpoint
        var endpointTypes = assembly.GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract && typeof(IEndpoint).IsAssignableFrom(t));

        foreach (var type in endpointTypes)
        {
            services.AddScoped(typeof(IEndpoint), type);
        }

        return services;
    }

    public static IApplicationBuilder MapEndpoints(
        this WebApplication app,
        RouteGroupBuilder? routeGroupBuilder = null)
    {
        // Get all registered endpoints
        using var scope = app.Services.CreateScope();
        var endpoints = scope.ServiceProvider.GetRequiredService<IEnumerable<IEndpoint>>();

        // Determine the base builder
        IEndpointRouteBuilder builder = routeGroupBuilder is not null ? routeGroupBuilder : app;

        // Map each endpoint
        foreach (var endpoint in endpoints)
        {
            endpoint.MapEndpoint(builder);
        }

        return app;
    }
    
    public static RouteGroupBuilder MapApiGroup(
        this WebApplication app,
        string version = "v1")
    {
        return app.MapGroup($"/api/{version}")
            .WithTags(version)
            .WithOpenApi();
    }
}