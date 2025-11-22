using MiniLibrary.SharedKernel;

namespace Domain.Borrows;

public static class BorrowErrors
{
    public static Error NotFound(Guid id) => Error.NotFound(
        "Borrow.NotFound",
        $"The borrow with Id '{id}' was not found.");

    public static Error MemberNotFound(Guid memberId) => Error.NotFound(
        "Borrow.MemberNotFound",
        $"The member with Id '{memberId}' was not found.");

    public static Error BookNotFound(Guid bookId) => Error.NotFound(
        "Borrow.BookNotFound",
        $"The book with Id '{bookId}' was not found.");

    public static Error SomeBooksNotFound => Error.NotFound(
        "Borrow.SomeBooksNotFound",
        "One or more books in the request do not exist.");

    public static Error BookNotAvailable(Guid bookId) => Error.Conflict(
        "Borrow.BookNotAvailable",
        $"The book with Id '{bookId}' is not available for borrowing.");
    public static Error BookNotInBorrow => Error.Conflict(
        "Borrow.BookNotInBorrow",
        $"The book is not in the borrow");

    public static Error BookAlreadyReturned(Guid bookId) => Error.Conflict(
        "Borrow.BookAlreadyReturned",
        $"The book with Id '{bookId}' has already been returned.");

    public static Error InvalidDueDate => Error.Problem(
        "Borrow.InvalidDueDate",
        "Due date must be greater than or equal to the borrow date.");
}