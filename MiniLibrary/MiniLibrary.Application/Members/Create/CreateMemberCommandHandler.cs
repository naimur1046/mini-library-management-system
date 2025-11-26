using MiniLibrary.Application.Abstractions.Authentication;
using MiniLibrary.Application.Abstractions.Data;
using MiniLibrary.Application.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;
using MiniLibrary.Domain.Members;
using MiniLibrary.SharedKernel;

namespace MiniLibrary.Application.Members.Create;

internal sealed class CreateMemberCommandHandler(
    IApplicationDbContext context,
    IDateTimeProvider dateTimeProvider,
    IUserContext userContext)
    : ICommandHandler<CreateMemberCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateMemberCommand command, CancellationToken cancellationToken)
    {
        bool emailExists = await context.Members
            .AnyAsync(m => m.Email == command.Email && !m.IsDeleted, cancellationToken);

        if (emailExists)
        {
            return Result.Failure<Guid>(MemberErrors.EmailAlreadyExists(command.Email));
        }
        
        var member = new Member
        {
            FullName = command.FullName,
            Email = command.Email,
            Phone = command.Phone,
            JoinDate = command.JoinDate,
            IsActive = command.IsActive,
            CreatedOnUtc = dateTimeProvider.UtcNow,
            CreatedBy = userContext.Email,
            IsDeleted = false
        };

        context.Members.Add(member);

        await context.SaveChangesAsync(cancellationToken);

        return member.Id;
    }
}