using Domain.Books;
using Microsoft.EntityFrameworkCore;
using MiniLibrary.Application.Abstractions.Data;
using MiniLibrary.Application.Abstractions.Messaging;
using MiniLibrary.SharedKernel;

namespace MiniLibrary.Application.Books.Delete;

internal sealed class DeleteBookCommandHandler(
    IApplicationDbContext context,
    IDateTimeProvider dateTimeProvider)
    : ICommandHandler<DeleteBookCommand, Guid>
{
    public async Task<Result<Guid>> Handle(DeleteBookCommand command, CancellationToken cancellationToken)
    {
        var book = await context.Books
            .FirstOrDefaultAsync(
                b => b.Id == command.Id && !b.IsDeleted,
                cancellationToken);

        if (book is null)
        {
            return Result.Failure<Guid>(BookErrors.NotFound(command.Id));
        }
        
        book.IsDeleted = true;
        book.IsAvailable = false;

        await context.SaveChangesAsync(cancellationToken);

        return book.Id;
    }
}