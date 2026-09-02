// Application/Features/PagePermissions/Commands/UpdateRolesPermissions/UpdateRolesPermissionsCommandValidator.cs
using FluentValidation;

namespace QrAssignment.Application.Features.PagePermissions.Commands.UpdateRolesPermissions
{
    public sealed class UpdateRolesPermissionsCommandValidator
        : AbstractValidator<UpdateRolesPermissionsCommand>
    {
        public UpdateRolesPermissionsCommandValidator()
        {
            RuleFor(x => x.RoleIds)
                .NotEmpty().WithMessage("En az bir rol seçilmelidir.");

            RuleForEach(x => x.Permissions).ChildRules(p =>
            {
                p.RuleFor(a => a)
                    .Must(a => !string.IsNullOrEmpty(a.PageName) || !string.IsNullOrEmpty(a.GroupKey))
                    .WithMessage("Her yetki satırı ya bir PageName ya da bir GroupKey içermelidir.");
            });
        }
    }
}