// Application/Features/Permission/Commands/UpdatePagePermissionsForPage/UpdatePagePermissionsForPageCommandValidator.cs
using FluentValidation;
using QrAssignment.Application.Features.PagePermissions.Queries;

namespace QrAssignment.Application.Features.PagePermissions.Commands.Update
{
    public sealed class UpdatePagePermissionsForPageCommandValidator
        : AbstractValidator<UpdatePagePermissionsForPageCommand>
    {
        public UpdatePagePermissionsForPageCommandValidator()
        {
            RuleFor(x => x.PageKey).NotEmpty();

            RuleForEach(x => x.Permissions).ChildRules(assignment =>
            {
                //assignment.RuleFor(a => a)
                //    .Must(a => (a.UserId.HasValue && !a.RoleId.HasValue)
                //            || (!a.UserId.HasValue && a.RoleId.HasValue))
                //    .WithMessage("Her yetki ataması ya bir kullanıcıya ya da bir role ait olmalıdır, ikisi birden veya hiçbiri olamaz.");
            });
        }
    }
}