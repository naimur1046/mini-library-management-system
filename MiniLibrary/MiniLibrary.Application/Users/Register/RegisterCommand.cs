using MiniLibrary.Domain.Users;
using MiniLibrary.Application.Abstractions.Messaging;

namespace MiniLibrary.Application.Users.Register;

public sealed class RegisterCommand : ICommand<Guid>
{
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public Role Role { get; set; } = Role.User;
}
