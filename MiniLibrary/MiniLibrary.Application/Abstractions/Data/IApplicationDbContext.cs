using Domain.Books;
using Domain.Borrows;
using Domain.EmailLogs;
using Domain.Members;
using Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace MiniLibrary.Application.Abstractions.Data;

public interface IApplicationDbContext
{
    DbSet<Book> Books { get; }
    DbSet<Member> Members { get; }
    DbSet<Borrow> Borrows { get; }
    DbSet<BorrowItem> BorrowItems { get; }
    DbSet<User> Users { get; }
    DbSet<EmailLog> EmailLogs { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
