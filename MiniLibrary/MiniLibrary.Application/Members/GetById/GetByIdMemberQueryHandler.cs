using Domain.Members;
using Microsoft.EntityFrameworkCore;
using MiniLibrary.Application.Abstractions.Data;
using MiniLibrary.Application.Abstractions.Messaging;
using SharedKernel;
using MiniLibrary.Application.Members.Get;

namespace MiniLibrary.Application.Members.GetById;

internal sealed class GetByIdMemberQueryHandler(IApplicationDbContext context)
    : IQueryHandler<GetByIdMemberQuery, MemberResponse>
{
    public async Task<Result<MemberResponse>> Handle(
        GetByIdMemberQuery query,
        CancellationToken cancellationToken)
    {
        MemberResponse? book = await context.Members
            .Where(m => m.Id == query.MemberId && !m.IsDeleted)
            .Select(m => new MemberResponse
            {
                Id = m.Id,
                FullName = m.FullName,
                Email = m.Email,
                Phone = m.Phone,
                JoinDate = m.JoinDate,
                IsActive = m.IsActive,
                CreatedOnUtc = m.CreatedOnUtc
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (book is null)
        {
            return Result.Failure<MemberResponse>(MemberErrors.NotFound(query.MemberId));
        }

        return book;
    }
}