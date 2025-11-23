using FluentValidation;

namespace MiniLibrary.Application.Books.Update;

internal sealed class UpdateBookCommandValidator : AbstractValidator<UpdateBookCommand>
{
    public UpdateBookCommandValidator()
    {
        RuleFor(c => c.BookId)
            .NotEmpty()
            .WithMessage("Book ID is required");
        
        RuleFor(c => c)
            .Must(command =>
                !string.IsNullOrWhiteSpace(command.Title) ||
                !string.IsNullOrWhiteSpace(command.Author) ||
                !string.IsNullOrWhiteSpace(command.ISBN) ||
                !string.IsNullOrWhiteSpace(command.Category) ||
                command.CopiesAvailable.HasValue ||
                command.PublishedYear.HasValue)
            .WithMessage("At least one property must be provided for update");
        
        When(c => !string.IsNullOrWhiteSpace(c.Title), () =>
        {
            RuleFor(c => c.Title);
        });
        
        When(c => !string.IsNullOrWhiteSpace(c.Author), () =>
        {
            RuleFor(c => c.Author);
        });
        
        When(c => !string.IsNullOrWhiteSpace(c.ISBN), () =>
        {
            RuleFor(c => c.ISBN);
        });
        
        When(c => !string.IsNullOrWhiteSpace(c.Category), () =>
        {
            RuleFor(c => c.Category)
                .MaximumLength(50)
                .WithMessage("Category must not exceed 50 characters");
        });
        
        When(c => c.CopiesAvailable.HasValue, () =>
        {
            RuleFor(c => c.CopiesAvailable!.Value)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Copies available must be greater than or equal to 0");
        });
        
        When(c => c.PublishedYear.HasValue, () =>
        {
            RuleFor(c => c.PublishedYear!.Value)
                .GreaterThan(0)
                .WithMessage("Published year must be greater than 0")
                .LessThanOrEqualTo(DateTime.UtcNow.Year + 1)
                .WithMessage("Published year cannot be more than one year in the future");
        });
    }
}
