using FluentValidation;

namespace MiniLibrary.Application.Members.Update;

internal sealed class UpdateMemberCommandValidator : AbstractValidator<UpdateMemberCommand>
{
    public UpdateMemberCommandValidator()
    {
        RuleFor(c => c.MemberId)
            .NotEmpty()
            .WithMessage("Member ID is required");

        // Only validate fields if provided

        RuleFor(c => c.FullName)
            .MaximumLength(200)
            .When(c => !string.IsNullOrWhiteSpace(c.FullName))
            .WithMessage("Full name must not exceed 200 characters");

        RuleFor(c => c.Email)
            .EmailAddress()
            .When(c => !string.IsNullOrWhiteSpace(c.Email))
            .WithMessage("Email must be a valid email address");

        RuleFor(c => c.Phone)
            .Matches(@"^(?:\+8801|01)[3-9]\d{8}$")
            .When(c => !string.IsNullOrWhiteSpace(c.Phone))
            .WithMessage("Phone number must be a valid Bangladeshi number");

        RuleFor(c => c.JoinDate)
            .LessThanOrEqualTo(DateTime.UtcNow)
            .When(c => c.JoinDate.HasValue)
            .WithMessage("Join Date cannot be in the future");
    }
}