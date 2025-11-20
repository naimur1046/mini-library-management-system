using FluentValidation;

namespace MiniLibrary.Application.Borrowings.GetSummary;

internal sealed class GetBorrowingSummaryQueryValidator : AbstractValidator<GetBorrowingSummaryQuery>
{
    public GetBorrowingSummaryQueryValidator()
    {
        RuleFor(q => q.StartDate)
            .NotEmpty()
            .WithMessage("Start date is required")
            .LessThanOrEqualTo(q => q.EndDate)
            .WithMessage("Start date must be less than or equal to end date");

        RuleFor(q => q.EndDate)
            .NotEmpty()
            .WithMessage("End date is required")
            .LessThanOrEqualTo(DateTime.UtcNow)
            .WithMessage("End date cannot be in the future");
    }
}
