using MiniLibrary.SharedKernel;

namespace MiniLibrary.Domain.Users;

public static class UserErrors
{
    public static Error EmailAlreadyExists(string email) => Error.Conflict(
        "Users.EmailAlreadyExists",
        $"A user with email '{email}' already exists");

    public static Error InvalidCredentials() => Error.Problem(
        "Users.InvalidCredentials",
        "The provided email or password is incorrect");

    public static Error InactiveAccount() => Error.Problem(
        "Users.InactiveAccount",
        "Your account has been deactivated. Please contact an administrator");
}
