using MiniLibrary.Application.Abstractions.Messaging;

namespace MiniLibrary.Application.Members.Delete;

public class DeleteMemberCommand : ICommand<Guid>
{
    public Guid Id { get; set; }

    public DeleteMemberCommand(Guid id)
    {
        Id = id;
    }
}