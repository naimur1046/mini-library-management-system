using Microsoft.EntityFrameworkCore;
using MiniLibrary.Application.Abstractions.Data;
using MiniLibrary.Application.Abstractions.Messaging;
using SharedKernel;

namespace MiniLibrary.Application.Borrowings.GetOverdue;

internal sealed class GetOverdueBorrowingsQueryHandler(
    IApplicationDbContext context,
    IDateTimeProvider dateTimeProvider)
    : IQueryHandler<GetOverdueBorrowingsQuery, List<OverdueBorrowingResponse>>
{
    public async Task<Result<List<OverdueBorrowingResponse>>> Handle(
        GetOverdueBorrowingsQuery query,
        CancellationToken cancellationToken)
    {
        DateTime now = dateTimeProvider.UtcNow;

        var overdueBorrowings = await context.Borrows
            .Include(b => b.Member)
            .Include(b => b.BorrowItems)
                .ThenInclude(bi => bi.Book)
            .Where(b => b.DueDate < now &&
                       b.BorrowItems.Any(bi => bi.ReturnDate == null))
            .Select(b => new OverdueBorrowingResponse
            {
                BorrowId = b.Id,
                MemberId = b.MemberId,
                MemberName = b.Member.FullName,
                MemberEmail = b.Member.Email,
                DueDate = b.DueDate,
                DaysOverdue = (int)(now - b.DueDate).TotalDays,
                OverdueBooks = b.BorrowItems
                    .Where(bi => bi.ReturnDate == null)
                    .Select(bi => new OverdueBookItem
                    {
                        BookId = bi.BookId,
                        BookTitle = bi.Book.Title,
                        BookAuthor = bi.Book.Author
                    })
                    .ToList()
            })
            .ToListAsync(cancellationToken);

        return overdueBorrowings;
    }
}
