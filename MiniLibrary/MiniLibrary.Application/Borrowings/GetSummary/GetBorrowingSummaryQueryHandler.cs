using Microsoft.EntityFrameworkCore;
using MiniLibrary.Application.Abstractions.Data;
using MiniLibrary.Application.Abstractions.Messaging;
using MiniLibrary.SharedKernel;

namespace MiniLibrary.Application.Borrowings.GetSummary;

internal sealed class GetBorrowingSummaryQueryHandler(IApplicationDbContext context)
    : IQueryHandler<GetBorrowingSummaryQuery, BorrowingSummaryResponse>
{
    public async Task<Result<BorrowingSummaryResponse>> Handle(
        GetBorrowingSummaryQuery query,
        CancellationToken cancellationToken)
    {
        var borrowItemsInRange = await context.BorrowItems
            .Include(bi => bi.Borrow)
            .Include(bi => bi.Book)
            .Where(bi => bi.Borrow.BorrowDate >= query.StartDate &&
                        bi.Borrow.BorrowDate <= query.EndDate)
            .ToListAsync(cancellationToken);
        
        var totalBooksBorrowed = borrowItemsInRange.Count;
        var totalBooksReturned = borrowItemsInRange.Count(bi => bi.ReturnDate.HasValue);
        var activeBorrowRecords = borrowItemsInRange.Count(bi => !bi.ReturnDate.HasValue);
        
        var mostBorrowedBook = borrowItemsInRange
            .GroupBy(bi => new { bi.BookId, bi.Book.Title, bi.Book.Author })
            .Select(g => new
            {
                BookId = g.Key.BookId,
                Title = g.Key.Title,
                Author = g.Key.Author,
                BorrowCount = g.Count()
            })
            .OrderByDescending(x => x.BorrowCount)
            .FirstOrDefault();

        var response = new BorrowingSummaryResponse
        {
            TotalBooksBorrowed = totalBooksBorrowed,
            TotalBooksReturned = totalBooksReturned,
            ActiveBorrowRecords = activeBorrowRecords,
            MostBorrowedBook = mostBorrowedBook != null
                ? new MostBorrowedBookResponse
                {
                    BookId = mostBorrowedBook.BookId,
                    Title = mostBorrowedBook.Title,
                    Author = mostBorrowedBook.Author,
                    BorrowCount = mostBorrowedBook.BorrowCount
                }
                : null
        };

        return response;
    }
}
