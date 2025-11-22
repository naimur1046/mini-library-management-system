using Domain.Books;
using Microsoft.EntityFrameworkCore;
using MiniLibrary.Application.Abstractions.Data;
using MiniLibrary.Application.Abstractions.Messaging;
using MiniLibrary.Application.Books.Get;
using MiniLibrary.SharedKernel;

namespace MiniLibrary.Application.Books.GetById;

internal sealed class GetByIdBookQueryHandler(IApplicationDbContext context)
    : IQueryHandler<GetByIdBookQuery, BookResponse>
{
    public async Task<Result<BookResponse>> Handle(
        GetByIdBookQuery query,
        CancellationToken cancellationToken)
    {
        BookResponse? book = await context.Books
            .Where(b => b.Id == query.BookId && !b.IsDeleted)
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
            .FirstOrDefaultAsync(cancellationToken);

        if (book is null)
        {
            return Result.Failure<BookResponse>(BookErrors.NotFound(query.BookId));
        }

        return book;
    }
}