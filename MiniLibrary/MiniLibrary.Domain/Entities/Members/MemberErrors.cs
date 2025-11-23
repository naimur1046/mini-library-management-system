using MiniLibrary.SharedKernel;

namespace Domain.Members;

public static class MemberErrors
{
    public static Error NotFound(Guid id) => Error.NotFound(
        "Members.NotFound",
        $"The member with Id '{id}' was not found.");

    public static Error EmailAlreadyExists(string email) => Error.Conflict(
        "Members.EmailAlreadyExists",
        $"A member with the email '{email}' already exists.");

    public static Error InvalidJoinDate() => Error.Problem(
        "Members.InvalidJoinDate", "A member with the email '{email}' already exists.");
}