using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;

namespace MiniLibrary.API;
using MiniLibrary.Application;
using MiniLibrary.Infrastructure;
using Serilog;


try
{
    var builder = WebApplication.CreateBuilder(args);

    // Configure Serilog
    Log.Logger = new LoggerConfiguration()
        .ReadFrom.Configuration(builder.Configuration)
        .Enrich.FromLogContext()
        .Enrich.WithProperty("Application", "MiniLibrary")
        .Enrich.WithProperty("Environment", builder.Environment.EnvironmentName)
        .WriteTo.Console()
        .WriteTo.File(
            path: "logs/minilibrary-.txt",
            rollingInterval: RollingInterval.Day,
            retainedFileCountLimit: 30)
        .CreateLogger();

    builder.Host.UseSerilog();

    Log.Information("Starting Mini Library Management System");

    // Add services to the container
    builder.Services.AddApiServices(builder.Configuration);
    builder.Services.AddApplication();
    builder.Services.AddInfrastructure(builder.Configuration);

    var app = builder.Build();

    // Configure the HTTP request pipeline
    if (app.Environment.IsDevelopment())
    {
        app.MapOpenApi();
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Mini Library API V1");
            c.RoutePrefix = string.Empty; // Swagger at root
        });
    }

    // Custom exception handling middleware
    app.UseMiddleware<ExceptionHandlingMiddleware>();

    app.UseHttpsRedirection();

    app.UseCors("AllowAll");

    app.UseAuthentication();
    app.UseAuthorization();

    // Map all endpoints
    app.MapEndpoints();

    Log.Information("Application started successfully");

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}