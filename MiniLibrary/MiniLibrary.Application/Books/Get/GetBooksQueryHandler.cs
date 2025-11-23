using Domain.Books;
using Microsoft.EntityFrameworkCore;
using MiniLibrary.Application.Abstractions.Data;
using MiniLibrary.Application.Abstractions.Messaging;
using MiniLibrary.SharedKernel;

namespace MiniLibrary.Application.Books.Get;

internal sealed class GetBooksQueryHandler(IApplicationDbContext context)
    : IQueryHandler<GetBooksQuery, PagedBooksResponse>
{
    private const int DefaultPageSize = 100;
    private const string Forward = "Forward";
    private const string Backward = "Backward";

    public async Task<Result<PagedBooksResponse>> Handle(
        GetBooksQuery query,
        CancellationToken cancellationToken)
    {
        int pageSize = query.Size ?? DefaultPageSize;
        string direction = string.IsNullOrWhiteSpace(query.Direction)
            ? Forward
            : query.Direction.ToLowerInvariant();
        
        if (pageSize <= 0 || pageSize > 1000)
        {
            return Result.Failure<PagedBooksResponse>(
                Error.Problem("Books.InvalidPageSize", "Page size must be between 1 and 1000"));
        }
        
        if (direction != Forward && direction != Backward)
        {
            return Result.Failure<PagedBooksResponse>(
                Error.Problem("Books.InvalidDirection", "Direction must be 'Forward' or 'Backword'"));
        }
        
        IQueryable<Book> baseQuery = context.Books.Where(b => !b.IsDeleted && b.IsAvailable);
        
        if (!string.IsNullOrWhiteSpace(query.Title))
        {
            baseQuery = baseQuery.Where(b => b.Title.ToLower().Contains(query.Title.ToLower()));
        }

        if (!string.IsNullOrWhiteSpace(query.Category))
        {
            baseQuery = baseQuery.Where(b => b.Category.ToLower().Contains(query.Category.ToLower()));
        }

        if (!string.IsNullOrWhiteSpace(query.ISBN))
        {
            baseQuery = baseQuery.Where(b => b.ISBN.Contains(query.ISBN));
        }

        Guid? cursorBookId = query.LastBookId;
        DateTime? cursorCreatedOnUtc = null;

        if (!cursorBookId.HasValue)
        {
            var firstBook = await baseQuery
                .OrderBy(b => b.CreatedOnUtc)
                .ThenBy(b => b.Id)
                .Select(b => new { b.Id, b.CreatedOnUtc })
                .FirstOrDefaultAsync(cancellationToken);

            if (firstBook is null)
            {
                return new PagedBooksResponse
                {
                    Books = [],
                    HasMore = false
                };
            }

            cursorBookId = firstBook.Id;
            cursorCreatedOnUtc = firstBook.CreatedOnUtc;
        }
        else
        {
            var cursorBook = await baseQuery
                .Where(b => b.Id == cursorBookId.Value)
                .Select(b => new { b.CreatedOnUtc })
                .FirstOrDefaultAsync(cancellationToken);

            if (cursorBook is null)
            {
                return Result.Failure<PagedBooksResponse>(BookErrors.NotFound(cursorBookId.Value));
            }

            cursorCreatedOnUtc = cursorBook.CreatedOnUtc;
        }

        int fetchSize = pageSize + 1;

        IQueryable<Book> booksQuery = baseQuery;

        if (direction == Forward)
        {
            booksQuery = booksQuery
                .Where(b => b.CreatedOnUtc > cursorCreatedOnUtc ||
                           (b.CreatedOnUtc == cursorCreatedOnUtc && b.Id.CompareTo(cursorBookId.Value) > 0))
                .OrderBy(b => b.CreatedOnUtc)
                .ThenBy(b => b.Id);
        }
        else
        {
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
        
        if (direction == Backward)
        {
            books.Reverse();
        }

        var response = new PagedBooksResponse
        {
            Books = books,
            HasMore = hasMore
        };

        return response;
    }
}
