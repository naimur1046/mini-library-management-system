using System.Reflection;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using MiniLibrary.API.Extensions;
using Serilog;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Configure Serilog
builder.Host.UseSerilog((context, loggerConfig) =>
    loggerConfig.ReadFrom.Configuration(context.Configuration));

// Add services to the container
builder.Services.AddApiServices(builder.Configuration);
    // TODO: Add Application layer services here when needed
    // TODO: Add Infrastructure layer services here when needed

WebApplication app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerWithUi();

    // Apply migrations in development
    // app.ApplyMigrations();
}

// Health checks endpoint
app.MapHealthChecks("health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

// Request context logging for correlation
app.UseRequestContextLogging();

// Serilog request logging
app.UseSerilogRequestLogging();

// Global exception handler
app.UseExceptionHandler();

app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthentication();

app.UseAuthorization();

// Map all endpoints
app.MapEndpoints();

await app.RunAsync();

// REMARK: Required for functional and integration tests to work.
namespace MiniLibrary.API
{
    public partial class Program;
}