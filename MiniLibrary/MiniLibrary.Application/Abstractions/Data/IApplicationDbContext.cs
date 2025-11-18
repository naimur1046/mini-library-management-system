using Domain.Books;
using Domain.Borrows;
using Domain.Members;
using Microsoft.EntityFrameworkCore;

namespace MiniLibrary.Application.Abstractions.Data;

public interface IApplicationDbContext
{
    DbSet<Book> Books { get; }
    DbSet<Member> Members { get; }
    DbSet<Borrow> Borrows { get; }
    DbSet<BorrowItem> BorrowItems { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
