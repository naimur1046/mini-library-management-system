using MiniLibrary.Application.Abstractions.Messaging;
namespace MiniLibrary.Application.Members.Create;

public sealed class CreateMemberCommand : ICommand<Guid>
{
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public DateTime JoinDate { get; set; }
    public bool IsActive { get; set; }
    public string CreatedBy { get; set; } = "System";
}