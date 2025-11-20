using FluentValidation;

namespace MiniLibrary.Application.Members.GetById;

internal sealed class GetByIdMemberQueryValidator : AbstractValidator<GetByIdMemberQuery>
{
    public GetByIdMemberQueryValidator()
    {
        RuleFor(q => q.MemberId)
            .NotEmpty()
            .WithMessage("Member ID is required");
    }
}