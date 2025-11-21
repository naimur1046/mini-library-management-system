using MiniLibrary.Application.Abstractions.Messaging;

namespace MiniLibrary.Application.Borrowings.Return;

public class ReturnBookCommand : ICommand<Guid>
{
    public Guid BorrowId { get; set; }
    public Guid BookId { get; set; }
}