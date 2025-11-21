using Domain.Books;
using Microsoft.EntityFrameworkCore;
using MiniLibrary.Application.Abstractions.Data;
using MiniLibrary.Application.Abstractions.Messaging;
using SharedKernel;

namespace MiniLibrary.Application.Books.Update;

internal sealed class UpdateBookCommandHandler(
    IApplicationDbContext context,
    IDateTimeProvider dateTimeProvider)
    : ICommandHandler<UpdateBookCommand, Guid>
{
    public async Task<Result<Guid>> Handle(UpdateBookCommand command, CancellationToken cancellationToken)
    {
        Book? book = await context.Books
            .FirstOrDefaultAsync(b => b.Id == command.BookId && !b.IsDeleted, cancellationToken);

        if (book is null)
        {
            return Result.Failure<Guid>(BookErrors.NotFound(command.BookId));
        }
        
        if (!string.IsNullOrWhiteSpace(command.ISBN) && command.ISBN != book.ISBN)
        {
            bool isbnExists = await context.Books
                .AnyAsync(b => b.ISBN == command.ISBN && b.Id != command.BookId && !b.IsDeleted, cancellationToken);

            if (isbnExists)
            {
                return Result.Failure<Guid>(BookErrors.ISBNAlreadyExists(command.ISBN));
            }
        }
        
        if (!string.IsNullOrWhiteSpace(command.Title))
        {
            book.Title = command.Title;
        }

        if (!string.IsNullOrWhiteSpace(command.Author))
        {
            book.Author = command.Author;
        }

        if (!string.IsNullOrWhiteSpace(command.ISBN))
        {
            book.ISBN = command.ISBN;
        }

        if (!string.IsNullOrWhiteSpace(command.Category))
        {
            book.Category = command.Category;
        }

        if (command.CopiesAvailable.HasValue)
        {
            if (command.CopiesAvailable.Value < 0)
            {
                return Result.Failure<Guid>(BookErrors.InvalidCopiesAvailable());
            }
            book.CopiesAvailable = command.CopiesAvailable.Value;
            book.IsAvailable = command.CopiesAvailable.Value > 0;
        }

        if (command.PublishedYear.HasValue)
        {
            if (command.PublishedYear.Value < 0 || command.PublishedYear.Value > DateTime.UtcNow.Year + 1)
            {
                return Result.Failure<Guid>(BookErrors.InvalidPublishedYear());
            }
            book.PublishedYear = command.PublishedYear.Value;
        }
        
        book.ModifiedOnUtc = dateTimeProvider.UtcNow;
        book.ModifiedBy = "System";

        await context.SaveChangesAsync(cancellationToken);

        return book.Id;
    }
}
