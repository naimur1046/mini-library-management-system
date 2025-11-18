using System.Reflection;
using MiniLibrary.API.Extensions;
using MiniLibrary.Application;
using MiniLibrary.Infrastructure;
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

    await app.RunAsync();
}
catch (Exception e)
{
    Log.Error(e, "An error occurred while starting the Application", e.Message);
}
finally
{
    await Log.CloseAndFlushAsync();
}