using FluentValidation;

namespace MiniLibrary.Application.Books.Create;

internal sealed class CreateBookCommandValidator : AbstractValidator<CreateBookCommand>
{
    public CreateBookCommandValidator()
    {
        RuleFor(c => c.Title)
            .NotEmpty()
            .WithMessage("Title is required")
            .MaximumLength(200)
            .WithMessage("Title must not exceed 200 characters");

        RuleFor(c => c.Author)
            .NotEmpty()
            .WithMessage("Author is required")
            .MaximumLength(100)
            .WithMessage("Author must not exceed 100 characters");

        RuleFor(c => c.ISBN)
            .NotEmpty()
            .WithMessage("ISBN is required")
            .MaximumLength(20)
            .WithMessage("ISBN must not exceed 20 characters")
            .Matches(@"^(?:ISBN(?:-1[03])?:? )?(?=[0-9X]{10}$|(?=(?:[0-9]+[- ]){3})[- 0-9X]{13}$|97[89][0-9]{10}$|(?=(?:[0-9]+[- ]){4})[- 0-9]{17}$)(?:97[89][- ]?)?[0-9]{1,5}[- ]?[0-9]+[- ]?[0-9]+[- ]?[0-9X]$")
            .WithMessage("ISBN format is invalid");

        RuleFor(c => c.Category)
            .NotEmpty()
            .WithMessage("Category is required")
            .MaximumLength(50)
            .WithMessage("Category must not exceed 50 characters");

        RuleFor(c => c.CopiesAvailable)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Copies available must be greater than or equal to 0");

        RuleFor(c => c.PublishedYear)
            .GreaterThan(1000)
            .WithMessage("Published year must be a valid year")
            .LessThanOrEqualTo(DateTime.UtcNow.Year)
            .WithMessage("Published year cannot be in the future");
    }
}
