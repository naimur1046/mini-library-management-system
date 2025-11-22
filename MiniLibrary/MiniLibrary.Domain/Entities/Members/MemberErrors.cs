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

    public static Error InvalidFullName() => Error.Problem(
        "Members.InvalidFullName",
        "The full name must not be empty and should be valid.");

    public static Error InvalidPhone() => Error.Problem(
        "Members.InvalidPhone",
        "The phone number must be valid and not empty.");

    public static Error InvalidJoinDate() => Error.Problem(
        "Members.InvalidJoinDate",
        "The join date must be a valid date in the past or today.");
}