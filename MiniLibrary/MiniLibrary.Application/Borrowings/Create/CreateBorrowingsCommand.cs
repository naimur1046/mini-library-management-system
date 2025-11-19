using MiniLibrary.Application.Abstractions.Messaging;

namespace MiniLibrary.Application.Borrowings.Create;

public class CreateBorrowingsCommand : ICommand<Guid>
{
    public Guid MemberId { get; set; }
    public DateTime BorrowDate { get; set; }
    public DateTime DueDate { get; set; }
    public List<Guid> BookIds { get; set; } = new();
}