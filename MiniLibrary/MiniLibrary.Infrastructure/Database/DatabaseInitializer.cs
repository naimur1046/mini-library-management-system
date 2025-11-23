using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MiniLibrary.Application.Abstractions.Authentication;
using MiniLibrary.SharedKernel;

namespace MiniLibrary.Infrastructure.Database;

public static class DatabaseInitializer
{
    public static async Task InitializeDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var services = scope.ServiceProvider;
        var logger = services.GetRequiredService<ILogger<DatabaseSeeder>>();

        try
        {
            var context = services.GetRequiredService<ApplicationDbContext>();
            var passwordHasher = services.GetRequiredService<IPasswordHasher>();
            var dateTimeProvider = services.GetRequiredService<IDateTimeProvider>();
            
            await context.Database.MigrateAsync();
            logger.LogInformation("Database migrations applied successfully");
            
            var seeder = new DatabaseSeeder(context, passwordHasher, dateTimeProvider, logger);
            await seeder.SeedAsync();
            logger.LogInformation("Database seeding completed successfully");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while initializing the database");
            throw;
        }
    }
}
