using FluentValidation;

namespace MiniLibrary.Application.Books.GetById;

internal sealed class GetByIdBookQueryValidator : AbstractValidator<GetByIdBookQuery>
{
    public GetByIdBookQueryValidator()
    {
        RuleFor(q => q.BookId)
            .NotEmpty()
            .WithMessage("Book ID is required");
    }
}