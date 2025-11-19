using MiniLibrary.Application.Abstractions.Messaging;
namespace MiniLibrary.Application.Books.Delete;
public sealed class DeleteBookCommand : ICommand<Guid>
{
    public Guid Id { get; set; }

    public DeleteBookCommand(Guid id)
    {
        Id = id;
    }
}