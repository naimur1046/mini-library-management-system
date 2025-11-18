using System.Reflection;
using MiniLibrary.API.Extensions;
using MiniLibrary.Application;
using MiniLibrary.Infrastructure;
using Serilog;

try
{
    Console.WriteLine("Starting application...");

    WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

    Console.WriteLine("Configuring Serilog...");
    builder.Host.UseSerilog((context, loggerConfig) =>
        loggerConfig.ReadFrom.Configuration(context.Configuration));

    Console.WriteLine("Adding API services...");
    builder.Services.AddApiServices(builder.Configuration);

    Console.WriteLine("Adding Application layer...");
    builder.Services.AddApplication();

    Console.WriteLine("Adding Infrastructure layer...");
    builder.Services.AddInfrastructure(builder.Configuration);

    Console.WriteLine("Building application...");
    WebApplication app = builder.Build();

    Console.WriteLine("Configuring middleware...");
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

    Console.WriteLine("Mapping endpoints...");
    app.MapEndpoints();

    Console.WriteLine("Starting application on URLs:");
    foreach (var address in app.Urls)
    {
        Console.WriteLine($"  - {address}");
    }

    await app.RunAsync();
}
catch (Exception e)
{
    Console.WriteLine($"FATAL ERROR: {e.Message}");
    Console.WriteLine($"Stack Trace: {e.StackTrace}");
    if (e.InnerException != null)
    {
        Console.WriteLine($"Inner Exception: {e.InnerException.Message}");
        Console.WriteLine($"Inner Stack Trace: {e.InnerException.StackTrace}");
    }
    Log.Fatal(e, "An error occurred while starting the Application");
    throw;
}
finally
{
    await Log.CloseAndFlushAsync();
}