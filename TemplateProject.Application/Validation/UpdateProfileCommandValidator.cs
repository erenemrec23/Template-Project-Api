using FluentValidation;
using QrAssignment.Application.Features.Profile.Commands;
using QrAssignment.Application.Features.Profile.Commands.Update;

namespace QrAssignment.Application.Features.Profile.Validators
{
    public sealed class UpdateProfileCommandValidator : AbstractValidator<UpdateProfileCommand>
    {
        public UpdateProfileCommandValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty().MinimumLength(2).MaximumLength(250);
            RuleFor(x => x.LastName).NotEmpty().MinimumLength(2).MaximumLength(250);
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
        }
    }

    public sealed class ChangePasswordCommandValidator : AbstractValidator<ChangePasswordCommand>
    {
        public ChangePasswordCommandValidator()
        {
            RuleFor(x => x.CurrentPassword).NotEmpty();
            RuleFor(x => x.NewPassword)
                .NotEmpty()
                .MinimumLength(6)
                .Matches(@"[0-9]").WithMessage("Sifre en az 1 rakam icermeli.");
        }
    }
}