using MiniLibrary.Domain.Members;
using Microsoft.EntityFrameworkCore;
using MiniLibrary.Application.Abstractions.Data;
using MiniLibrary.Application.Abstractions.Messaging;
using MiniLibrary.SharedKernel;

namespace MiniLibrary.Application.Members.Delete;

internal sealed class DeleteMemberCommandHandler(
    IApplicationDbContext context,
    IDateTimeProvider dateTimeProvider)
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

        await context.SaveChangesAsync(cancellationToken);

        return member.Id;
    }
}
