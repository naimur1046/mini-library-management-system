using Domain.Members;
using Microsoft.EntityFrameworkCore;
using MiniLibrary.Application.Abstractions.Authentication;
using MiniLibrary.Application.Abstractions.Data;
using MiniLibrary.Application.Abstractions.Messaging;
using MiniLibrary.SharedKernel;

namespace MiniLibrary.Application.Members.Delete;

internal sealed class DeleteMemberCommandHandler(
    IApplicationDbContext context,
    IDateTimeProvider dateTimeProvider,
    IUserContext userContext)
    : ICommandHandler<DeleteMemberCommand, Guid>
{
    public async Task<Result<Guid>> Handle(DeleteMemberCommand command, CancellationToken cancellationToken)
    {
        var member = await context.Members
            .FirstOrDefaultAsync(
                m => m.Id == command.Id && !m.IsDeleted,
                cancellationToken);

        if (member is null)
        {
            return Result.Failure<Guid>(MemberErrors.NotFound(command.Id));
        }
        
        member.IsDeleted = true;
        member.DeletedOnUtc = dateTimeProvider.UtcNow;
        member.DeletedBy = userContext.Email;

        await context.SaveChangesAsync(cancellationToken);

        return member.Id;
    }
}
