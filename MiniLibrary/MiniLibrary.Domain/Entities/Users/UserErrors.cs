using MiniLibrary.SharedKernel;

namespace Domain.Users;

public static class UserErrors
{
    public static Error NotFound(Guid id) => Error.NotFound(
        "Users.NotFound",
        $"The user with Id '{id}' was not found");

    public static Error EmailAlreadyExists(string email) => Error.Conflict(
        "Users.EmailAlreadyExists",
        $"A user with email '{email}' already exists");

    public static Error InvalidCredentials() => Error.Problem(
        "Users.InvalidCredentials",
        "The provided email or password is incorrect");

    public static Error InactiveAccount() => Error.Problem(
        "Users.InactiveAccount",
        "Your account has been deactivated. Please contact an administrator");

    public static Error InvalidEmail() => Error.Problem(
        "Users.InvalidEmail",
        "The email address is not valid");
}
