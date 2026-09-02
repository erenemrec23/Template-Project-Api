// Application/Features/PagePermissions/Commands/UpdateUsersPermissions/UpdateUsersPermissionsCommandValidator.cs
using FluentValidation;

namespace QrAssignment.Application.Features.PagePermissions.Commands.UpdateUsersPermissions
{
    public sealed class UpdateUsersPermissionsCommandValidator
        : AbstractValidator<UpdateUsersPermissionsCommand>
    {
        public UpdateUsersPermissionsCommandValidator()
        {
            RuleFor(x => x.UserIds)
                .NotEmpty().WithMessage("En az bir kullanıcı seçilmelidir.");

            RuleForEach(x => x.Permissions).ChildRules(p =>
            {
                p.RuleFor(a => a)
                    .Must(a => !string.IsNullOrEmpty(a.PageName) || !string.IsNullOrEmpty(a.GroupKey))
                    .WithMessage("Her yetki satırı ya bir PageName ya da bir GroupKey içermelidir.");
            });
        }
    }
}