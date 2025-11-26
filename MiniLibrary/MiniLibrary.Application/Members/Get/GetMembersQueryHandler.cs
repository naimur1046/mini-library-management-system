using MiniLibrary.Domain.Members;
using Microsoft.EntityFrameworkCore;
using MiniLibrary.Application.Abstractions.Data;
using MiniLibrary.Application.Abstractions.Messaging;
using MiniLibrary.SharedKernel;

namespace MiniLibrary.Application.Members.Get;

internal sealed class GetMembersQueryHandler(IApplicationDbContext context)
    : IQueryHandler<GetMembersQuery, PagedMembersResponse>
{
    private const int DefaultPageSize = 100;
    private const string Forward = "Forward";
    private const string Backward = "Backward";

    public async Task<Result<PagedMembersResponse>> Handle(
        GetMembersQuery query,
        CancellationToken cancellationToken)
    {
        int pageSize = query.Size ?? DefaultPageSize;
        string direction = string.IsNullOrWhiteSpace(query.Direction)
            ? Forward
            : query.Direction.ToLowerInvariant();
        
        if (pageSize <= 0 || pageSize > 1000)
        {
            return Result.Failure<PagedMembersResponse>(
                Error.Problem("Books.InvalidPageSize", "Page size must be between 1 and 1000"));
        }
        
        if (direction != Forward && direction != Backward)
        {
            return Result.Failure<PagedMembersResponse>(
                Error.Problem("Books.InvalidDirection", "Direction must be 'up' or 'down'"));
        }

        Guid? cursorMemberId = query.LastMemberId;
        DateTime? cursorCreatedOnUtc = null;
        
        if (!cursorMemberId.HasValue)
        {
            var firstMember = await context.Books
                .Where(b => !b.IsDeleted)
                .OrderBy(b => b.CreatedOnUtc)
                .ThenBy(b => b.Id)
                .Select(b => new { b.Id, b.CreatedOnUtc })
                .FirstOrDefaultAsync(cancellationToken);

            if (firstMember is null)
            {
                return new PagedMembersResponse()
                {
                    Members = [],
                    HasMore = false
                };
            }

            cursorMemberId = firstMember.Id;
            cursorCreatedOnUtc = firstMember.CreatedOnUtc;
        }
        else
        {
            var cursorMember = await context.Books
                .Where(b => b.Id == cursorMemberId.Value && !b.IsDeleted)
                .Select(b => new { b.CreatedOnUtc })
                .FirstOrDefaultAsync(cancellationToken);

            if (cursorMember is null)
            {
                return Result.Failure<PagedMembersResponse>(MemberErrors.NotFound(cursorMemberId.Value));
            }

            cursorCreatedOnUtc = cursorMember.CreatedOnUtc;
        }
        
        int fetchSize = pageSize + 1;

        IQueryable<Member> membersQuery = context.Members.Where(b => !b.IsDeleted);

        if (direction == Forward)
        {
            membersQuery = membersQuery
                .Where(b => b.CreatedOnUtc > cursorCreatedOnUtc ||
                           (b.CreatedOnUtc == cursorCreatedOnUtc && b.Id.CompareTo(cursorMemberId.Value) > 0))
                .OrderBy(b => b.CreatedOnUtc)
                .ThenBy(b => b.Id);
        }
        else
        {
            membersQuery = membersQuery
                .Where(b => b.CreatedOnUtc < cursorCreatedOnUtc ||
                    (b.CreatedOnUtc == cursorCreatedOnUtc && b.Id.CompareTo(cursorMemberId.Value) < 0))
                .OrderByDescending(b => b.CreatedOnUtc)
                .ThenByDescending(b => b.Id);
        }

        List<MemberResponse> members = await membersQuery
            .Take(fetchSize)
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
            .ToListAsync(cancellationToken);


        bool hasMore = members.Count > pageSize;
        if (hasMore)
        {
            members = members.Take(pageSize).ToList();
        }
        
        if (direction == Backward)
        {
            members.Reverse();
        }

        var response = new PagedMembersResponse
        {
            Members = members,
            HasMore = hasMore
        };

        return response;
    }
}

