using MiniLibrary.Domain.Books;
using MiniLibrary.Domain.Borrows;
using MiniLibrary.Domain.Members;
using MiniLibrary.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace MiniLibrary.Application.Abstractions.Data;

public interface IApplicationDbContext
{
    DbSet<Book> Books { get; }
    DbSet<Member> Members { get; }
    DbSet<Borrow> Borrows { get; }
    DbSet<BorrowItem> BorrowItems { get; }
    DbSet<User> Users { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
