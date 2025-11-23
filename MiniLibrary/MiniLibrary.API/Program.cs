using System.Reflection;
using MiniLibrary.API.Extensions;
using MiniLibrary.Application;
using MiniLibrary.Infrastructure;
using MiniLibrary.Infrastructure.Database;
using Serilog;

try
{
    WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
    
    builder.Host.UseSerilog((context, loggerConfig) =>
        loggerConfig.ReadFrom.Configuration(context.Configuration));
    
    builder.Services.AddApiServices(builder.Configuration);
    
    builder.Services.AddApplication();
    
    builder.Services.AddInfrastructure(builder.Configuration);
    
    WebApplication app = builder.Build();

    // Initialize database (apply migrations and seed initial data)
    await app.InitializeDatabaseAsync();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwaggerWithUi();
    }

    app.UseRequestContextLogging();

    app.UseSerilogRequestLogging();

    app.UseExceptionHandler();

    app.UseHttpsRedirection();

    app.UseCors("AllowAll");

    app.UseAuthentication();

    app.UseAuthorization();
    
    app.MapEndpoints();
    
    foreach (var address in app.Urls)
    {
        Console.WriteLine($"  - {address}");
    }

    await app.RunAsync();
}
catch (Exception e)
{
    Log.Fatal(e, "An error occurred while starting the Application");
    throw;
}
finally
{
    await Log.CloseAndFlushAsync();
}