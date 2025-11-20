using Domain.Books;
using Microsoft.EntityFrameworkCore;
using MiniLibrary.Application.Abstractions.Data;
using MiniLibrary.Application.Abstractions.Messaging;
using SharedKernel;

namespace MiniLibrary.Application.Books.Get;

internal sealed class GetBooksQueryHandler(IApplicationDbContext context)
    : IQueryHandler<GetBooksQuery, PagedBooksResponse>
{
    private const int DefaultPageSize = 100;
    private const string DirectionUp = "up";
    private const string DirectionDown = "down";

    public async Task<Result<PagedBooksResponse>> Handle(
        GetBooksQuery query,
        CancellationToken cancellationToken)
    {
        int pageSize = query.Size ?? DefaultPageSize;
        string direction = string.IsNullOrWhiteSpace(query.Direction)
            ? DirectionUp
            : query.Direction.ToLowerInvariant();

        // Validate page size
        if (pageSize <= 0 || pageSize > 1000)
        {
            return Result.Failure<PagedBooksResponse>(
                Error.Problem("Books.InvalidPageSize", "Page size must be between 1 and 1000"));
        }

        // Validate direction
        if (direction != DirectionUp && direction != DirectionDown)
        {
            return Result.Failure<PagedBooksResponse>(
                Error.Problem("Books.InvalidDirection", "Direction must be 'up' or 'down'"));
        }

        Guid? cursorBookId = query.LastBookId;
        DateTime? cursorCreatedOnUtc = null;

        // If no cursor is provided, get the first book's ID
        if (!cursorBookId.HasValue)
        {
            var firstBook = await context.Books
                .Where(b => !b.IsDeleted)
                .OrderBy(b => b.CreatedOnUtc)
                .ThenBy(b => b.Id)
                .Select(b => new { b.Id, b.CreatedOnUtc })
                .FirstOrDefaultAsync(cancellationToken);

            if (firstBook is null)
            {
                // No books in the database
                return new PagedBooksResponse
                {
                    Books = [],
                    NextCursor = null,
                    PreviousCursor = null,
                    HasMore = false,
                    TotalReturned = 0
                };
            }

            cursorBookId = firstBook.Id;
            cursorCreatedOnUtc = firstBook.CreatedOnUtc;
        }
        else
        {
            // Get the cursor book's CreatedOnUtc
            var cursorBook = await context.Books
                .Where(b => b.Id == cursorBookId.Value && !b.IsDeleted)
                .Select(b => new { b.CreatedOnUtc })
                .FirstOrDefaultAsync(cancellationToken);

            if (cursorBook is null)
            {
                return Result.Failure<PagedBooksResponse>(BookErrors.NotFound(cursorBookId.Value));
            }

            cursorCreatedOnUtc = cursorBook.CreatedOnUtc;
        }

        // Fetch one extra to determine if there are more results
        int fetchSize = pageSize + 1;

        IQueryable<Book> booksQuery = context.Books.Where(b => !b.IsDeleted);

        if (direction == DirectionUp)
        {
            // Fetch newer books (created after the cursor)
            booksQuery = booksQuery
                .Where(b => b.CreatedOnUtc > cursorCreatedOnUtc ||
                           (b.CreatedOnUtc == cursorCreatedOnUtc && b.Id.CompareTo(cursorBookId.Value) > 0))
                .OrderBy(b => b.CreatedOnUtc)
                .ThenBy(b => b.Id);
        }
        else // DirectionDown
        {
            // Fetch older books (created before the cursor)
            booksQuery = booksQuery
                .Where(b => b.CreatedOnUtc < cursorCreatedOnUtc ||
                           (b.CreatedOnUtc == cursorCreatedOnUtc && b.Id.CompareTo(cursorBookId.Value) < 0))
                .OrderByDescending(b => b.CreatedOnUtc)
                .ThenByDescending(b => b.Id);
        }

        List<BookResponse> books = await booksQuery
            .Take(fetchSize)
            .Select(b => new BookResponse
            {
                Id = b.Id,
                Title = b.Title,
                Author = b.Author,
                ISBN = b.ISBN,
                Category = b.Category,
                CopiesAvailable = b.CopiesAvailable,
                PublishedYear = b.PublishedYear,
                IsAvailable = b.IsAvailable,
                CreatedOnUtc = b.CreatedOnUtc
            })
            .ToListAsync(cancellationToken);

        bool hasMore = books.Count > pageSize;
        if (hasMore)
        {
            books = books.Take(pageSize).ToList();
        }

        // If fetching down, reverse the results to maintain chronological order
        if (direction == DirectionDown)
        {
            books.Reverse();
        }

        Guid? nextCursor = null;
        Guid? previousCursor = null;

        if (books.Count > 0)
        {
            if (direction == DirectionUp)
            {
                nextCursor = hasMore ? books[^1].Id : null;
                previousCursor = books[0].Id;
            }
            else // DirectionDown
            {
                nextCursor = books[^1].Id;
                previousCursor = hasMore ? books[0].Id : null;
            }
        }

        var response = new PagedBooksResponse
        {
            Books = books,
            NextCursor = nextCursor,
            PreviousCursor = previousCursor,
            HasMore = hasMore,
            TotalReturned = books.Count
        };

        return response;
    }
}
