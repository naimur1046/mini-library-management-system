using Serilog.Context;

namespace MiniLibrary.API.Extensions;

public static class MiddlewareExtensions
{
    public static IApplicationBuilder UseSwaggerWithUi(this WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "Mini Library API V1");
            options.RoutePrefix = string.Empty; // Swagger at root
            options.DocumentTitle = "Mini Library API Documentation";
        });

        return app;
    }

    public static IApplicationBuilder UseRequestContextLogging(this IApplicationBuilder app)
    {
        app.Use(async (context, next) =>
        {
            using (LogContext.PushProperty("CorrelationId", Guid.NewGuid()))
            using (LogContext.PushProperty("RequestPath", context.Request.Path))
            using (LogContext.PushProperty("RequestMethod", context.Request.Method))
            {
                await next();
            }
        });

        return app;
    }
}
