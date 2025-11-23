using Domain.Borrows;
using Microsoft.EntityFrameworkCore;
using MiniLibrary.Application.Abstractions.Authentication;
using MiniLibrary.Application.Abstractions.Data;
using MiniLibrary.Application.Abstractions.Messaging;
using MiniLibrary.SharedKernel;

namespace MiniLibrary.Application.Borrowings.Create;

internal sealed class CreateBorrowingsCommandHandler(
    IApplicationDbContext context,
    IDateTimeProvider dateTimeProvider,
    IUserContext userContext)
    : ICommandHandler<CreateBorrowingsCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateBorrowingsCommand command, CancellationToken cancellationToken)
    {
        bool memberExists = await context.Members
            .AnyAsync(m => m.Id == command.MemberId && !m.IsDeleted, cancellationToken);

        if (!memberExists)
        {
            return Result.Failure<Guid>(BorrowErrors.MemberNotFound(command.MemberId));
        }
        
        var books = await context.Books
            .Where(b => command.BookIds.Contains(b.Id) && !b.IsDeleted && b.IsAvailable)
            .ToListAsync(cancellationToken);

        if (books.Count != command.BookIds.Count)
        {
            return Result.Failure<Guid>(BorrowErrors.SomeBooksNotFound);
        }

        if (books.Count > 5)
        {
            return Result.Failure<Guid>(BorrowErrors.LimitExitedBorrowing());
        }
        
        foreach (var book in books)
        {
            if (book.CopiesAvailable <= 0 || !book.IsAvailable)
            {
                return Result.Failure<Guid>(BorrowErrors.BookNotAvailable(book.Id));
            }
        }
        
        var borrow = new Borrow
        {
            MemberId = command.MemberId,
            BorrowDate = command.BorrowDate,
            DueDate = command.DueDate,
            CreatedOnUtc = dateTimeProvider.UtcNow,
            CreatedBy = userContext.Email
        };
        
        foreach (var book in books)
        {
            borrow.BorrowItems.Add(new BorrowItem
            {
                BookId = book.Id,
                ReturnDate = command.ReturnDate,
                CreatedOnUtc = dateTimeProvider.UtcNow,
                CreatedBy = userContext.Email
            });
            
            book.CopiesAvailable--;
            
            book.IsAvailable = book.CopiesAvailable > 0;
        }
        
        context.Borrows.Add(borrow);
        await context.SaveChangesAsync(cancellationToken);

        return borrow.Id;
    }
}
