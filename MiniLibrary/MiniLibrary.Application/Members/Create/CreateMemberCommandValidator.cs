using FluentValidation;

namespace MiniLibrary.Application.Members.Create;

internal sealed class CreateMemberCommandValidator : AbstractValidator<CreateMemberCommand>
{
    public CreateMemberCommandValidator()
    {
        RuleFor(c => c.FullName)
            .NotEmpty()
            .WithMessage("Full name is required")
            .MaximumLength(200)
            .WithMessage("Full name must not exceed 200 characters");

        RuleFor(c => c.Email)
            .NotEmpty()
            .WithMessage("Email is required")
            .EmailAddress()
            .WithMessage("Email must be a valid email address")
            .MaximumLength(100)
            .WithMessage("Email must not exceed 100 characters");

        RuleFor(c => c.Phone)
            .NotEmpty()
            .WithMessage("Phone number is required")
            .Matches(@"^(?:\+8801|01)[3-9]\d{8}$")
            .WithMessage("Phone number must be a valid Bangladeshi number (e.g., 01XXXXXXXXX or +8801XXXXXXXXX)");

        RuleFor(c => c.JoinDate)
            .LessThanOrEqualTo(DateTime.UtcNow)
            .WithMessage("Join date cannot be in the future");

        RuleFor(c => c.IsActive)
            .NotNull()
            .WithMessage("IsActive must be specified");
    }
}
