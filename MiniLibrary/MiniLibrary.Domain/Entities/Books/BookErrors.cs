using SharedKernel;

namespace Domain.Books;

public static class BookErrors
{
    public static Error NotFound(Guid id) => Error.NotFound(
        "Books.NotFound",
        $"The book with Id '{id}' was not found");

    public static Error ISBNAlreadyExists(string isbn) => Error.Conflict(
        "Books.ISBNAlreadyExists",
        $"A book with ISBN '{isbn}' already exists");

    public static Error InvalidCopiesAvailable() => Error.Problem(
        "Books.InvalidCopiesAvailable",
        "The number of copies available must be greater than or equal to 0");

    public static Error InvalidPublishedYear() => Error.Problem(
        "Books.InvalidPublishedYear",
        "The published year must be a valid year");
}
