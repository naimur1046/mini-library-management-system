using MiniLibrary.Domain.Members;
using Microsoft.EntityFrameworkCore;
using MiniLibrary.Application.Abstractions.Authentication;
using MiniLibrary.Application.Abstractions.Data;
using MiniLibrary.Application.Abstractions.Messaging;
using MiniLibrary.SharedKernel;

namespace MiniLibrary.Application.Members.Update;

internal sealed class UpdateMemberCommandHandler(
    IApplicationDbContext context,
    IDateTimeProvider dateTimeProvider,
    IUserContext userContext)
    : ICommandHandler<UpdateMemberCommand, Guid>
{
    public async Task<Result<Guid>> Handle(UpdateMemberCommand command, CancellationToken cancellationToken)
    {
        Member? member = await context.Members
            .FirstOrDefaultAsync(m => m.Id == command.MemberId && !m.IsDeleted, cancellationToken);

        if (member is null)
        {
            return Result.Failure<Guid>(MemberErrors.NotFound(command.MemberId));
        }
        
        if (!string.IsNullOrWhiteSpace(command.Email) && command.Email != member.Email)
        {
            bool emailExists = await context.Members
                .AnyAsync(m => m.Email == command.Email && m.Id != command.MemberId && !m.IsDeleted, cancellationToken);

            if (emailExists)
            {
                return Result.Failure<Guid>(MemberErrors.EmailAlreadyExists(command.Email));
            }

            member.Email = command.Email;
        }

        if (!string.IsNullOrWhiteSpace(command.FullName))
        {
            member.FullName = command.FullName;
        }

        if (!string.IsNullOrWhiteSpace(command.Phone))
        {
            member.Phone = command.Phone;
        }

        if (command.JoinDate.HasValue)
        {
            if (command.JoinDate.Value > dateTimeProvider.UtcNow)
            {
                return Result.Failure<Guid>(MemberErrors.InvalidJoinDate());
            }

            member.JoinDate = command.JoinDate.Value;
        }

        if (command.IsActive.HasValue)
        {
            member.IsActive = command.IsActive.Value;
        }
        
        member.ModifiedOnUtc = dateTimeProvider.UtcNow;
        member.ModifiedBy = userContext.Email;

        await context.SaveChangesAsync(cancellationToken);

        return member.Id;
    }
}
