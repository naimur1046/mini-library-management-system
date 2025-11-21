using MiniLibrary.Application.Abstractions.Messaging;

namespace MiniLibrary.Application.Users.Login;

public sealed class LoginCommand : ICommand<string>
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
