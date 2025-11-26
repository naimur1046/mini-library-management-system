using MiniLibrary.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MiniLibrary.Application.Abstractions.Authentication;
using MiniLibrary.SharedKernel;

namespace MiniLibrary.Infrastructure.Database;

internal sealed class DatabaseSeeder
{
    private readonly ApplicationDbContext _context;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ILogger<DatabaseSeeder> _logger;

    public DatabaseSeeder(
        ApplicationDbContext context,
        IPasswordHasher passwordHasher,
        IDateTimeProvider dateTimeProvider,
        ILogger<DatabaseSeeder> logger)
    {
        _context = context;
        _passwordHasher = passwordHasher;
        _dateTimeProvider = dateTimeProvider;
        _logger = logger;
    }

    public async Task SeedAsync()
    {
        try
        {
            if (!await _context.Users.AnyAsync())
            {
                await SeedAdminUserAsync();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while seeding the database");
            throw;
        }
    }

    private async Task SeedAdminUserAsync()
    {
        const string adminEmail = "admin@minilibrary.com";
        const string adminPassword = "Admin@123";

        var adminUser = new User
        {
            FullName = "System Administrator",
            Email = adminEmail.ToLowerInvariant(),
            PasswordHash = _passwordHasher.Hash(adminPassword),
            Role = Role.Admin,
            IsActive = true,
            IsDeleted = false,
            CreatedOnUtc = _dateTimeProvider.UtcNow,
            CreatedBy = "System"
        };

        _context.Users.Add(adminUser);
        await _context.SaveChangesAsync();

        _logger.LogInformation(
            "Initial admin user created successfully. Email: {Email}, Password: {Password}",
            adminEmail,
            adminPassword);
    }
}
