using MiniLibrary.Application.Abstractions.Messaging;

namespace MiniLibrary.Application.Members.Update;

public class UpdateMemberCommand : ICommand<Guid>
{
    public Guid MemberId { get; set; }
    public string? FullName { get; set; } = string.Empty;
    public string? Email { get; set; } = string.Empty;
    public string? Phone { get; set; } = string.Empty;
    public DateTime? JoinDate { get; set; }
    public bool? IsActive { get; set; }
}