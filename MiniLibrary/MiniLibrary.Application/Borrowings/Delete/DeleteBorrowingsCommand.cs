using MiniLibrary.Application.Abstractions.Messaging;

namespace MiniLibrary.Application.Borrowings.Delete;

public class DeleteBorrowingsCommand : ICommand<Guid>
{
    public Guid Id { get; set; }

    public DeleteBorrowingsCommand(Guid id)
    {
        Id = id;
    }
}