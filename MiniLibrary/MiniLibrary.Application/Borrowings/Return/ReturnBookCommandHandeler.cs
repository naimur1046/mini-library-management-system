using Domain.Borrows;
using Microsoft.EntityFrameworkCore;
using MiniLibrary.Application.Abstractions.Data;
using MiniLibrary.Application.Abstractions.Messaging;
using MiniLibrary.SharedKernel;

namespace MiniLibrary.Application.Borrowings.Return;

internal sealed class ReturnBookCommandHandler(
    IApplicationDbContext context,
    IDateTimeProvider dateTimeProvider)
    : ICommandHandler<ReturnBookCommand, Guid>
{
    public async Task<Result<Guid>> Handle(ReturnBookCommand command, CancellationToken cancellationToken)
    {
        var borrow = await context.Borrows
            .Include(b => b.BorrowItems)
            .FirstOrDefaultAsync(b => b.Id == command.BorrowId, cancellationToken);

        if (borrow is null)
        {
            return Result.Failure<Guid>(BorrowErrors.NotFound(command.BorrowId));
        }
        
        var borrowItem = borrow.BorrowItems
            .FirstOrDefault(i => i.BookId == command.BookId);

        if (borrowItem is null)
        {
            return Result.Failure<Guid>(BorrowErrors.BookNotInBorrow);
        }
        
        if (borrowItem.ReturnDate != null)
        {
            return Result.Failure<Guid>(BorrowErrors.BookAlreadyReturned(command.BookId));
        }
        
        var book = await context.Books
            .FirstOrDefaultAsync(b => b.Id == command.BookId && !b.IsDeleted, cancellationToken);

        if (book is null)
        {
            return Result.Failure<Guid>(BorrowErrors.BookNotFound(command.BookId));
        }
        
        borrowItem.ReturnDate = dateTimeProvider.UtcNow;
        borrowItem.ModifiedOnUtc = dateTimeProvider.UtcNow;
        borrowItem.ModifiedBy = "System";
        
        book.CopiesAvailable++;
        book.IsAvailable = book.CopiesAvailable > 0;
        book.ModifiedOnUtc = dateTimeProvider.UtcNow;
        book.ModifiedBy = "System";
        
        await context.SaveChangesAsync(cancellationToken);

        return borrow.Id;
    }
}
