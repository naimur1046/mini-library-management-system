using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using MiniLibrary.Infrastructure.Authentication;
using SharedKernel;

namespace MiniLibrary.Infrastructure.Interceptors;

public sealed class AuditableEntityInterceptor : SaveChangesInterceptor
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IDateTimeProvider _dateTimeProvider;

    public AuditableEntityInterceptor(
        IHttpContextAccessor httpContextAccessor,
        IDateTimeProvider dateTimeProvider)
    {
        _httpContextAccessor = httpContextAccessor;
        _dateTimeProvider = dateTimeProvider;
    }

    public override InterceptionResult<int> SavingChanges(
        DbContextEventData eventData,
        InterceptionResult<int> result)
    {
        UpdateAuditableEntities(eventData.Context);
        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        UpdateAuditableEntities(eventData.Context);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private void UpdateAuditableEntities(DbContext? context)
    {
        if (context is null)
        {
            return;
        }

        // Get current user ID from HTTP context, or use "system" as fallback
        var currentUserId = _httpContextAccessor.HttpContext?.User.GetUserId().ToString() ?? "system";
        var currentTime = _dateTimeProvider.UtcNow;

        var entries = context.ChangeTracker.Entries<IAuditableEntity>();

        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedOnUtc = currentTime;
                entry.Entity.CreatedBy = currentUserId;
            }

            if (entry.State == EntityState.Modified || entry.State == EntityState.Added)
            {
                entry.Entity.ModifiedOnUtc = currentTime;
                entry.Entity.ModifiedBy = currentUserId;
            }
        }

        var softDeleteEntries = context.ChangeTracker.Entries<ISoftDeletableEntity>();

        foreach (var entry in softDeleteEntries)
        {
            if (entry.State == EntityState.Deleted)
            {
                entry.State = EntityState.Modified;
                entry.Entity.IsDeleted = true;
                entry.Entity.DeletedOnUtc = currentTime;
                entry.Entity.DeletedBy = currentUserId;
            }
        }
    }
}
