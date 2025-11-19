using Domain.Users;
using Microsoft.EntityFrameworkCore;
using MiniLibrary.Application.Abstractions.Authentication;
using MiniLibrary.Application.Abstractions.Data;
using MiniLibrary.Application.Abstractions.Messaging;
using SharedKernel;

namespace MiniLibrary.Application.Users.Register;

internal sealed class RegisterCommandHandler(
    IApplicationDbContext context,
    IPasswordHasher passwordHasher,
    IDateTimeProvider dateTimeProvider)
    : ICommandHandler<RegisterCommand, Guid>
{
    public async Task<Result<Guid>> Handle(RegisterCommand command, CancellationToken cancellationToken)
    {
        bool emailExists = await context.Users
            .AnyAsync(u => u.Email == command.Email.ToLowerInvariant() && !u.IsDeleted, cancellationToken);

        if (emailExists)
        {
            return Result.Failure<Guid>(UserErrors.EmailAlreadyExists(command.Email));
        }

        string passwordHash = passwordHasher.Hash(command.Password);

        var user = new User
        {
            FullName = command.FullName,
            Email = command.Email.ToLowerInvariant(),
            PasswordHash = passwordHash,
            Role = command.Role,
            IsActive = true,
            CreatedOnUtc = dateTimeProvider.UtcNow,
            CreatedBy = "System",
            IsDeleted = false
        };

        context.Users.Add(user);

        await context.SaveChangesAsync(cancellationToken);

        return user.Id;
    }
}
