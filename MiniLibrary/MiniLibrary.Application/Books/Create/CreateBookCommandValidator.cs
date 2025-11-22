using FluentValidation;

namespace MiniLibrary.Application.Books.Create;

internal sealed class CreateBookCommandValidator : AbstractValidator<CreateBookCommand>
{
    public CreateBookCommandValidator()
    {
        RuleFor(c => c.Title)
            .NotEmpty()
            .WithMessage("Title is required");

        RuleFor(c => c.Author)
            .NotEmpty()
            .WithMessage("Author is required");

        RuleFor(c => c.ISBN)
            .NotEmpty()
            .WithMessage("ISBN is required");

        RuleFor(c => c.Category)
            .NotEmpty()
            .WithMessage("Category is required")
            .MaximumLength(50)
            .WithMessage("Category must not exceed 50 characters");

        RuleFor(c => c.CopiesAvailable)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Copies available must be greater than or equal to 0");

        RuleFor(c => c.PublishedYear)
            .LessThanOrEqualTo(DateTime.UtcNow.Year)
            .WithMessage("Published year cannot be in the future");
    }
}
