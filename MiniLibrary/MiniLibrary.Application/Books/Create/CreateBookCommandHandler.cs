using Domain.Books;
using Microsoft.EntityFrameworkCore;
using MiniLibrary.Application.Abstractions.Data;
using MiniLibrary.Application.Abstractions.Messaging;
using SharedKernel;

namespace MiniLibrary.Application.Books.Create;

internal sealed class CreateBookCommandHandler(
    IApplicationDbContext context,
    IDateTimeProvider dateTimeProvider)
    : ICommandHandler<CreateBookCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateBookCommand command, CancellationToken cancellationToken)
    {
        bool isbnExists = await context.Books
            .AnyAsync(b => b.ISBN == command.ISBN && !b.IsDeleted, cancellationToken);

        if (isbnExists)
        {
            return Result.Failure<Guid>(BookErrors.ISBNAlreadyExists(command.ISBN));
        }

        var book = new Book
        {
            Title = command.Title,
            Author = command.Author,
            ISBN = command.ISBN,
            Category = command.Category,
            CopiesAvailable = command.CopiesAvailable,
            PublishedYear = command.PublishedYear,
            IsAvailable = command.CopiesAvailable > 0,
            IsDeleted = false
        };

        context.Books.Add(book);

        await context.SaveChangesAsync(cancellationToken);

        return book.Id;
    }
}
